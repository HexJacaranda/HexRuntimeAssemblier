using Antlr4.Runtime.Tree;
using Antlr4.Runtime;
using HexRuntimeAssemblier.IL;
using HexRuntimeAssemblier.Interfaces;
using HexRuntimeAssemblier.Meta;
using HexRuntimeAssemblier.Reference;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;

namespace HexRuntimeAssemblier
{
    public partial class AssemblyBuilder : IAssemblyBuilder
    {
        private AssemblyHeaderMD mCurrentAssembly;
        private readonly GlobalResolver mResolver;
        private readonly Dictionary<MDRecordKinds, DefinitionTable> mDefTables = new();
        private readonly Dictionary<MDRecordKinds, ReferenceTable> mRefTables = new();
        private readonly StringTable mStringTable = new();
        private readonly ILogger<AssemblyBuilder> mLog;

        private readonly AssemblyOptions mOptions;
        private readonly CoreAssemblyConstant mConstant;

        public AssemblyBuilder(
            CoreAssemblyConstant constant,
            AssemblyOptions options,
            ILogger<AssemblyBuilder> logger,
            IEnumerable<IAssemblyResolver> externalResolvers)
        {
            mLog = logger;
            mConstant = constant;
            mOptions = options;

            mResolver = new GlobalResolver(this, externalResolvers.ToDictionary(x => x.AssemblyName, x => x));
            
            //Set def tables
            for (int kind = (int)MDRecordKinds.Argument; kind < (int)MDRecordKinds.KindLimit; ++kind)
                mDefTables.Add((MDRecordKinds)kind, new DefinitionTable((MDRecordKinds)kind));

            //Set ref tables
            //The zeroth token is reserved for self reference
            var assemblyRefTable = new ReferenceTable(MDRecordKinds.KindLimit);
            assemblyRefTable.GetReferenceToken("Self", () => new AssemblyRefMD());

            //Initialize assembly reference table
            foreach (var assembly in externalResolvers)
                assemblyRefTable.GetReferenceToken(assembly.AssemblyName,
                    () => new AssemblyRefMD
                    {
                        AssemblyName = GetTokenFromString(assembly.AssemblyName),
                        GUID = assembly.AssemblyGuid
                    });

            mRefTables.Add(MDRecordKinds.KindLimit, assemblyRefTable);

            //Other reference table
            mRefTables.Add(MDRecordKinds.FieldRef, new ReferenceTable(MDRecordKinds.FieldRef));
            mRefTables.Add(MDRecordKinds.TypeRef, new ReferenceTable(MDRecordKinds.TypeRef));
        }
        #region TableAlias
        private DefinitionTable TypeDefTable => mDefTables[MDRecordKinds.TypeDef];
        private DefinitionTable FieldDefTable => mDefTables[MDRecordKinds.FieldDef];
        private DefinitionTable MethodDefTable => mDefTables[MDRecordKinds.MethodDef];
        private DefinitionTable PropertyDefTable => mDefTables[MDRecordKinds.PropertyDef];
        private DefinitionTable EventDefTable => mDefTables[MDRecordKinds.EventDef];
        private DefinitionTable GenericParameterDefTable => mDefTables[MDRecordKinds.GenericParameter];
        private DefinitionTable GenericInstantiationDefTable => mDefTables[MDRecordKinds.GenericInstantiationDef];
        public ReferenceTable AssemblyReferenceTable => mRefTables[MDRecordKinds.KindLimit];
        public ReferenceTable MemberReferenceTable => mRefTables[MDRecordKinds.FieldRef];
        public ReferenceTable TypeReferenceTable => mRefTables[MDRecordKinds.TypeRef];
        #endregion
        #region Helper
        private static string GetPropertyKey(Assemblier.PropertyContext context)
            => context.GetChild<Assemblier.PropertyKeyContext>(0).GetText();
        private static string GetPropertyValue(Assemblier.PropertyContext context)
            => context.GetChild<Assemblier.PropertyValueContext>(0).GetText();
        private static byte MapAccessbility(int tokenType) => tokenType switch
        {
            Assemblier.MODIFIER_PUBLIC => 0,
            Assemblier.MODIFIER_PRIVATE => 1,
            Assemblier.MODIFIER_PROTECTED => 2,
            Assemblier.MODIFIER_INTERNAL => 3,
            _ => throw new Exception()
        };
        public uint GetTokenFromString(string value)
            => mStringTable.GetTokenFromString(value);
        private uint GetArrayCanonicalDefToken()
        {
            string assembly = null;
            string fullyQualifiedName = $"{mConstant.Array}<{CanonicalPlaceHolder}>";
            if (mOptions.CoreLibrary)
                assembly = mConstant.AssemblyStandardName;

            return mResolver.QueryTypeDefinition(assembly, fullyQualifiedName);
        }
        private uint GetInteriorReferenceCanonicalDefToken()
        {
            string assembly = null;
            string fullyQualifiedName = $"{mConstant.InteriorReference}<{CanonicalPlaceHolder}>";
            if (mOptions.CoreLibrary)
                assembly = mConstant.AssemblyStandardName;

            return mResolver.QueryTypeDefinition(assembly, fullyQualifiedName);
        }
        private string CanonicalPlaceHolder => mConstant.CanonicalPlaceholder;
        #endregion
        #region FQN
        /* Full qualified name(FQN) is used in reference to type, method, field. There are 
         * also two different kinds of such name according to generic.
         * 
         * 1. Type: 
         * [Core][System]Array<[System]Int32>, this is called a normal FQN
         * [Core][System]Array<Canon>, this is called a canonical FQN
         * 
         * 2. Method: 
         * int32 [Core][System]Converter<double>::To<int32>(double), this is a normal FQN
         * 
         * NOTE: method requires return type and argument types to be at canonical form as well.
         * Canon [Core][System]Converter<Canon>::To<Canon>(Canon, Canon, Safe<Canon>), this is a canonical FQN
         * 
         * 3. Field:
         * [Core][System]Converter<double>::Some, this is a normal FQN
         * [Core][System]Converter<Canon>::To, this is a canonical FQN
         * 
         * Assembly reference is a must, while the empty namespace means [global] namespace
         */

        private string GetNamespaceTag(Assemblier.ClassNameSpaceContext context)
        {
            if (context == null)
                return $"[{mConstant.GlobalNamespace}]";
            else
                return $"[{context.namespaceValue().GetText()}]";
        }
        private string GetNamespaceTag(Assemblier.TypeRefNamespaceContext context)
        {
            if (context == null)
                return $"[{mConstant.GlobalNamespace}]";
            else
                return $"[{context.namespaceValue().GetText()}]";
        }
        private string GetFullyQualifiedName(Assemblier.ClassDefContext context, string parentFullyQualifiedName)
        {
            var stringBuilder = new StringBuilder();

            if (string.IsNullOrEmpty(parentFullyQualifiedName))
            {
                stringBuilder.Append($"[{AssemblyName}]");
                stringBuilder.Append(GetNamespaceTag(context.classNameSpace()));
            }
            else
            {
                stringBuilder.Append(parentFullyQualifiedName);
                stringBuilder.Append('.');
            }
            stringBuilder.Append(context.className().GetText());
            if (context.genericList() != null)
                stringBuilder.Append($"<{string.Join(", ", Enumerable.Repeat(CanonicalPlaceHolder, context.genericList().IDENTIFIER().Length))}>");
            return stringBuilder.ToString();
        }
        private string GetFullyQualifiedName(Assemblier.MethodDefContext context, string typeFullyQualifiedName)
        {
            var methodShortName = context.methodName().GetText();
            var arguments = context.methodArgumentList().methodArgument();
            var genericArguments = context.genericList()?.IDENTIFIER();

            string methodSignature = null;
            if (genericArguments != null && genericArguments.Length > 0)
            {
                methodSignature = $"{methodShortName}<{string.Join(", ", Enumerable.Repeat(CanonicalPlaceHolder, genericArguments.Length))}>" +
                $"({string.Join(", ", arguments.Select(x => GetFullyQualifiedName(x.type())))})";
            }
            else
            {
                methodSignature = $"{methodShortName}({string.Join(", ", arguments.Select(x => GetFullyQualifiedName(x.type())))})";
            }

            return $"{typeFullyQualifiedName}::{methodSignature}";
        }
        private string GetFullyQualifiedName(Assemblier.TypeContext context)
            => context.GetChild(0) switch
            {
                Assemblier.ArrayTypeContext array => GetFullyQualifiedName(array),
                Assemblier.GenericParameterRefContext genericParam => GetFullyQualifiedName(genericParam),
                Assemblier.TypeRefContext typeRef => GetFullyQualifiedName(typeRef),
                Assemblier.InteriorRefTypeContext interiorRef => GetFullyQualifiedName(interiorRef),
                Assemblier.PrimitiveTypeContext primitive => GetFullyQualifiedName(primitive),
                _ => throw new UnexpectedParseRuleException("Unexpected child of ArrayType rule"),
            };
        private string GetFullyQualifiedName(Assemblier.PrimitiveTypeContext context)
        {
            return mConstant.PrimitiveTypes[context.GetText()];
        }
        private string GetFullyQualifiedName(Assemblier.TypeRefNodeContext context)
            => context.GetChild(0) switch
            {
                Assemblier.TypeRefGenericContext generic => $"{generic.IDENTIFIER().GetText()}" +
                    $"<{string.Join(", ", generic.type().Select(x => GetFullyQualifiedName(x)))}>",
                _ => context.GetText(),
            };
        private string GetFullyQualifiedName(Assemblier.TypeRefContext context)
        {
            StringBuilder builder = new();

            //Assembly tag
            builder.Append($"[{context.assemblyRef().IDENTIFIER()}]");

            //Top namespace, mandatory
            var typeName = context.typeName();
            builder.Append(GetNamespaceTag(typeName.typeRefNamespace()));

            var nodes = typeName.typeRefNode();
            for(int i = 0; i< nodes.Length - 1; i++)
            {
                builder.Append(GetFullyQualifiedName(nodes[i]));
                builder.Append('.');
            }

            builder.Append(GetFullyQualifiedName(nodes.Last()));
            return builder.ToString();
        }
        private string GetFullyQualifiedName(Assemblier.InteriorRefTypeContext context)
        {
            string internalQualifiedName = context.GetChild(0) switch
            {
                Assemblier.PrimitiveTypeContext primitive => GetFullyQualifiedName(primitive),
                Assemblier.TypeRefContext typeRef => GetFullyQualifiedName(typeRef),
                Assemblier.ArrayTypeContext array => GetFullyQualifiedName(array),
                _ => throw new UnexpectedParseRuleException("Unexpected interior reference child")
            };
            return $"{internalQualifiedName}&";
        }
        private string GetFullyQualifiedName(Assemblier.ArrayTypeContext context)
            => context.GetChild(0) switch
            {
                Assemblier.NestArrayTypeContext nest => GetFullyQualifiedName(nest),
                Assemblier.MultidimensionArrayTypeContext multiDimension => GetFullyQualifiedName(multiDimension),
                _ => throw new UnexpectedParseRuleException("Unexpected child of ArrayType rule")
            };
        private string GetFullyQualifiedName(Assemblier.NestArrayTypeContext context)
            => $"{mConstant.Array}<{GetFullyQualifiedName(context.type())}>";
        private string GetFullyQualifiedName(Assemblier.MultidimensionArrayTypeContext context)
            => $"{mConstant.Array}<{GetFullyQualifiedName(context.type())}, {context.INT().GetText()}>";
        private string GetFullyQualifiedName(Assemblier.MethodRefContext context)
        {
            var typeRef = GetFullyQualifiedName(context.methodParentType().GetUnderlyingType() as Assemblier.TypeContext);
            var methodName = context.methodName().GetText();

            string generic = null;
            if (context.genericParameterList() != null && context.genericParameterList().type().Length > 0)
                generic = $"<{string.Join(", ", context.genericParameterList().type().Select(x => GetFullyQualifiedName(x)))}>";

            var parameters = $"({string.Join(", ", context.type().Select(x => GetFullyQualifiedName(x)))})";
            if (generic != null)
                return $"{typeRef}::{methodName}{generic}{parameters}";
            else
                return $"{typeRef}::{methodName}{parameters}";
        }
        private string GetFullyQualifiedName(Assemblier.FieldRefContext context)
        {
            var typeRef = GetFullyQualifiedName(context.type());
            return $"{typeRef}::{context.IDENTIFIER().GetText()}";
        }
        private string GetFullyQualifiedName(Assemblier.GenericParameterRefContext context)
            => context.GetText();
        #endregion
        #region Canonical FQN
        private string GetCanonicalFullyQualifiedName(Assemblier.TypeContext context)
            => context.GetChild(0) switch
            {
                Assemblier.ArrayTypeContext array => GetCanonicalFullyQualifiedName(array),
                Assemblier.PrimitiveTypeContext primitive => GetFullyQualifiedName(primitive),
                Assemblier.TypeRefContext typeRef => GetCanonicalFullyQualifiedName(typeRef),
                Assemblier.InteriorRefTypeContext interior => GetCanonicalFullyQualifiedName(interior),
                Assemblier.GenericParameterRefContext genericParam => GetCanonicalFullyQualifiedName(genericParam),
                _ => throw new UnexpectedParseRuleException("Unexpected child of ArrayType rule")
            };
        private string GetCanonicalFullyQualifiedName(Assemblier.ArrayTypeContext context)
            => context.GetChild(0) switch
            {
                Assemblier.NestArrayTypeContext nest => GetCanonicalFullyQualifiedName(nest),
                Assemblier.MultidimensionArrayTypeContext multi => GetCanonicalFullyQualifiedName(multi),
                _ => throw new UnexpectedParseRuleException("Unexpected child of ArrayType rule")
            };
        private string GetCanonicalFullyQualifiedName(Assemblier.NestArrayTypeContext _)
            => $"{mConstant.Array}<{CanonicalPlaceHolder}>";
        private string GetCanonicalFullyQualifiedName(Assemblier.MultidimensionArrayTypeContext context)
            => $"{mConstant.Array}<{CanonicalPlaceHolder}, {context.INT().GetText()}>";
        private string GetCanonicalFullyQualifiedName(Assemblier.InteriorRefTypeContext context)
        {
            string internalQualifiedName = context.GetChild(0) switch
            {
                Assemblier.PrimitiveTypeContext primitive => GetFullyQualifiedName(primitive),
                Assemblier.TypeRefContext typeRef => GetCanonicalFullyQualifiedName(typeRef),
                Assemblier.ArrayTypeContext array => GetCanonicalFullyQualifiedName(array),
                _ => throw new UnexpectedParseRuleException("Unexpected interior reference child")
            };
            return $"{internalQualifiedName}&";
        }
        private string GetCanonicalFullyQualifiedName(Assemblier.GenericParameterRefContext _)
            => CanonicalPlaceHolder;
        private string GetCanonicalFullyQualifiedName(Assemblier.TypeRefContext context)
        {
            StringBuilder builder = new();
            //Insert the namespace part
            builder.Append(context.assemblyRef().GetText());
            builder.Append(GetNamespaceTag(context.typeName().typeRefNamespace()));
            var typeRefNodes = context.typeName().typeRefNode();
            void AppendNode(IParseTree x)
            {
                switch (x)
                {
                    case Assemblier.TypeRefPlainContext plain:
                        builder.Append(plain.GetText());
                        break;
                    case Assemblier.TypeRefGenericContext generic:
                        {
                            builder.Append(generic.IDENTIFIER().GetText());
                            var genericParams = generic.type();
                            builder.Append($"<{string.Join(", ", Enumerable.Repeat(CanonicalPlaceHolder, genericParams.Length))}>");
                        }
                        break;
                }
            }

            {
                for (int i = 0; i < typeRefNodes.Length - 1; i++)
                {
                    AppendNode(typeRefNodes[i].GetChild(0));
                    builder.Append('.');
                }
                AppendNode(typeRefNodes[^1].GetChild(0));
            }

            return builder.ToString();
        }
        private string GetCanonicalFullyQualifiedName(
            Assemblier.MethodRefContext context, 
            out string sourceAssemblyName)
        {
            var parentType = context.methodParentType().type();
            var parentTypeRefToken = ResolveType(parentType);
            var typeRefMD = TypeReferenceTable.GetMeta<TypeRefMD>(parentTypeRefToken);

            string parentTypeCanonicalQFN = GetCanonicalFullyQualifiedName(parentType);
            sourceAssemblyName = null;
            if (typeRefMD.AssemblyToken != AssemblyRefMD.Self)
            {
                var assembly = AssemblyReferenceTable.GetMeta<AssemblyRefMD>(typeRefMD.AssemblyToken);
                sourceAssemblyName = mStringTable.Contents[(int)assembly.AssemblyName];
            }

            StringBuilder builder = new();
            builder.Append($"{parentTypeCanonicalQFN}::{context.methodName().GetText()}");

            //Generic
            if (context.genericParameterList() != null)
            {
                int genericArgumentCount = context.genericParameterList().type().Length;
                builder.Append($"<{string.Join(", ", Enumerable.Repeat(CanonicalPlaceHolder, genericArgumentCount))}>");
            }

            builder.Append($"({string.Join(", ", context.type().Select(x => GetFullyQualifiedName(x)))})");
            return builder.ToString();
        }
        private string GetCanonicalFullyQualifiedName(
            Assemblier.FieldRefContext context,
            out string sourceAssemblyName)
        {
            var parentType = context.type();
            var parentTypeRefToken = ResolveType(parentType);

            var typeRefMD = TypeReferenceTable.GetMeta<TypeRefMD>(parentTypeRefToken);
            string strippedAssemblyTagTypeName = GetCanonicalFullyQualifiedName(parentType);

            sourceAssemblyName = null;
            if (typeRefMD.AssemblyToken != AssemblyRefMD.Self)
            {
                var assembly = AssemblyReferenceTable.GetMeta<AssemblyRefMD>(typeRefMD.AssemblyToken);
                sourceAssemblyName = mStringTable.Contents[(int)assembly.AssemblyName];
            }

            var fieldName = context.IDENTIFIER().GetText();
            return $"{strippedAssemblyTagTypeName}::{fieldName}";
        }
        #endregion
        public void ResolveStart(Assemblier.StartContext context)
        {
            ResolveAssemblyDef(context.assemblyDef());
            foreach (var classContext in context.classDef())
                ResolveClassDef(classContext, null);
        }
        public void ResolveAssemblyDef(Assemblier.AssemblyDefContext context)
        {
            var map = context.property().ToDictionary(x => GetPropertyKey(x), x => GetPropertyValue(x));

            mCurrentAssembly = new AssemblyHeaderMD
            {
                NameToken = GetTokenFromString(map["name"].Trim('"')),
                GroupNameToken = GetTokenFromString(map["groupname"].Trim('"')),
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
                Assemblier.TypeRefContext typeRef => ResolveTypeRef(typeRef),
                Assemblier.PrimitiveTypeContext primitiveType => ResolvePrimitiveType(primitiveType),
                Assemblier.ArrayTypeContext arrayType => ResolveArrayType(arrayType),
                Assemblier.NestArrayTypeContext nestArrayType => ResolveArrayType(nestArrayType),
                Assemblier.MultidimensionArrayTypeContext multidimensionArrayType => ResolveArrayType(multidimensionArrayType),
                Assemblier.InteriorRefTypeContext interiorRefType => ResolveInteriorRefType(interiorRefType),
                Assemblier.GenericParameterRefContext genericParam => ResolveGenericParameterRef(genericParam),
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
            if (mOptions.DisableCoreType)
                return uint.MaxValue;

            //Primitive type always requires metadata from core library
            string alias = context.GetText();
            string fullyQualifiedName = mConstant.PrimitiveTypes[alias];

            string asemblyName = null;
            //Are we actually building the core library?
            if (!mOptions.CoreLibrary)
                asemblyName = mConstant.AssemblyStandardName;

            return mResolver.QueryTypeReference(asemblyName, fullyQualifiedName,
                defToken => GetReferenceTokenOfType(asemblyName, fullyQualifiedName, defToken));
        }
        public uint ResolveArrayType(Assemblier.ArrayTypeContext context)
            => context.GetChild(0) switch
            {
                Assemblier.NestArrayTypeContext nestArray => ResolveArrayType(nestArray),
                Assemblier.MultidimensionArrayTypeContext multi => ResolveArrayType(multi),
                _ => throw new TypeResolveException("Unknown underlying array representation")
            }; 
        public uint ResolveArrayType(Assemblier.MultidimensionArrayTypeContext context)
        {
            string assemblyName = null;
            if(mOptions.CoreLibrary)
                assemblyName = mConstant.AssemblyStandardName;

            var arrayCanonicalDefToken = uint.MaxValue;
            if (!mOptions.DisableCoreType)
                arrayCanonicalDefToken = GetArrayCanonicalDefToken();

            var elementType = context.type();
            var elementRefToken = ResolveType(elementType);
            var fullyQualifiedName = GetFullyQualifiedName(context);

            var genericInstDefToken = GenericInstantiationDefTable.GetDefinitionToken(
                fullyQualifiedName,
                () => new GenericInstantiationMD
                {
                    //Multidimension-Array generic ref, need to get from the core library
                    CanonicalTypeDefToken = arrayCanonicalDefToken,
                    GenericParameterTokens = new uint[] { elementRefToken }
                });

            return GetReferenceTokenOfType(
                assemblyName, 
                fullyQualifiedName, 
                genericInstDefToken, 
                MDRecordKinds.GenericInstantiationDef);
        }
        public uint ResolveArrayType(Assemblier.NestArrayTypeContext context)
        {
            string assemblyName = null;
            if (mOptions.CoreLibrary)
                assemblyName = mConstant.AssemblyStandardName;

            var arrayCanonicalDefToken = uint.MaxValue;
            if (!mOptions.DisableCoreType)
                arrayCanonicalDefToken = GetArrayCanonicalDefToken();

            var elementType = context.type();
            var elementRefToken = ResolveType(elementType);
            var fullyQualifiedName = GetFullyQualifiedName(context);

            var genericInstDefToken = GenericInstantiationDefTable.GetDefinitionToken(
                fullyQualifiedName,
                () => new GenericInstantiationMD
                {
                    //Array generic ref, need to get from the core library
                    CanonicalTypeDefToken = arrayCanonicalDefToken,
                    GenericParameterTokens = new uint[] { elementRefToken }
                });

            return GetReferenceTokenOfType(
                assemblyName,
                fullyQualifiedName,
                genericInstDefToken,MDRecordKinds.GenericInstantiationDef);
        }
        public uint ResolveInteriorRefType(Assemblier.InteriorRefTypeContext context)
        {
            string assemblyName = null;
            if (mOptions.CoreLibrary)
                assemblyName = mConstant.AssemblyStandardName;

            var interiorCanonicalDefToken = uint.MaxValue;
            if (!mOptions.DisableCoreType)
                interiorCanonicalDefToken = GetInteriorReferenceCanonicalDefToken();

            var internalType = context.GetChild(0);
            var internalRefToken = ResolveUnknownTypeForm(internalType);
            var fullyQualifiedName = GetFullyQualifiedName(context);

            var genericInstDefToken = GenericInstantiationDefTable.GetDefinitionToken(
                fullyQualifiedName,
                () => new GenericInstantiationMD
                {
                    //interior generic ref, need to get from the core library
                    CanonicalTypeDefToken = interiorCanonicalDefToken,
                    GenericParameterTokens = new uint[] { internalRefToken }
                });

            return GetReferenceTokenOfType(
               assemblyName,
               fullyQualifiedName,
               genericInstDefToken, MDRecordKinds.GenericInstantiationDef);
        }
        public uint ResolveGenericParameterRef(Assemblier.GenericParameterRefContext context)
        {
            var genericParam = context.IDENTIFIER().GetText();
            var genericParamDefToken = GenericParameterDefTable.GetDefinitionToken(
                genericParam,
                () => new GenericParamterMD
                {
                    NameToken = GetTokenFromString(genericParam)
                });

            return GetReferenceTokenOfType(
                null, 
                genericParam, 
                genericParamDefToken, 
                MDRecordKinds.GenericParameter);
        }
        public uint ResolveFieldDef(
            Assemblier.FieldDefContext context,
            string typeFullyQualifiedName)
        {
            //Handle the type mapping    
            var fieldShortName = context.IDENTIFIER().GetText();
            var fieldFullyQualifiedName = $"{typeFullyQualifiedName}::{fieldShortName}";

            var fieldDefToken = TryDefineField(fieldFullyQualifiedName);
            var field = FieldDefTable[fieldDefToken] as FieldMD;

            field.NameToken = GetTokenFromString(fieldShortName);
            field.FullyQualifiedNameToken = GetTokenFromString(fieldFullyQualifiedName);
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

            field.Flags = flag;

            field.TypeRefToken = ResolveType(context.type());
            field.ParentTypeRefToken = mResolver.QueryTypeReference(
                null,
                typeFullyQualifiedName,
                defToken => GetReferenceTokenOfType(null, typeFullyQualifiedName, defToken));
            return fieldDefToken;
        }
        public uint ResolveMethodDef(
            Assemblier.MethodDefContext context,
            string typeFullyQualifiedName)
        {  
            var methodFullyQualifiedName = GetFullyQualifiedName(context, typeFullyQualifiedName);
            var methodShortName = context.methodName().GetText();

            var methodDefToken = TryDefineMethod(methodFullyQualifiedName);
            var method = MethodDefTable[methodDefToken] as MethodMD;
            method.NameToken = GetTokenFromString(methodShortName);
            method.FullyQualifiedNameToken = GetTokenFromString(methodFullyQualifiedName);

            //Parent ref
            method.ParentTypeRefToken = mResolver.QueryTypeReference(
                null,
                typeFullyQualifiedName,
                defToken => GetReferenceTokenOfType(null, typeFullyQualifiedName, defToken));

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

            method.ILCodeMD = new ILAssemblier(context.methodBody(), context.methodArgumentList(), this).Generate();

            //Signature
            {
                uint returnTypeRef = Token.Null;
                if (context.methodReturnType().VOID() == null)
                    returnTypeRef = ResolveType(context.methodReturnType().GetUnderlyingType() as Assemblier.TypeContext);

                var arguments = context.methodArgumentList();
                uint[] argumentTokens = null;
                if (arguments != null)
                {
                    argumentTokens = arguments.methodArgument().Select(x =>
                        mDefTables[MDRecordKinds.Argument].GetDefinitionToken(
                            $"{methodFullyQualifiedName}~{x.IDENTIFIER().GetText()}",
                        () => new ArgumentMD()
                        {
                            //TODO: default value
                            DefaultStringValueToken = Token.Null,
                            NameToken = GetTokenFromString(x.IDENTIFIER().GetText()),
                            TypeRefToken = ResolveType(x.type())
                        })).ToArray();
                }

                method.Signature = new MethodSignatureMD()
                {
                    ReturnTypeRefToken = returnTypeRef,
                    ArgumentTokens = argumentTokens
                };
            }
            
            return methodDefToken;
        }
        public uint ResolvePropertyDef(
            Assemblier.PropertyDefContext context,
            string typeFullyQualifiedName)
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
            string fullyQualifiedName = $"{typeFullyQualifiedName}::{shortName}";

            if (getMethodRef == uint.MaxValue && setMethodRef == uint.MaxValue)
            {
                //Not exist
                throw new TypeResolveException($"Property [{fullyQualifiedName}] contains no valid setter or getter");
            }

            return PropertyDefTable.GetDefinitionToken(fullyQualifiedName, () => new PropertyMD
            {
                GetterToken = getMethodRef,
                SetterToken = setMethodRef,
                NameToken = GetTokenFromString(shortName),
                ParentTypeRefToken = mResolver.QueryTypeReference(
                    null, 
                    typeFullyQualifiedName,
                    defToken => GetReferenceTokenOfType(null, typeFullyQualifiedName, defToken)),
                TypeRefToken = ResolveType(context.type())
            });
        }
        public uint ResolveEventDef(
            Assemblier.EventDefContext context,
            string typeFullyQualifiedName)
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
            string fullyQualifiedName = $"{typeFullyQualifiedName}::{shortName}";

            if (addMethodRef == uint.MaxValue && removeMethodRef == uint.MaxValue)
            {
                //Not exist
                throw new TypeResolveException($"Event [{fullyQualifiedName}] contains no valid setter or getter");
            }

            return PropertyDefTable.GetDefinitionToken(fullyQualifiedName, () => new EventMD
            {
                AdderToken = addMethodRef,
                RemoverToken = removeMethodRef,
                NameToken = GetTokenFromString(shortName),
                ParentTypeRefToken = mResolver.QueryTypeReference(
                    null,
                    typeFullyQualifiedName,
                    defToken => GetReferenceTokenOfType(null, typeFullyQualifiedName, defToken)),
                TypeRefToken = ResolveType(context.type())
            });
        }
        public uint ResolveClassDef(Assemblier.ClassDefContext context, string outerClassFullyQualifiedName, bool isNest = false)
        {
            //Handle the type mapping    
            var typeFullyQualifiedName = GetFullyQualifiedName(context, outerClassFullyQualifiedName);
            var typeDefToken = TypeDefTable.GetDefinitionToken(typeFullyQualifiedName, () => new TypeMD());

            var type = TypeDefTable[typeDefToken] as TypeMD;
            type.NameToken = GetTokenFromString(context.className().GetText());
            type.FullyQualifiedNameToken = GetTokenFromString(typeFullyQualifiedName);

            //Set assumed core type
            type.CoreType = CoreTypes.Ref;

            //Handle flags
            TypeFlag flag = 0;

            if (context.ExistToken(Assemblier.MODIFIER_NEST) && isNest)
                flag |= TypeFlag.Nested;
            else if (context.ExistToken(Assemblier.MODIFIER_NEST) ^ isNest)
                throw new BadModifierException("Nest modifier not consistent with declaration level");

            if (context.ExistToken(Assemblier.MODIFIER_SEALED))
                flag |= TypeFlag.Sealed;
            if (context.ExistToken(Assemblier.MODIFIER_ABSTRACT))
                flag |= TypeFlag.Abstract;
            if (context.ExistToken(Assemblier.MODIFIER_ATTRIBUTE))
                flag |= TypeFlag.Attribute;

            if (context.ExistToken(Assemblier.KEY_STRUCT))
            {
                flag |= TypeFlag.Struct;
                //Set struct core type
                type.CoreType = CoreTypes.Struct;
            }
            else if (context.ExistToken(Assemblier.KEY_INTERFACE))
                flag |= TypeFlag.Interface;

            //Set flag
            type.Flags = flag;

            //Accessbility
            var accessToken = context.modifierAccess()
                .GetUnderlyingTokenType();

            type.Accessibility = MapAccessbility(accessToken);

            //Overrides generic Core Type
            if (mConstant.PrimitiveToCore.TryGetValue(typeFullyQualifiedName, out var core))
                type.CoreType = core;

            //Generic
            var genericList = context.genericList();
            if (genericList != null)
            {
                var genericParams = genericList.IDENTIFIER();
                type.GenericParameterTokens = genericParams
                    .Select(x =>
                    {
                        var fullyQualifiedName = x.GetText();
                        return GenericParameterDefTable.GetDefinitionToken(fullyQualifiedName,
                            () => new GenericParamterMD { NameToken = GetTokenFromString(fullyQualifiedName) });
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
                ResolveClassDef(member, typeFullyQualifiedName ,true);

            type.FieldTokens = body.fieldDef().Select(
                x => ResolveFieldDef(x, typeFullyQualifiedName)).ToArray();
            type.MethodTokens = body.methodDef().Select(
                x => ResolveMethodDef(x, typeFullyQualifiedName)).ToArray();
            type.PropertyTokens = body.propertyDef().Select(
                x => ResolvePropertyDef(x, typeFullyQualifiedName)).ToArray();
            type.EventTokens = body.eventDef().Select(
                x => ResolveEventDef(x, typeFullyQualifiedName)).ToArray();

            return typeDefToken;
        }
        /// <summary>
        /// Special care for type reference that may contain generic instantiation
        /// </summary>
        /// <param name="context"></param>
        /// <param name="genericParamTokens"></param>
        /// <param name="canonicalName"></param>
        /// <param name="assembly"></param>
        public void Canonicalize(
            Assemblier.TypeRefContext context, 
            out List<uint> genericParamTokens, 
            out string canonicalName,
            out string assembly)
        {
            assembly = context.assemblyRef().IDENTIFIER().GetText();
            StringBuilder builder = new();
            //Insert the namespace part
            builder.Append(context.assemblyRef().GetText());
            builder.Append(GetNamespaceTag(context.typeName().typeRefNamespace()));

            var typeRefNodes = context.typeName().typeRefNode();
            genericParamTokens = new();

            void AppendNode(IParseTree x, List<uint> genericParamTokensIn)
            {
                switch (x)
                {
                    case Assemblier.TypeRefPlainContext plain:
                        builder.Append(plain.GetText());
                        break;
                    case Assemblier.TypeRefGenericContext generic:
                        {
                            builder.Append(generic.IDENTIFIER().GetText());
                            var genericParams = generic.type();
                            builder.Append($"<{string.Join(", ", Enumerable.Repeat(CanonicalPlaceHolder, genericParams.Length))}>");

                            //Resolve
                            genericParamTokensIn.AddRange(genericParams.Select(x => ResolveType(x)));
                        }
                        break;
                }
            }

            {
                for (int i = 0; i < typeRefNodes.Length - 1; i++)
                {
                    AppendNode(typeRefNodes[i].GetChild(0), genericParamTokens);
                    builder.Append('.');
                }
                AppendNode(typeRefNodes[^1].GetChild(0), genericParamTokens);
            }

            canonicalName = builder.ToString();
        }
        /// <summary>
        /// Type reference should be TypeRef { Assembly, TypeDef }
        /// or TypeRef { Assembly, GenericInst } -> GenericInst { TypeDef, TypeRef... }
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public uint ResolveTypeRef(Assemblier.TypeRefContext context)
        {
            Canonicalize(context,
                out var genericParamTokens,
                out var canonicalFullyQualifiedName,
                out var assemblyName);         

            string fullyQualifiedName = GetFullyQualifiedName(context);
            var canonicalDef = mResolver.QueryTypeDefinition(assemblyName, canonicalFullyQualifiedName);
            if (genericParamTokens.Count == 0)
                return GetReferenceTokenOfType(assemblyName, fullyQualifiedName, canonicalDef);
            else
            {
                //For generic instantiation, we need a GenericInstantiation
                var genericInstantiationToken = GenericInstantiationDefTable.GetDefinitionToken(
                    fullyQualifiedName,
                    () => new GenericInstantiationMD
                    {
                        CanonicalTypeDefToken = canonicalDef,
                        GenericParameterTokens = genericParamTokens.ToArray()
                    });

                return GetReferenceTokenOfType(
                    assemblyName,
                    fullyQualifiedName,
                    genericInstantiationToken,
                    MDRecordKinds.GenericInstantiationDef);
            }
        }
        /// <summary>
        /// Method reference should be MemberRef { TypeRef, MethodDef }
        /// or MemberRef { TypeRef, GenericInst } -> GenericInst { MethodDef, TypeRef... }
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public uint ResolveMethodRef(Assemblier.MethodRefContext context)
        {
            var parentType = context.methodParentType().GetUnderlyingType() as Assemblier.TypeContext;
            var typeRefToken = ResolveType(parentType);

            var methodCanonicalName = GetCanonicalFullyQualifiedName(context, out string sourceAssemblyName);
            var methodFullyQualifiedName = GetFullyQualifiedName(context);
            var methodDefToken = mResolver.QueryMethodDefinition(sourceAssemblyName, methodCanonicalName);

            int memberDefKind = (int)MDRecordKinds.MethodRef;
            var genericList = context.genericParameterList();
            if (genericList != null)
            {
                var typeTokens = genericList.type().Select(x => ResolveType(x)).ToArray();
                var genericInstantiationToken = GenericInstantiationDefTable.GetDefinitionToken(methodFullyQualifiedName,
                    () => new GenericInstantiationMD()
                    {
                        CanonicalTypeDefToken = methodDefToken,
                        GenericParameterTokens = typeTokens
                    });
                methodDefToken = genericInstantiationToken;
                //Set highest bit
                memberDefKind |= unchecked((int)0x80000000);
            }

            return MemberReferenceTable.GetReferenceToken(methodFullyQualifiedName, () => new MemberRefMD()
            {
                MemberDefKind = memberDefKind,
                MemberDefToken = methodDefToken,
                TypeRefToken = typeRefToken
            });
        }
        public uint ResolveFieldRef(Assemblier.FieldRefContext context)
        {
            var fieldCanonicalName = GetCanonicalFullyQualifiedName(context,
                out string sourceAssemblyName);
            var fieldFullyQualifiedName = GetFullyQualifiedName(context);

            var fieldDefToken = mResolver.QueryFieldDefinition(sourceAssemblyName, fieldCanonicalName);
            var parentRefToken = ResolveType(context.type());

            return MemberReferenceTable.GetReferenceToken(fieldFullyQualifiedName, () => new MemberRefMD()
            {
                MemberDefKind = (int)MDRecordKinds.FieldRef,
                MemberDefToken = fieldDefToken,
                TypeRefToken = parentRefToken
            });
        }
        public IAssemblyBuilder Build(Stream stream)
        {
            var lexer = new AssemblierLexer(CharStreams.fromStream(stream));
            var parser = new Assemblier(new CommonTokenStream(lexer));
            ResolveStart(parser.start());
            return this;
        }
    }
}
