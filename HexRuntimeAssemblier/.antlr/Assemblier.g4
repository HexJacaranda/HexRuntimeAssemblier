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
methodArgumentList: (methodArgument COMMA methodArgumentList) | ;
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
        (methodIl | methodLabel)+
    BODY_END;

methodIl: DOT IDENTIFIER methodOpcodeOperand*;
methodLabel: DOT IDENTIFIER COLON;
methodOpcodeOperand: HEX | NUMBER | INT | STRING | methodRef | fieldRef | type ;
methodProperty: PROPERTY_GET | PROPERTY_SET;

methodBody: methodLocals? methodCode;
methodDef: KEY_METHOD
    methodProperty?
    modifierAccess
    modifierLife
    MODIFIER_VIRTUAL? 
    methodReturnType methodName PARAM_BEGIN methodArgumentList PARAM_END methodSource
    BODY_BEGIN
        methodBody
    BODY_END;

methodRef: methodReturnType typeRef JUNCTION methodName PARAM_BEGIN type* PARAM_END;

//Field
fieldDef: KEY_FIELD modifierAccess modifierLife MODIFIER_THREAD_LOCAL? (MODIFIER_VOLATILE | MODIFIER_CONSTANT | MODIFIER_READONLY)? type IDENTIFIER;
fieldRef: type typeRef JUNCTION IDENTIFIER;

//Property
propertyGet: PROPERTY_GET methodRef;
propertySet: PROPERTY_SET methodRef;
propertyDef: KEY_PROPERTY
    BODY_BEGIN
        propertyGet?
        propertySet?
    BODY_END;

//Event
eventAdd: PROPERTY_GET methodRef;
eventRemove: PROPERTY_SET methodRef;
eventDef: KEY_EVENT
    BODY_BEGIN
        eventAdd?
        eventRemove?
    BODY_END;

//Class
typeRefList: (typeRef COMMA)* typeRef;
implementList: KEY_IMPLEMENT typeRefList;
typeInherit: KEY_INHERIT typeRef;

//Type part
assemblyRef: LMID IDENTIFIER RMID;
typeName: (IDENTIFIER DOT)+ IDENTIFIER;
typeRef: assemblyRef? typeName;

type: (PRIMITIVE_TYPE | typeRef | typeArray | typeInteriorRef);
typeArray: typeNestArray | typeMultidimensionArray;
typeNestArray: type LMID RMID;
typeMultidimensionArray: type LMID COMMA+ RMID;
typeInteriorRef: (PRIMITIVE_TYPE | typeRef | typeArray) REF;

classBody: (methodDef | propertyDef | eventDef | fieldDef | classDef)*;

classDef: (KEY_STRUCT | KEY_CLASS | KEY_INTERFACE) 
MODIFIER_NEST? 
MODIFIER_ATTRIBUTE?
(MODIFIER_ABSTRACT | MODIFIER_SEALED)? 
modifierAccess 
modifierLife typeName
typeInherit?
implementList?
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


