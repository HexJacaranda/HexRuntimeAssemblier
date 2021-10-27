parser grammar Assemblier;

options {
    language = CSharp;
    // 表示解析token的词法解析器使用SearchLexer
    tokenVocab = AssemblierLexer;
}

start: assemblyDef classDef*;

modifierAccess: MODIFIER_PUBLIC | MODIFIER_PRIVATE | MODIFIER_PROTECTED | MODIFIER_INTERNAL;
modifierLife: MODIFIER_INSTANCE | MODIFIER_STATIC;

//Method
//1. Argument
methodArgument: type IDENTIFIER;
methodArgumentList: (methodArgument (COMMA methodArgument)*)?;
//2. Return type
methodReturnType: VOID | type;
//3. name
methodName: CTOR | IDENTIFIER;
//4. special mark
methodImport: METHOD_IMPORT LMID STRING COMMA STRING RMID;
methodSource: METHOD_MANAGED | methodImport;
//- DEF

methodLocal: LMID INT RMID type IDENTIFIER;

methodLocals: METHOD_LOCAL
    BODY_BEGIN
        methodLocal+
    BODY_END;

//IL
methodCode: METHOD_CODE
    BODY_BEGIN
        ilInstruction+
    BODY_END;

methodProperty: PROPERTY_GET | PROPERTY_SET;

methodBody: methodLocals? methodCode;
methodDef: KEY_METHOD
    methodProperty?
    modifierAccess
    modifierLife
    MODIFIER_VIRTUAL? 
    methodReturnType methodName PARAM_BEGIN methodArgumentList PARAM_END methodSource
    genericList?
    BODY_BEGIN
        methodBody
    BODY_END;

methodParentType: type;
methodRef: methodParentType JUNCTION methodName genericParameterList? PARAM_BEGIN type* PARAM_END;

//Field
fieldDef: KEY_FIELD modifierAccess modifierLife MODIFIER_THREAD_LOCAL? (MODIFIER_VOLATILE | MODIFIER_CONSTANT | MODIFIER_READONLY)? type IDENTIFIER;
fieldRef: type JUNCTION IDENTIFIER;

//Property
propertyGet: PROPERTY_GET methodRef;
propertySet: PROPERTY_SET methodRef;
propertyDef: KEY_PROPERTY type IDENTIFIER
    BODY_BEGIN
        propertyGet?
        propertySet?
    BODY_END;

//Event
eventAdd: EVENT_ADD methodRef;
eventRemove: EVENT_REMOVE methodRef;
eventDef: KEY_EVENT type IDENTIFIER
    BODY_BEGIN
        eventAdd?
        eventRemove?
    BODY_END;

//Class
inheritOrImplementType: typeRef | genericParameterRef | PRIMITIVE_OBJECT;
typeRefList: (inheritOrImplementType COMMA)* inheritOrImplementType;
implementList: KEY_IMPLEMENT typeRefList;
typeInherit: KEY_INHERIT inheritOrImplementType;

//Type part
assemblyRef: LMID IDENTIFIER RMID;
typeRef: assemblyRef typeName;
typeName: typeRefNamespace? (typeRefNode DOT)* typeRefNode;

typeRefNamespace: LMID namespaceValue RMID;
typeRefGeneric: IDENTIFIER LBRACE (type COMMA)* type RBRACE;
typeRefPlain: IDENTIFIER;
typeRefNode: typeRefGeneric | typeRefPlain ;


primitiveType: PRIMITIVE_INT |
        PRIMITIVE_UINT |
        PRIMITIVE_LONG |
        PRIMITIVE_ULONG |
        PRIMITIVE_SHORT |
        PRIMITIVE_USHORT |
        PRIMITIVE_BYTE |
        PRIMITIVE_UBYTE |
        PRIMITIVE_CHAR |
        PRIMITIVE_R4 |
        PRIMITIVE_R8 |
        PRIMITIVE_STRING |
        PRIMITIVE_OBJECT |
        PRIMITIVE_BOOL;

genericParameterList: LBRACE (type COMMA)* type RBRACE;
type: (primitiveType | typeRef | arrayType | interiorRefType | genericParameterRef);
genericParameterRef: EXCLAMATION IDENTIFIER;
arrayType: nestArrayType | multidimensionArrayType;
nestArrayType: ARRAY LBRACE type RBRACE;
multidimensionArrayType: ARRAY LBRACE type COMMA INT RBRACE;
interiorRefType: (primitiveType | typeRef | arrayType) REF;

genericList: KEY_GENERIC IDENTIFIER+;

classBody: (methodDef | propertyDef | eventDef | fieldDef | classDef)*;

className: IDENTIFIER;
namespaceValue: (IDENTIFIER DOT)* IDENTIFIER;
classNameSpace: KEY_NAMESPACE namespaceValue;

classDef: (KEY_STRUCT | KEY_CLASS | KEY_INTERFACE) 
MODIFIER_NEST? 
MODIFIER_ATTRIBUTE?
(MODIFIER_ABSTRACT | MODIFIER_SEALED)? 
modifierAccess 
modifierLife 
className
classNameSpace?
typeInherit?
implementList?
genericList?
BODY_BEGIN
    classBody
BODY_END;

propertyValue: STRING | NUMBER | GUID | HEX | INT;
propertyKey: IDENTIFIER;
property: propertyKey EQ propertyValue;
assemblyDef: KEY_ASSEMBLY
BODY_BEGIN
    property+
BODY_END;

opLabel: IL_PRESUDO_LABEL IDENTIFIER;
//IL
opLdFld: IL_LDFLD fieldRef;
opLdFldA: IL_LDFLDA fieldRef;

opLdLoc: IL_LDLOC IDENTIFIER;
opLdLocA: IL_LDLOCA IDENTIFIER;

opLdArg: IL_LDARG IDENTIFIER;
opLdArgA: IL_LDARGA IDENTIFIER;

opLdElem: IL_LDELEM;
opLdElemA: IL_LDELEMA;

opLdStr: IL_LDSTR STRING;

opConstant: INT | HEX | NUMBER | KEY_TRUE | KEY_FALSE;
opLdC: IL_LDC primitiveType opConstant;

opLdFn: IL_LDFN methodRef;
opLdNull: IL_LDNULL;

opStFld: IL_STFLD fieldRef;
opStLoc: IL_STLOC IDENTIFIER;
opStArg: IL_STARG IDENTIFIER;
opStElem: IL_STELEM;
opStTA: IL_STTA;

opAdd: IL_ARC? IL_ADD;
opSub: IL_ARC? IL_SUB;
opMul: IL_ARC? IL_MUL;
opDiv: IL_ARC? IL_DIV;

opMod: IL_MOD;
opAnd: IL_AND;
opOr: IL_OR;
opXor: IL_XOR;
opNot: IL_NOT;
opNeg: IL_NEG;

opConv: IL_CONV primitiveType;

opCall: IL_CALL methodRef;
opCallVirt: IL_CALLVIRT methodRef;
opRet: IL_RET;


opCmpCond: IL_CMP_EQ | IL_CMP_NE | IL_CMP_GE | IL_CMP_GT | IL_CMP_LE | IL_CMP_LT;
opCmp: IL_CMP opCmpCond;

opJcc: IL_JCC IDENTIFIER;
opJmp: IL_JMP IDENTIFIER;

opThrow: IL_THROW;
opTry: IL_TRY IDENTIFIER;
opCatch: IL_CATCH typeRef;
opFinally: IL_FINALLY;

opNew: IL_NEW methodRef;
opNewArr: IL_NEWARRAY typeRef;

opCast: IL_CAST typeRef;
opBox: IL_BOX;
opUnBox: IL_UNBOX typeRef;

opDup: IL_DUP;
opPop: IL_POP;
opNop: IL_NOP;

ilInstruction: opLabel | 
        opLdFld | 
        opLdFldA | 
        opLdLoc | 
        opLdLocA | 
        opLdArg | 
        opLdArgA | 
        opLdElem | 
        opLdElemA | 
        opLdStr | 
        opLdC | 
        opLdFn | 
        opLdNull |
        opStFld |
        opStLoc |
        opStArg |
        opStElem |
        opStTA |
        opAdd |
        opSub |
        opMul |
        opDiv |
        opMod |
        opAnd |
        opOr |
        opXor |
        opNot |
        opNeg |
        opConv |
        opCall |
        opCallVirt |
        opRet |
        opCmp |
        opJcc |
        opJmp |
        opThrow |
        opTry |
        opCatch |
        opFinally |
        opNew |
        opNewArr |
        opDup |
        opPop |
        opNop;