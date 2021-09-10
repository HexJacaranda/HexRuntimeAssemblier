//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.8
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from c:\Users\Hex\source\repos\HexJacaranda\HexRuntimeAssemblier\HexRuntimeAssemblier\.antlr\Assemblier.g4 by ANTLR 4.8

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="Assemblier"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.8")]
[System.CLSCompliant(false)]
public interface IAssemblierVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.start"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitStart([NotNull] Assemblier.StartContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.modifierAccess"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitModifierAccess([NotNull] Assemblier.ModifierAccessContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.modifierLife"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitModifierLife([NotNull] Assemblier.ModifierLifeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.methodArgument"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMethodArgument([NotNull] Assemblier.MethodArgumentContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.methodArgumentList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMethodArgumentList([NotNull] Assemblier.MethodArgumentListContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.methodReturnType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMethodReturnType([NotNull] Assemblier.MethodReturnTypeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.methodName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMethodName([NotNull] Assemblier.MethodNameContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.methodImport"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMethodImport([NotNull] Assemblier.MethodImportContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.methodSource"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMethodSource([NotNull] Assemblier.MethodSourceContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.methodLocal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMethodLocal([NotNull] Assemblier.MethodLocalContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.methodLocals"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMethodLocals([NotNull] Assemblier.MethodLocalsContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.methodCode"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMethodCode([NotNull] Assemblier.MethodCodeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.methodProperty"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMethodProperty([NotNull] Assemblier.MethodPropertyContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.methodBody"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMethodBody([NotNull] Assemblier.MethodBodyContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.methodDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMethodDef([NotNull] Assemblier.MethodDefContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.methodParentType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMethodParentType([NotNull] Assemblier.MethodParentTypeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.methodRef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMethodRef([NotNull] Assemblier.MethodRefContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.fieldDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFieldDef([NotNull] Assemblier.FieldDefContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.fieldRef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFieldRef([NotNull] Assemblier.FieldRefContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.propertyGet"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPropertyGet([NotNull] Assemblier.PropertyGetContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.propertySet"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPropertySet([NotNull] Assemblier.PropertySetContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.propertyDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPropertyDef([NotNull] Assemblier.PropertyDefContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.eventAdd"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEventAdd([NotNull] Assemblier.EventAddContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.eventRemove"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEventRemove([NotNull] Assemblier.EventRemoveContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.eventDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitEventDef([NotNull] Assemblier.EventDefContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.inheritOrImplementType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitInheritOrImplementType([NotNull] Assemblier.InheritOrImplementTypeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.typeRefList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTypeRefList([NotNull] Assemblier.TypeRefListContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.implementList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitImplementList([NotNull] Assemblier.ImplementListContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.typeInherit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTypeInherit([NotNull] Assemblier.TypeInheritContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.assemblyRef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAssemblyRef([NotNull] Assemblier.AssemblyRefContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.typeRef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTypeRef([NotNull] Assemblier.TypeRefContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.typeName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTypeName([NotNull] Assemblier.TypeNameContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.typeRefNamespace"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTypeRefNamespace([NotNull] Assemblier.TypeRefNamespaceContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.typeRefGeneric"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTypeRefGeneric([NotNull] Assemblier.TypeRefGenericContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.typeRefPlain"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTypeRefPlain([NotNull] Assemblier.TypeRefPlainContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.typeRefNode"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTypeRefNode([NotNull] Assemblier.TypeRefNodeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.primitiveType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPrimitiveType([NotNull] Assemblier.PrimitiveTypeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.genericParameterList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitGenericParameterList([NotNull] Assemblier.GenericParameterListContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitType([NotNull] Assemblier.TypeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.genericParameterRef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitGenericParameterRef([NotNull] Assemblier.GenericParameterRefContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.arrayType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArrayType([NotNull] Assemblier.ArrayTypeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.nestArrayType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNestArrayType([NotNull] Assemblier.NestArrayTypeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.multidimensionArrayType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMultidimensionArrayType([NotNull] Assemblier.MultidimensionArrayTypeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.interiorRefType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitInteriorRefType([NotNull] Assemblier.InteriorRefTypeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.genericList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitGenericList([NotNull] Assemblier.GenericListContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.classBody"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitClassBody([NotNull] Assemblier.ClassBodyContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.className"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitClassName([NotNull] Assemblier.ClassNameContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.namespaceValue"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitNamespaceValue([NotNull] Assemblier.NamespaceValueContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.classNameSpace"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitClassNameSpace([NotNull] Assemblier.ClassNameSpaceContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.classDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitClassDef([NotNull] Assemblier.ClassDefContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.propertyValue"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPropertyValue([NotNull] Assemblier.PropertyValueContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.propertyKey"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitPropertyKey([NotNull] Assemblier.PropertyKeyContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.property"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitProperty([NotNull] Assemblier.PropertyContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.assemblyDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAssemblyDef([NotNull] Assemblier.AssemblyDefContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opLabel"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpLabel([NotNull] Assemblier.OpLabelContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opLdFld"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpLdFld([NotNull] Assemblier.OpLdFldContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opLdFldA"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpLdFldA([NotNull] Assemblier.OpLdFldAContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opLdLoc"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpLdLoc([NotNull] Assemblier.OpLdLocContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opLdLocA"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpLdLocA([NotNull] Assemblier.OpLdLocAContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opLdArg"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpLdArg([NotNull] Assemblier.OpLdArgContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opLdArgA"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpLdArgA([NotNull] Assemblier.OpLdArgAContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opLdElem"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpLdElem([NotNull] Assemblier.OpLdElemContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opLdElemA"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpLdElemA([NotNull] Assemblier.OpLdElemAContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opLdStr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpLdStr([NotNull] Assemblier.OpLdStrContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opConstant"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpConstant([NotNull] Assemblier.OpConstantContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opLdC"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpLdC([NotNull] Assemblier.OpLdCContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opLdFn"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpLdFn([NotNull] Assemblier.OpLdFnContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opLdNull"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpLdNull([NotNull] Assemblier.OpLdNullContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opStFld"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpStFld([NotNull] Assemblier.OpStFldContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opStLoc"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpStLoc([NotNull] Assemblier.OpStLocContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opStArg"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpStArg([NotNull] Assemblier.OpStArgContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opStElem"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpStElem([NotNull] Assemblier.OpStElemContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opStTA"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpStTA([NotNull] Assemblier.OpStTAContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opAdd"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpAdd([NotNull] Assemblier.OpAddContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opSub"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpSub([NotNull] Assemblier.OpSubContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opMul"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpMul([NotNull] Assemblier.OpMulContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opDiv"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpDiv([NotNull] Assemblier.OpDivContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opMod"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpMod([NotNull] Assemblier.OpModContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opAnd"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpAnd([NotNull] Assemblier.OpAndContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opOr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpOr([NotNull] Assemblier.OpOrContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opXor"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpXor([NotNull] Assemblier.OpXorContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opNot"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpNot([NotNull] Assemblier.OpNotContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opNeg"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpNeg([NotNull] Assemblier.OpNegContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opConv"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpConv([NotNull] Assemblier.OpConvContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opCall"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpCall([NotNull] Assemblier.OpCallContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opCallVirt"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpCallVirt([NotNull] Assemblier.OpCallVirtContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opRet"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpRet([NotNull] Assemblier.OpRetContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opCmpCond"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpCmpCond([NotNull] Assemblier.OpCmpCondContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opCmp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpCmp([NotNull] Assemblier.OpCmpContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opJcc"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpJcc([NotNull] Assemblier.OpJccContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opJmp"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpJmp([NotNull] Assemblier.OpJmpContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opThrow"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpThrow([NotNull] Assemblier.OpThrowContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opTry"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpTry([NotNull] Assemblier.OpTryContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opCatch"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpCatch([NotNull] Assemblier.OpCatchContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opFinally"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpFinally([NotNull] Assemblier.OpFinallyContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opNew"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpNew([NotNull] Assemblier.OpNewContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opNewArr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpNewArr([NotNull] Assemblier.OpNewArrContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opDup"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpDup([NotNull] Assemblier.OpDupContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opPop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpPop([NotNull] Assemblier.OpPopContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.opNop"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitOpNop([NotNull] Assemblier.OpNopContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.ilInstruction"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIlInstruction([NotNull] Assemblier.IlInstructionContext context);
}
