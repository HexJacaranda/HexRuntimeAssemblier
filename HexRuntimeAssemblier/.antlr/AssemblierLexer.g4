lexer grammar AssemblierLexer;

options {
    language = CSharp;
}

WS : [ \t\n\r]+ -> skip;

LBRACE: '<';
RBRACE: '>';
LMID: '[';
RMID: ']';
BODY_BEGIN: '{';
BODY_END: '}';
PARAM_BEGIN: '(';
PARAM_END: ')';
EQ: '=';
DOT: '.';
COMMA: ',';
REF: '&';
JUNCTION: '::';
COLON: ':';

KEY_ASSEMBLY: '.assembly';

KEY_INTERFACE: '.interface';
KEY_CLASS: '.class';
KEY_STRUCT: '.struct';
KEY_METHOD: '.method';
KEY_PROPERTY: '.property';
KEY_EVENT: '.event';
KEY_FIELD: '.field';

KEY_INHERIT: 'inherits';
KEY_IMPLEMENT: 'implements';


MODIFIER_ABSTRACT: 'abstract';
MODIFIER_VIRTUAL: 'virtual';
MODIFIER_SEALED: 'sealed';
MODIFIER_STATIC: 'static';
MODIFIER_INSTANCE: 'instance';

MODIFIER_VOLATILE: 'volatile';
MODIFIER_THREAD_LOCAL: 'threadlocal';
MODIFIER_CONSTANT: 'const';
MODIFIER_READONLY: 'readonly';
MODIFIER_ATTRIBUTE: 'attribute';

MODIFIER_NEST: 'nested';

MODIFIER_PUBLIC: 'public';
MODIFIER_PRIVATE: 'private';
MODIFIER_INTERNAL: 'internal';
MODIFIER_PROTECTED: 'protected';

CTOR: '.ctor' | '.cctor';
ARRAY : 'array';
VOID: 'void';

PRIMITIVE_TYPE: PRIMITIVE_INT |
        PRIMITIVE_LONG |
        PRIMITIVE_SHORT |
        PRIMITIVE_BYTE |
        PRIMITIVE_CHAR |
        PRIMITIVE_STRING;

//Support for primitive type abbr. form
PRIMITIVE_INT: 'int32';
PRIMITIVE_LONG: 'int64';
PRIMITIVE_SHORT: 'int16';
PRIMITIVE_BYTE: 'int8';
PRIMITIVE_CHAR: 'char';
PRIMITIVE_STRING: 'string';

METHOD_MANAGED: 'managed';
METHOD_IMPORT: 'import';

METHOD_LOCAL: '.local';
METHOD_CODE: '.code';

PROPERTY_GET: '.get';
PROPERTY_SET: '.set';

EVENT_ADD: '.add';
EVENT_REMOVE: '.remove';

STRING: '"'.*?'"';
HEX: '0x'[0-9a-fA-F]+;
INT: [0-9]+;
NUMBER: [0-9]+('.'[0-9]+)?;
GUID: [0-9a-fA-F]+ '-' ([0-9a-fA-F]+ '-')+ [0-9a-fA-F]+;

IDENTIFIER: [a-zA-Z_@][a-zA-Z0-9_]*;