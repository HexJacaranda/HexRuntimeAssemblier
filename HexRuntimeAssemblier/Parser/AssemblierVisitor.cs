//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.8
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from c:\Users\I525976\source\repos\HexRuntimeAssemblier\HexRuntimeAssemblier\.antlr\Assemblier.g4 by ANTLR 4.8

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
	/// Visit a parse tree produced by <see cref="Assemblier.methodIl"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMethodIl([NotNull] Assemblier.MethodIlContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.methodLabel"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMethodLabel([NotNull] Assemblier.MethodLabelContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.methodOpcodeOperand"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitMethodOpcodeOperand([NotNull] Assemblier.MethodOpcodeOperandContext context);
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
	/// Visit a parse tree produced by <see cref="Assemblier.type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitType([NotNull] Assemblier.TypeContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.typeArray"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTypeArray([NotNull] Assemblier.TypeArrayContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.typeNestArray"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTypeNestArray([NotNull] Assemblier.TypeNestArrayContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.typeMultidimensionArray"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTypeMultidimensionArray([NotNull] Assemblier.TypeMultidimensionArrayContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="Assemblier.typeInteriorRef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitTypeInteriorRef([NotNull] Assemblier.TypeInteriorRefContext context);
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
	/// Visit a parse tree produced by <see cref="Assemblier.classBody"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitClassBody([NotNull] Assemblier.ClassBodyContext context);
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
}
