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
using IParseTreeListener = Antlr4.Runtime.Tree.IParseTreeListener;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete listener for a parse tree produced by
/// <see cref="Assemblier"/>.
/// </summary>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.8")]
[System.CLSCompliant(false)]
public interface IAssemblierListener : IParseTreeListener {
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.start"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterStart([NotNull] Assemblier.StartContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.start"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitStart([NotNull] Assemblier.StartContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.modifierAccess"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterModifierAccess([NotNull] Assemblier.ModifierAccessContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.modifierAccess"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitModifierAccess([NotNull] Assemblier.ModifierAccessContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.modifierLife"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterModifierLife([NotNull] Assemblier.ModifierLifeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.modifierLife"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitModifierLife([NotNull] Assemblier.ModifierLifeContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.methodArgument"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMethodArgument([NotNull] Assemblier.MethodArgumentContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.methodArgument"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMethodArgument([NotNull] Assemblier.MethodArgumentContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.methodArgumentList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMethodArgumentList([NotNull] Assemblier.MethodArgumentListContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.methodArgumentList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMethodArgumentList([NotNull] Assemblier.MethodArgumentListContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.methodReturnType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMethodReturnType([NotNull] Assemblier.MethodReturnTypeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.methodReturnType"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMethodReturnType([NotNull] Assemblier.MethodReturnTypeContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.methodName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMethodName([NotNull] Assemblier.MethodNameContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.methodName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMethodName([NotNull] Assemblier.MethodNameContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.methodImport"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMethodImport([NotNull] Assemblier.MethodImportContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.methodImport"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMethodImport([NotNull] Assemblier.MethodImportContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.methodSource"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMethodSource([NotNull] Assemblier.MethodSourceContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.methodSource"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMethodSource([NotNull] Assemblier.MethodSourceContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.methodLocal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMethodLocal([NotNull] Assemblier.MethodLocalContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.methodLocal"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMethodLocal([NotNull] Assemblier.MethodLocalContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.methodLocals"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMethodLocals([NotNull] Assemblier.MethodLocalsContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.methodLocals"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMethodLocals([NotNull] Assemblier.MethodLocalsContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.methodCode"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMethodCode([NotNull] Assemblier.MethodCodeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.methodCode"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMethodCode([NotNull] Assemblier.MethodCodeContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.methodIl"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMethodIl([NotNull] Assemblier.MethodIlContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.methodIl"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMethodIl([NotNull] Assemblier.MethodIlContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.methodLabel"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMethodLabel([NotNull] Assemblier.MethodLabelContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.methodLabel"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMethodLabel([NotNull] Assemblier.MethodLabelContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.methodOpcodeOperand"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMethodOpcodeOperand([NotNull] Assemblier.MethodOpcodeOperandContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.methodOpcodeOperand"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMethodOpcodeOperand([NotNull] Assemblier.MethodOpcodeOperandContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.methodProperty"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMethodProperty([NotNull] Assemblier.MethodPropertyContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.methodProperty"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMethodProperty([NotNull] Assemblier.MethodPropertyContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.methodBody"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMethodBody([NotNull] Assemblier.MethodBodyContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.methodBody"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMethodBody([NotNull] Assemblier.MethodBodyContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.methodDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMethodDef([NotNull] Assemblier.MethodDefContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.methodDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMethodDef([NotNull] Assemblier.MethodDefContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.methodRef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterMethodRef([NotNull] Assemblier.MethodRefContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.methodRef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitMethodRef([NotNull] Assemblier.MethodRefContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.fieldDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFieldDef([NotNull] Assemblier.FieldDefContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.fieldDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFieldDef([NotNull] Assemblier.FieldDefContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.fieldRef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterFieldRef([NotNull] Assemblier.FieldRefContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.fieldRef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitFieldRef([NotNull] Assemblier.FieldRefContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.propertyGet"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPropertyGet([NotNull] Assemblier.PropertyGetContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.propertyGet"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPropertyGet([NotNull] Assemblier.PropertyGetContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.propertySet"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPropertySet([NotNull] Assemblier.PropertySetContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.propertySet"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPropertySet([NotNull] Assemblier.PropertySetContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.propertyDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPropertyDef([NotNull] Assemblier.PropertyDefContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.propertyDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPropertyDef([NotNull] Assemblier.PropertyDefContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.eventAdd"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterEventAdd([NotNull] Assemblier.EventAddContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.eventAdd"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitEventAdd([NotNull] Assemblier.EventAddContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.eventRemove"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterEventRemove([NotNull] Assemblier.EventRemoveContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.eventRemove"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitEventRemove([NotNull] Assemblier.EventRemoveContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.eventDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterEventDef([NotNull] Assemblier.EventDefContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.eventDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitEventDef([NotNull] Assemblier.EventDefContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.typeRefList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTypeRefList([NotNull] Assemblier.TypeRefListContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.typeRefList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTypeRefList([NotNull] Assemblier.TypeRefListContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.implementList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterImplementList([NotNull] Assemblier.ImplementListContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.implementList"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitImplementList([NotNull] Assemblier.ImplementListContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.typeInherit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTypeInherit([NotNull] Assemblier.TypeInheritContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.typeInherit"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTypeInherit([NotNull] Assemblier.TypeInheritContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterType([NotNull] Assemblier.TypeContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.type"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitType([NotNull] Assemblier.TypeContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.typeArray"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTypeArray([NotNull] Assemblier.TypeArrayContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.typeArray"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTypeArray([NotNull] Assemblier.TypeArrayContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.typeNestArray"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTypeNestArray([NotNull] Assemblier.TypeNestArrayContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.typeNestArray"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTypeNestArray([NotNull] Assemblier.TypeNestArrayContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.typeMultidimensionArray"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTypeMultidimensionArray([NotNull] Assemblier.TypeMultidimensionArrayContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.typeMultidimensionArray"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTypeMultidimensionArray([NotNull] Assemblier.TypeMultidimensionArrayContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.typeInteriorRef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTypeInteriorRef([NotNull] Assemblier.TypeInteriorRefContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.typeInteriorRef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTypeInteriorRef([NotNull] Assemblier.TypeInteriorRefContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.assemblyRef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAssemblyRef([NotNull] Assemblier.AssemblyRefContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.assemblyRef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAssemblyRef([NotNull] Assemblier.AssemblyRefContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.typeRef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTypeRef([NotNull] Assemblier.TypeRefContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.typeRef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTypeRef([NotNull] Assemblier.TypeRefContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.typeName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterTypeName([NotNull] Assemblier.TypeNameContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.typeName"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitTypeName([NotNull] Assemblier.TypeNameContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.classBody"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterClassBody([NotNull] Assemblier.ClassBodyContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.classBody"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitClassBody([NotNull] Assemblier.ClassBodyContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.classDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterClassDef([NotNull] Assemblier.ClassDefContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.classDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitClassDef([NotNull] Assemblier.ClassDefContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.propertyValue"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPropertyValue([NotNull] Assemblier.PropertyValueContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.propertyValue"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPropertyValue([NotNull] Assemblier.PropertyValueContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.propertyKey"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterPropertyKey([NotNull] Assemblier.PropertyKeyContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.propertyKey"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitPropertyKey([NotNull] Assemblier.PropertyKeyContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.property"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterProperty([NotNull] Assemblier.PropertyContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.property"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitProperty([NotNull] Assemblier.PropertyContext context);
	/// <summary>
	/// Enter a parse tree produced by <see cref="Assemblier.assemblyDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void EnterAssemblyDef([NotNull] Assemblier.AssemblyDefContext context);
	/// <summary>
	/// Exit a parse tree produced by <see cref="Assemblier.assemblyDef"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	void ExitAssemblyDef([NotNull] Assemblier.AssemblyDefContext context);
}
