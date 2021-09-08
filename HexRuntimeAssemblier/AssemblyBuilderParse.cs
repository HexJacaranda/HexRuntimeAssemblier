using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using HexRuntimeAssemblier.IL;
using HexRuntimeAssemblier.Interfaces;
using HexRuntimeAssemblier.Meta;
using HexRuntimeAssemblier.Reference;

namespace HexRuntimeAssemblier
{
    public partial class AssemblyBuilder : IAssemblyBuilder
    {
        private AssemblyHeaderMD mCurrentAssembly;
        private readonly GlobalResolver mResolver;
        private readonly Dictionary<MDRecordKinds, DefinitionTable> mDefTables;
        private readonly Dictionary<MDRecordKinds, ReferenceTable> mRefTables;
        private readonly ReferenceTable mAssemblyRefTable;
        private readonly StringTable mStringTable;
        private readonly static Regex GenericReplace = new(@"\[\d+\]::[@_\w]+");
        private const string CanonicalPlaceHolder = "Canon";
        #region TableAlias
        private DefinitionTable TypeDefTable => mDefTables[MDRecordKinds.TypeDef];
        private DefinitionTable FieldDefTable => mDefTables[MDRecordKinds.FieldDef];
        private DefinitionTable MethodDefTable => mDefTables[MDRecordKinds.MethodDef];
        private DefinitionTable PropertyDefTable => mDefTables[MDRecordKinds.PropertyDef];
        private DefinitionTable EventDefTable => mDefTables[MDRecordKinds.EventDef];
        private DefinitionTable GenericParameterDefTable => mDefTables[MDRecordKinds.GenericParameter];
        private DefinitionTable GenericInstantiationDefTable => mDefTables[MDRecordKinds.GenericInstantiationDef];

        public ReferenceTable AssemblyReferenceTable => throw new NotImplementedException();
        #endregion
        #region NameHelper
        #endregion
        public static string GetPropertyKey(Assemblier.PropertyContext context)
            => context.GetChild<Assemblier.PropertyKeyContext>(0).GetText();
        public static string GetPropertyValue(Assemblier.PropertyContext context)
            => context.GetChild<Assemblier.PropertyValueContext>(0).GetText();
        public static byte MapAccessbility(int tokenType) => tokenType switch
        {
            Assemblier.MODIFIER_PUBLIC => 0,
            Assemblier.MODIFIER_PRIVATE => 1,
            Assemblier.MODIFIER_PROTECTED => 2,
            Assemblier.MODIFIER_INTERNAL => 3,
            _ => throw new Exception()
        };
        /// <summary>
        /// Get generated token from managed string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public uint GetTokenFromString(string value)
            => mStringTable.GetTokenFromString(value);

        #region Canonical Name
        /* Full qualified name(FQN) is used in reference to type, method, field. There are 
         * also two different kinds of such name according to generic.
         * 
         * 1. Type: 
         * [Core]System.Array<System.Int32>, this is called a normal FQN
         * [Core]System.Array<Canon>, this is called a canonical FQN
         * 
         * 2. Method: 
         * int32 [Core]System.Converter<double>::To<int32>(double), this is a normal FQN
         * 
         * NOTE: method requires return type and argument types to be at canonical form as well.
         * Canon [Core]System.Converter<Canon>::To<Canon>(Canon, Canon, Safe<Canon>), this is a canonical FQN
         * 
         * 3. Field:
         * [Core]System.Converter<double>::Some, this is a normal FQN
         * [Core]System.Converter<Canon>::To, this is a canonical FQN
         */
        public static string GetFullQualifiedName(ParserRuleContext memberContext, string referenceJunction, string shortName)
        {
            var stringBuilder = new StringBuilder();
            var current = memberContext.Parent;
            var currentClass = current as Assemblier.ClassDefContext;
           
            stringBuilder.Append(currentClass.typeName().GetText());
            stringBuilder.Append(referenceJunction);
            stringBuilder.Append(shortName);

            //Generic
            if (memberContext is Assemblier.ClassDefContext classDef)
            {
                if (classDef.genericList() != null)
                {
                    var placeHolderCount = classDef.genericList().IDENTIFIER().Length;
                    stringBuilder.Append($"<{ string.Join(", ", Enumerable.Repeat(CanonicalPlaceHolder, placeHolderCount))}>");
                }
            }

            while (current is Assemblier.ClassDefContext)
            {
                currentClass = current as Assemblier.ClassDefContext;
                stringBuilder.Insert(0, '.');
                //Generic
                if (currentClass.genericList() != null)
                {
                    var placeHolderCount = currentClass.genericList().IDENTIFIER().Length;
                    stringBuilder.Insert(0, $"<{ string.Join(", ", Enumerable.Repeat(CanonicalPlaceHolder, placeHolderCount))}>");
                }
                stringBuilder.Insert(0, currentClass.typeName().GetText());
                current = current.Parent;
            }
            
            return stringBuilder.ToString();
        }
        private static string GetFullQualifiedName(Assemblier.MethodDefContext context, string typeFullQualifiedName)
        {
            var methodShortName = context.methodName().GetText();
            var returnType = context.methodReturnType().GetText();
            var arguments = context.methodArgumentList().methodArgument();
            var genericArguments = context.genericList()?.IDENTIFIER();
            string methodSignature = string.Empty;
            if (genericArguments != null && genericArguments.Length > 0)
            {
                methodSignature = $"{methodShortName}<{genericArguments?.Length}>" +
                $"({string.Join(", ", arguments.Select(x => x.type().GetText()))})";
            }
            else
            {
                methodSignature = $"{methodShortName}({string.Join(", ", arguments.Select(x => x.type().GetText()))})";
            }

            //Replace the generic placeholder
            return GenericReplace.Replace(
                $"{returnType} {typeFullQualifiedName}::{methodSignature}",
                CanonicalPlaceHolder);

        }
        private string GetCanonicalFullQualifiedName(Assemblier.TypeContext context)
        {
            var child = context.GetChild(0);
            while (true)
            {               
                switch (child)
                {
                    case Assemblier.ArrayTypeContext array:
                        child = array.GetChild(0);
                        break;
                    case Assemblier.NestArrayTypeContext nest:
                        return $"{nest.ARRAY().GetText()}<{CanonicalPlaceHolder}>";
                    case Assemblier.GenericInstantiationContext generic:
                        return $"{generic.typeRef().GetText()}<" +
                            $"{string.Join(", ", Enumerable.Repeat(CanonicalPlaceHolder, generic.genericParameterList().type().Length))}>";
                    default:
                        return context.GetText();
                }
            }
        }
        private string GetCanonicalFullQualifiedName(Assemblier.GenericInstantiationContext context)
        {
            var canonical = context.typeRef();
            var genericParameterCount = context.genericParameterList().type().Length;
            return $"{canonical.typeName().GetText()}" +
                $"<{string.Join(", ", Enumerable.Repeat(CanonicalPlaceHolder, genericParameterCount))}>";
        }
        private string GetCanonicalFullQualifiedName(Assemblier.MethodRefContext context, 
            out string sourceAssemblyName)
        {
            var returnType = context.methodReturnType().GetText();
            var parentType = context.methodParentType().type();
            var parentTypeRefToken = ResolveType(parentType);
            var typeRefMD = mRefTables[MDRecordKinds.TypeRef].GetMeta<TypeRefMD>(parentTypeRefToken);

            string strippedAssemblyTagTypeName = GetCanonicalFullQualifiedName(parentType);
            sourceAssemblyName = null;
            if (typeRefMD.AssemblyToken != AssemblyRefMD.Self)
            {
                var assembly = mAssemblyRefTable.GetMeta<AssemblyRefMD>(typeRefMD.AssemblyToken);
                sourceAssemblyName = mStringTable.Contents[(int)assembly.AssemblyName];
                //Remove the assembly tag [Assembly Name] in parent type
                string assemblyTag = $"[{sourceAssemblyName}]";
                int assemblyTagIndex = strippedAssemblyTagTypeName.IndexOf(assemblyTag);
                strippedAssemblyTagTypeName = strippedAssemblyTagTypeName.Remove(assemblyTagIndex, assemblyTag.Length);
            }

            StringBuilder builder = new();
            builder.Append($"{returnType} {strippedAssemblyTagTypeName}::{context.methodName().GetText()}");

            //Generic
            if (context.genericParameterList() != null)
                builder.Append($"<{context.genericParameterList().type().Length}>");

            builder.Append($"({string.Join(", ", context.type().Select(x => x.GetText()))})");
            return GenericReplace.Replace(builder.ToString(), CanonicalPlaceHolder);
        }
        #endregion
        public void ResolveStart(Assemblier.StartContext context)
        {
            ResolveAssemblyDef(context.assemblyDef());
            foreach (var classContext in context.classDef())
                ResolveClassDef(classContext);
        }
        public void ResolveAssemblyDef(Assemblier.AssemblyDefContext context)
        {
            var map = context.property().ToDictionary(x => GetPropertyKey(x), x => GetPropertyValue(x));

            mCurrentAssembly = new AssemblyHeaderMD
            {
                NameToken = GetTokenFromString(map["name"]),
                GroupNameToken = GetTokenFromString(map["groupname"]),
                MajorVersion = int.Parse(map["major"]),
                MinorVersion = int.Parse(map["minor"]),
                GUID = Guid.Parse(map["guid"])
            };
        }
        public uint ResolveUnknownTypeForm(IParseTree context)
        {
            return context switch
            {
                Assemblier.TypeContext type => ResolveType(type),
                Assemblier.PrimitiveTypeContext primitiveType => ResolvePrimitiveType(primitiveType),
                Assemblier.ArrayTypeContext arrayType => ResolveArrayType(arrayType),
                Assemblier.NestArrayTypeContext nestArrayType => ResolveArrayType(nestArrayType),
                Assemblier.MultidimensionArrayTypeContext multidimensionArrayType => ResolveArrayType(multidimensionArrayType),
                Assemblier.InteriorRefTypeContext interiorRefType => ResolveInteriorRefType(interiorRefType),
                Assemblier.GenericParameterRefContext genericParam => ResolveGenericParameterRef(genericParam),
                Assemblier.GenericInstantiationContext genericInst => ResolveGenericInstantiation(genericInst),
                _ => throw new TypeResolveException("Unknown type representation"),
            };
        }
        /// <summary>
        /// Resolve the type and get the reference token
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public uint ResolveType(Assemblier.TypeContext context)
        {
            var child = context.GetChild(0);
            return child switch
            {
                Assemblier.PrimitiveTypeContext primitiveType => ResolvePrimitiveType(primitiveType),
                Assemblier.TypeRefContext typeRef => ResolveTypeRef(typeRef),
                Assemblier.InteriorRefTypeContext interiorRefType => ResolveInteriorRefType(interiorRefType),
                Assemblier.ArrayTypeContext arrayType => ResolveArrayType(arrayType),
                Assemblier.GenericParameterRefContext genericParam => ResolveGenericParameterRef(genericParam),
                Assemblier.GenericInstantiationContext genericInst => ResolveGenericInstantiation(genericInst),
                _ => throw new TypeResolveException("Unknown type representation"),
            };
        }
        /// <summary>
        /// Resolve the primitive type and get the reference token
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public uint ResolvePrimitiveType(Assemblier.PrimitiveTypeContext context)
        {
            //Primitive type always requires metadata from core library
            return 0u;
        }
        public uint ResolveArrayType(Assemblier.ArrayTypeContext context)
        {
            var child = context.GetChild(0);
            return child switch
            {
                Assemblier.NestArrayTypeContext nestArray => ResolveArrayType(nestArray),
                Assemblier.MultidimensionArrayTypeContext multi => ResolveArrayType(multi),
                _ => throw new TypeResolveException("Unknown underlying array representation")
            };
        }
        public uint ResolveArrayType(Assemblier.MultidimensionArrayTypeContext context)
        {
            var elementType = context.type();
            var elementRefToken = ResolveType(elementType);
            return GenericParameterDefTable.GetDefinitionToken(context.GetText(), () => new GenericInstantiationMD
            {
                //Multidimension-Array generic ref, need to get from the core library
                CanonicalTypeRefToken = 0u,
                GenericParameterTokens = new uint[] { elementRefToken }
            });
        }
        public uint ResolveArrayType(Assemblier.NestArrayTypeContext context)
        {
            var elementType = context.type();
            var elementRefToken = ResolveType(elementType);

            return GenericParameterDefTable.GetDefinitionToken(context.GetText(), () => new GenericInstantiationMD
            {
                //Array generic ref, need to get from the core library
                CanonicalTypeRefToken = 0u,
                GenericParameterTokens = new uint[] { elementRefToken }
            });
        }
        public uint ResolveInteriorRefType(Assemblier.InteriorRefTypeContext context)
        {
            var internalType = context.GetChild(0);
            var internalRefToken = ResolveUnknownTypeForm(internalType);

            return GenericParameterDefTable.GetDefinitionToken(context.GetText(), () => new GenericInstantiationMD
            {
                //interior generic ref, need to get from the core library
                CanonicalTypeRefToken = 0u,
                GenericParameterTokens = new uint[] { internalRefToken }
            });
        }
        public uint ResolveGenericParameterRef(Assemblier.GenericParameterRefContext context)
        {
            return GenericParameterDefTable.GetDefinitionToken(context.GetText(), () => new GenericParamterMD
            {
                NameToken = GetTokenFromString(context.GetText())
            });
        }
        public uint ResolveGenericInstantiation(Assemblier.GenericInstantiationContext context)
        {
            //Special for canonical full qualified name           
            var assemblyName = context.typeRef().assemblyRef()?.IDENTIFIER().GetText();
            var canonicalTypeName = GetCanonicalFullQualifiedName(context);
            var canonicalRefToken = mResolver.QueryTypeReference(assemblyName, canonicalTypeName,
                defToken => GetReferenceTokenOfType(assemblyName, canonicalTypeName, defToken));

            var parameterTypeTokens = context.genericParameterList().type().Select(x => ResolveType(x)).ToArray();
            return GenericInstantiationDefTable.GetDefinitionToken(context.GetText(), () => new GenericInstantiationMD
            {
                CanonicalTypeRefToken = canonicalRefToken,
                GenericParameterTokens = parameterTypeTokens
            });
        }
        public uint ResolveFieldDef(
            Assemblier.FieldDefContext context,
            string typeFullQualifiedName)
        {
            //Handle the type mapping    
            var fieldShortName = context.IDENTIFIER().GetText();
            var fieldFullQualifiedName = GetFullQualifiedName(context, "::", fieldShortName);

            var fieldDefToken = TryDefineField(fieldFullQualifiedName);
            var field = FieldDefTable[fieldDefToken] as FieldMD;

            field.NameToken = GetTokenFromString(fieldShortName);
            //Flags
            FieldFlag flag = 0;
            if (context.ExistToken(Assemblier.MODIFIER_THREAD_LOCAL))
                flag |= FieldFlag.ThreadLocal;
            if (context.ExistToken(Assemblier.MODIFIER_READONLY))
                flag |= FieldFlag.ReadOnly;
            if (context.ExistToken(Assemblier.MODIFIER_VOLATILE))
                flag |= FieldFlag.Volatile;
            if (context.ExistToken(Assemblier.MODIFIER_CONSTANT))
                flag |= FieldFlag.Constant;

            var accessToken = context.modifierAccess()
                .GetUnderlyingTokenType();
            field.Accessibility = MapAccessbility(accessToken);

            var lifeToken = context.modifierLife()
                .GetUnderlyingTokenType();
            if (lifeToken == Assemblier.MODIFIER_STATIC)
                flag |= FieldFlag.Static;

            field.TypeRefToken = ResolveType(context.type());
            field.ParentTypeRefToken = mResolver.QueryTypeReference(
                null,
                typeFullQualifiedName,
                defToken => GetReferenceTokenOfType(null, typeFullQualifiedName, defToken));
            return fieldDefToken;
        }
        public uint ResolveMethodDef(
            Assemblier.MethodDefContext context,
            string typeFullQualifiedName)
        {  
            var methodFullQualifiedName = GetFullQualifiedName(context, typeFullQualifiedName);
            var methodShortName = context.methodName().GetText();

            var methodDefToken = TryDefineMethod(methodFullQualifiedName);
            var method = MethodDefTable[methodDefToken] as MethodMD;
            method.NameToken = GetTokenFromString(methodFullQualifiedName);

            //Parent ref
            method.ParentTypeRefToken = mResolver.QueryTypeReference(
                null,
                typeFullQualifiedName,
                defToken => GetReferenceTokenOfType(null, typeFullQualifiedName, defToken));

            MethodFlag flag = 0;
            if (context.ExistToken(Assemblier.MODIFIER_VIRTUAL))
                flag |= MethodFlag.Virtual;
            if (context.modifierLife().GetUnderlyingTokenType() == Assemblier.MODIFIER_STATIC)
                flag |= MethodFlag.Static;
            if (context.methodProperty() != null)
                flag |= MethodFlag.RTSpecial;
            method.Flags = flag;

            if (context.methodSource().methodImport() != null)
            {
                //TODO: import from external native dynamic library
            }

            var access = context.modifierAccess().GetUnderlyingTokenType();
            method.Accessibility = MapAccessbility(access);

            method.ILCodeMD = new ILAssemblier(context.methodBody(), this).Generate();
            
            return methodDefToken;
        }
        public uint ResolvePropertyDef(
            Assemblier.PropertyDefContext context,
            string typeFullQualifiedName)
        {
            var getter = context.propertyGet();
            uint getMethodRef = uint.MaxValue;
            if (getter != null)
                getMethodRef = ResolveMethodRef(getter.methodRef());

            var setter = context.propertySet();
            uint setMethodRef = uint.MaxValue;
            if (setter != null)
                setMethodRef = ResolveMethodRef(setter.methodRef());

            string shortName = context.IDENTIFIER().GetText();
            string fullQualifiedName = $"{typeFullQualifiedName}::{shortName}";

            if (getMethodRef == uint.MaxValue && setMethodRef == uint.MaxValue)
            {
                //Not exist
                throw new TypeResolveException($"Property [{fullQualifiedName}] contains no valid setter or getter");
            }

            return PropertyDefTable.GetDefinitionToken(fullQualifiedName, () => new PropertyMD
            {
                GetterToken = getMethodRef,
                SetterToken = setMethodRef,
                NameToken = GetTokenFromString(shortName),
                ParentTypeRefToken = mResolver.QueryTypeReference(
                    null, 
                    typeFullQualifiedName,
                    defToken => GetReferenceTokenOfType(null, typeFullQualifiedName, defToken)),
                TypeRefToken = ResolveType(context.type())
            });
        }
        public uint ResolveEventDef(
            Assemblier.EventDefContext context,
            string typeFullQualifiedName)
        {
            var adder = context.eventAdd();
            uint addMethodRef = uint.MaxValue;
            if (adder != null)
                addMethodRef = ResolveMethodRef(adder.methodRef());

            var remover = context.eventRemove();
            uint removeMethodRef = uint.MaxValue;
            if (remover != null)
                removeMethodRef = ResolveMethodRef(remover.methodRef());

            string shortName = context.IDENTIFIER().GetText();
            string fullQualifiedName = $"{typeFullQualifiedName}::{shortName}";

            if (addMethodRef == uint.MaxValue && removeMethodRef == uint.MaxValue)
            {
                //Not exist
                throw new TypeResolveException($"Event [{fullQualifiedName}] contains no valid setter or getter");
            }

            return PropertyDefTable.GetDefinitionToken(fullQualifiedName, () => new EventMD
            {
                AdderToken = addMethodRef,
                RemoverToken = removeMethodRef,
                NameToken = GetTokenFromString(shortName),
                ParentTypeRefToken = mResolver.QueryTypeReference(
                    null,
                    typeFullQualifiedName,
                    defToken => GetReferenceTokenOfType(null, typeFullQualifiedName, defToken)),
                TypeRefToken = ResolveType(context.type())
            });
        }
        public uint ResolveClassDef(Assemblier.ClassDefContext context, bool isNest = false)
        {
            //Handle the type mapping    
            var typeFullQualifiedName = GetFullQualifiedName(context, ".", context.typeName().GetText());
            var typeDefToken = TypeDefTable.GetDefinitionToken(typeFullQualifiedName, () => new TypeMD() { 
                NameToken = GetTokenFromString(typeFullQualifiedName)
            });
            var type = TypeDefTable[typeDefToken] as TypeMD;

            //Handle flags
            TypeFlag flag = 0;
            if (context.ExistToken(Assemblier.MODIFIER_NEST) && isNest)
                flag |= TypeFlag.Nested;
            else
                throw new BadModifierException("Nest modifier not consistent with declaration level");

            if (context.ExistToken(Assemblier.MODIFIER_SEALED))
                flag |= TypeFlag.Sealed;
            if (context.ExistToken(Assemblier.MODIFIER_ABSTRACT))
                flag |= TypeFlag.Abstract;
            if (context.ExistToken(Assemblier.MODIFIER_ATTRIBUTE))
                flag |= TypeFlag.Attribute;

            if (context.ExistToken(Assemblier.KEY_STRUCT))
                flag |= TypeFlag.Struct;
            else if (context.ExistToken(Assemblier.KEY_INTERFACE))
                flag |= TypeFlag.Interface;

            //Accessbility
            var accessToken = context.modifierAccess()
                .GetUnderlyingTokenType();

            type.Accessibility = MapAccessbility(accessToken);

            //Generic
            var genericList = context.genericList();
            if (genericList != null)
            {
                var genericParams = genericList.IDENTIFIER();
                type.GenericParameterTokens = genericParams
                    .Select(x =>
                    {
                        var fullQualifiedName = $"{typeFullQualifiedName}::{x.GetText()}";
                        return GenericParameterDefTable.GetDefinitionToken(fullQualifiedName,
                            () => new GenericParamterMD { NameToken = GetTokenFromString(fullQualifiedName) });
                    })
                    .ToArray();
            }

            //Inherit
            var inheritContext = context.typeInherit();
            if (inheritContext != null)
            {
                var parentType = inheritContext.inheritOrImplementType();
                type.ParentTypeRefToken = ResolveUnknownTypeForm(parentType.GetUnderlyingType());
            }

            //Interfaces
            var implementsContext = context.implementList();
            if (implementsContext != null)
            {
                type.InterfaceTokens = implementsContext
                    .typeRefList()
                    .inheritOrImplementType()
                    .Select(x => ResolveUnknownTypeForm(x.GetUnderlyingType()))
                    .ToArray();
            }

            //Handle the body
            var body = context.classBody();
            foreach (var member in body.classDef())
                ResolveClassDef(member, true);

            type.FieldTokens = body.fieldDef().Select(
                x => ResolveFieldDef(x, typeFullQualifiedName)).ToArray();
            type.MethodTokens = body.methodDef().Select(
                x => ResolveMethodDef(x, typeFullQualifiedName)).ToArray();
            type.PropertyTokens = body.propertyDef().Select(
                x => ResolvePropertyDef(x, typeFullQualifiedName)).ToArray();
            type.EventTokens = body.eventDef().Select(
                x => ResolveEventDef(x, typeFullQualifiedName)).ToArray();

            return typeDefToken;
        }
        public uint ResolveTypeRef(Assemblier.TypeRefContext context)
        {
            string assemblyName = context.assemblyRef().IDENTIFIER().GetText();
            string typeName = context.typeName().GetText();
            return mResolver.QueryTypeReference(assemblyName, typeName,
                defToken => GetReferenceTokenOfType(assemblyName, typeName, defToken));
        }
        public uint ResolveMethodRef(Assemblier.MethodRefContext context)
        {
            var methodCanonicalName = GetCanonicalFullQualifiedName(context,
                out string sourceAssemblyName);

            var parentType = context.methodParentType().GetUnderlyingType() as Assemblier.TypeContext;
            var typeRefToken = ResolveType(parentType);

            var methodDefToken = mResolver.QueryMethodDefinition(sourceAssemblyName, methodCanonicalName);

            var genericList = context.genericParameterList();
            if (genericList != null)
            {
                var typeTokens = genericList.type().Select(x => ResolveType(x)).ToArray();
            }      
            else
            {

            }
        }
        public uint ResolveFieldRef(Assemblier.FieldRefContext context)
        {
            return 0u;
        }
    }
}
