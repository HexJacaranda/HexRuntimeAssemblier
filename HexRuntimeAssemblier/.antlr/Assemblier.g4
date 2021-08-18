parser grammar Assemblier;

options {
    language = CSharp;
    // 表示解析token的词法解析器使用SearchLexer
    tokenVocab = AssemblierLexer;
}

start: assemblyDef classDef*;

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
        (methodLocal COLON)+
    BODY_END;

//IL
methodCode: METHOD_CODE
    BODY_BEGIN
        methodIl+
    BODY_END;

methodIl: DOT IDENTIFIER methodOpcodeOperand*;
methodOpcodeOperand: HEX | NUMBER | STRING | methodRef | fieldRef | type | typeName;

methodBody: methodLocals? methodCode;
methodDef: KEY_METHOD
    MODIFIER_ACCESS
    MODIFIER_LIFE
    METHOD_PROPERTY?
    methodReturnType methodName PARAM_BEGIN methodArgumentList PARAM_END methodSource
    BODY_BEGIN
        methodBody
    BODY_END;

methodRef: methodReturnType typeRef JUNCTION methodName PARAM_BEGIN type* PARAM_END;

//Field
fieldDef: MODIFIER_ACCESS MODIFIER_LIFE type IDENTIFIER;
fieldRef: typeRef JUNCTION IDENTIFIER;

//Property
propertyGet: PROPERTY_GET methodRef;
propertySet: PROPERTY_SET methodRef;
propertyDef: KEY_PROPERTY
    BODY_BEGIN
        (propertyGet COLON)?
        (propertySet COLON)?
    BODY_END;

//Class
typeRefList: (typeRef COMMA)* typeRef;
implementList: KEY_IMPLEMENT typeRefList;

//Type part
type: (PRIMITIVE_TYPE | typeRef | typeArray | typeInteriorRef);

typeArray: typeNestArray | typeMultidimensionArray;
typeNestArray: ARRAY LBRACE type RBRACE;
typeMultidimensionArray: ARRAY LBRACE type COMMA INT RBRACE;
typeInteriorRef: (PRIMITIVE_TYPE | typeRef | typeArray) REF;

typeRef: (LMID IDENTIFIER RMID)? typeName;
typeName: (IDENTIFIER DOT)+ IDENTIFIER;

classBody: (methodDef | propertyDef | fieldDef COLON | classDef)*;

classDef: KEY_CLASS MODIFIER_NEST? MODIFIER_ACCESS MODIFIER_LIFE typeName
(KEY_INHERIT typeRef)?
implementList?
BODY_BEGIN
    classBody
BODY_END;

propertyValue: STRING | NUMBER | GUID | HEX;
propertyKey: IDENTIFIER;
property: propertyKey EQ propertyValue;
assemblyDef: KEY_ASSEMBLY
BODY_BEGIN
    property+
BODY_END;


