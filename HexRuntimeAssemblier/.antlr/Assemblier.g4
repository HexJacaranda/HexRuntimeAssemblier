parser grammar Assemblier;

options {
    language = CSharp;
    // 表示解析token的词法解析器使用SearchLexer
    tokenVocab = AssemblierLexer;
}

start: assemblyDef classDef*;

modifier_access: MODIFIER_PUBLIC | MODIFIER_PRIVATE | MODIFIER_PROTECTED | MODIFIER_INTERNAL;
modifier_life: MODIFIER_INSTANCE | MODIFIER_STATIC;

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
methodOpcodeOperand: HEX | NUMBER | INT | STRING | methodRef | fieldRef | type | typeName;

methodBody: methodLocals? methodCode;
methodDef: KEY_METHOD
    modifier_access
    modifier_life
    MODIFIER_VIRTUAL?
    METHOD_PROPERTY?  
    methodReturnType methodName PARAM_BEGIN methodArgumentList PARAM_END methodSource
    BODY_BEGIN
        methodBody
    BODY_END;

methodRef: methodReturnType typeRef JUNCTION methodName PARAM_BEGIN type* PARAM_END;

//Field
fieldDef: KEY_FIELD modifier_access modifier_life type IDENTIFIER;
fieldRef: typeRef JUNCTION IDENTIFIER;

//Property
propertyGet: PROPERTY_GET methodRef;
propertySet: PROPERTY_SET methodRef;
propertyDef: KEY_PROPERTY
    BODY_BEGIN
        propertyGet?
        propertySet?
    BODY_END;

//Class
typeRefList: (typeRef COMMA)* typeRef;
implementList: KEY_IMPLEMENT typeRefList;
typeInherit: KEY_INHERIT typeRef;

//Type part
type: (PRIMITIVE_TYPE | typeRef | typeArray | typeInteriorRef);

typeArray: typeNestArray | typeMultidimensionArray;
typeNestArray: ARRAY LBRACE type RBRACE;
typeMultidimensionArray: ARRAY LBRACE type COMMA INT RBRACE;
typeInteriorRef: (PRIMITIVE_TYPE | typeRef | typeArray) REF;

assemblyRef: LMID IDENTIFIER RMID;
typeRef: assemblyRef? typeName;
typeName: (IDENTIFIER DOT)+ IDENTIFIER;

classBody: (methodDef | propertyDef | fieldDef SEMICOLON | classDef)*;

classDef: KEY_CLASS 
MODIFIER_NEST? 
(MODIFIER_ABSTRACT | MODIFIER_SEALED | MODIFIER_INTERFACE)? 
modifier_access 
modifier_life typeName
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


