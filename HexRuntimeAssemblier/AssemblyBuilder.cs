using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

namespace HexRuntimeAssemblier
{
    public class AssemblyBuilder
    {
        private AssemblyHeaderMD mCurrentAssembly;
        private ReferenceResolver mReferenceResolver;
        private Dictionary<MDRecordKinds, DefinitionTable> mDefinitionTables;
        private Dictionary<string, uint> mString2Token = new();
        private uint mStringToken = 0u;

        private DefinitionTable TypeDefTable => mDefinitionTables[MDRecordKinds.TypeDef];
        private DefinitionTable FieldDefTable => mDefinitionTables[MDRecordKinds.FieldDef];
        private DefinitionTable MethodDefTable => mDefinitionTables[MDRecordKinds.MethodDef];
        private DefinitionTable PropertyDefTable => mDefinitionTables[MDRecordKinds.PropertyDef];
        private DefinitionTable EventDefTable => mDefinitionTables[MDRecordKinds.EventDef];
        private DefinitionTable GenericParameterDefTable => mDefinitionTables[MDRecordKinds.GenericParameter];
        private DefinitionTable GenericInstantiationDefTable => mDefinitionTables[MDRecordKinds.GenericInstantiationDef];

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

        public uint GetTokenFromString(string value)
        {
            if (!mString2Token.TryGetValue(value, out var token))
            {
                token = mStringToken++;
                mString2Token.Add(value, token);
            }
            return token;
        }
        /// <summary>
        /// Get the full qualified name of nested class or class member
        /// </summary>
        /// <returns></returns>
        public static string GetFullQualifiedName(ParserRuleContext memberContext, string referenceJunction, string shortName)
        {
            var stringBuilder = new StringBuilder();
            var current = memberContext.Parent;
            var currentClass = current as Assemblier.ClassDefContext;
           
            stringBuilder.Append(currentClass.typeName().GetText());
            stringBuilder.Append(referenceJunction);
            stringBuilder.Append(shortName);

            while (current is Assemblier.ClassDefContext)
            {
                currentClass = current as Assemblier.ClassDefContext;
                stringBuilder.Insert(0, '.');
                stringBuilder.Insert(0, currentClass.typeName().GetText());
                current = current.Parent;
            }
            
            return stringBuilder.ToString();
        }
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
                GenericParameterTokens = new List<uint> { elementRefToken }
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
                GenericParameterTokens = new List<uint> { elementRefToken }
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
                GenericParameterTokens = new List<uint> { internalRefToken }
            });
        }
        public uint ResolveGenericParameterRef(Assemblier.GenericParameterRefContext context)
        {
            return GenericParameterDefTable.GetDefinitionToken(context.GetText(), () => new GenericParamterMD
            {
                //interior generic ref, need to get from the core library
                NameToken = GetTokenFromString(context.GetText())
            });
        }
        public uint ResolveGenericInstantiation(Assemblier.GenericInstantiationContext context)
        {
            var canonical = context.typeRef();
            var canonicalRefToken = ResolveTypeRef(canonical);
            var parameterTypeTokens = context.type().Select(x => ResolveType(x)).ToList();

            return GenericInstantiationDefTable.GetDefinitionToken(context.GetText(), () => new GenericInstantiationMD
            {
                CanonicalTypeRefToken = canonicalRefToken,
                GenericParameterTokens = parameterTypeTokens
            });
        }
        public uint ResolveFieldDef(
            Assemblier.FieldDefContext context,
            string typeFullQualifiedName,
            uint typeDefToken)
        {
            //Handle the type mapping    
            var fieldShortName = context.IDENTIFIER().GetText();
            var fieldFullQualifiedName = GetFullQualifiedName(context, "::", fieldShortName);

            var fieldDefToken = FieldDefTable.GetDefinitionToken(fieldFullQualifiedName, () => new FieldMD());
            var field = FieldDefTable[fieldDefToken] as FieldMD;

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
            field.ParentTypeRefToken = mReferenceResolver.AcquireInternalTypeReference(typeFullQualifiedName, typeDefToken);
            return fieldDefToken;
        }
        public uint ResolveMethodDef(Assemblier.MethodDefContext context)
        {
            //Handle the type mapping    
            var methodShortName = context.methodName().GetText();
            var arguments = context.methodArgumentList().methodArgument();
            var methodSignature = $"{methodShortName}({string.Join(',', arguments.Select(x => x.type().GetText()))})";
            var methodFullQualifiedName = GetFullQualifiedName(context, "::", methodSignature);

            var fieldDefToken = TypeDefTable.GetDefinitionToken(methodFullQualifiedName, () => new FieldMD());
            var field = FieldDefTable[fieldDefToken] as FieldMD;
            return 0u;
        }
        public uint ResolvePropertyDef(
            Assemblier.PropertyDefContext context, 
            string typeFullQualifiedName,
            uint typeDefToken)
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
                ParentTypeRefToken = mReferenceResolver.AcquireInternalTypeReference(typeFullQualifiedName, typeDefToken),
                TypeRefToken = ResolveType(context.type())
            });
        }
        public uint ResolveEventDef(
            Assemblier.EventDefContext context,
            string typeFullQualifiedName,
            uint typeDefToken)
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
                throw new TypeResolveException($"Property [{fullQualifiedName}] contains no valid setter or getter");
            }

            return PropertyDefTable.GetDefinitionToken(fullQualifiedName, () => new EventMD
            {
                AdderToken = addMethodRef,
                RemoverToken = removeMethodRef,
                NameToken = GetTokenFromString(shortName),
                ParentTypeRefToken = mReferenceResolver.AcquireInternalTypeReference(typeFullQualifiedName, typeDefToken),
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
                    .ToList();
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
                    .ToList();
            }

            //Handle the body
            var body = context.classBody();
            foreach (var member in body.classDef())
                ResolveClassDef(member, true);

            type.FieldTokens = body.fieldDef().Select(x => ResolveFieldDef(x)).ToList();
            type.MethodTokens = body.methodDef().Select(x => ResolveMethodDef(x)).ToList();
            type.PropertyTokens = body.propertyDef().Select(
                x => ResolvePropertyDef(x, typeFullQualifiedName, typeDefToken)).ToList();
            type.EventTokens = body.eventDef().Select(
                x => ResolveEventDef(x, typeFullQualifiedName, typeDefToken)).ToList();

            return typeDefToken;
        }
        public uint ResolveTypeRef(Assemblier.TypeRefContext context)
        {
            var assemblyRef = context.assemblyRef();
            if (assemblyRef == null)
            {
                string typeName = context.GetText();
                var defToken = TypeDefTable.GetDefinitionToken(
                    typeName,
                    () => new TypeMD());

                return mReferenceResolver.AcquireInternalTypeReference(typeName, defToken);
            }
            else
            {
                return mReferenceResolver.ResolveTypeReference(assemblyRef.GetText(), context.GetText());
            }
        }
        public uint ResolveMethodRef(Assemblier.MethodRefContext context)
        {
            return 0u;
        }
        public uint ResolveFieldRef(Assemblier.FieldRefContext context)
        {
            return 0u;
        }
    }
}
