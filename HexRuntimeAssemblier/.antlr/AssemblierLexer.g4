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
EXCLAMATION: '!';

KEY_NAMESPACE: '.namespace';
KEY_ASSEMBLY: '.assembly';

KEY_INTERFACE: '.interface';
KEY_CLASS: '.class';
KEY_STRUCT: '.struct';
KEY_METHOD: '.method';
KEY_PROPERTY: '.property';
KEY_EVENT: '.event';
KEY_FIELD: '.field';

KEY_INHERIT: '.inherits';
KEY_IMPLEMENT: '.implements';
KEY_GENERIC: '.generic';

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

KEY_TRUE: 'true';
KEY_FALSE: 'false';

CTOR: '.ctor' | '.cctor';
ARRAY : 'array';
VOID: 'void';

//Support for primitive type abbr. form
PRIMITIVE_INT: 'int32';
PRIMITIVE_UINT: 'uint32';
PRIMITIVE_LONG: 'int64';
PRIMITIVE_ULONG: 'uint64';
PRIMITIVE_SHORT: 'int16';
PRIMITIVE_USHORT: 'uint16';
PRIMITIVE_BYTE: 'int8';
PRIMITIVE_UBYTE: 'uint8';
PRIMITIVE_CHAR: 'char';
PRIMITIVE_R4: 'float';
PRIMITIVE_R8: 'double';
PRIMITIVE_STRING: 'string';
PRIMITIVE_OBJECT: 'object';
PRIMITIVE_BOOL: 'bool';

METHOD_MANAGED: 'managed';
METHOD_IMPORT: 'import';

METHOD_LOCAL: '.local';
METHOD_CODE: '.code';

PROPERTY_GET: '.get';
PROPERTY_SET: '.set';

EVENT_ADD: '.adder';
EVENT_REMOVE: '.remover';

IL_CMP_EQ: '.eq';
IL_CMP_NE: '.ne';
IL_CMP_GT: '.gt';
IL_CMP_LT: '.lt';
IL_CMP_GE: '.ge';
IL_CMP_LE: '.le';

IL_PRESUDO_LABEL: '.label';
//IL
IL_LDFLD: '.ldfld';
IL_LDFLDA: '.ldflda';
IL_LDLOC: '.ldloc';
IL_LDLOCA: '.ldloca';
IL_LDARG: '.ldarg';
IL_LDARGA: '.ldarga';
IL_LDELEM: '.ldelem';
IL_LDELEMA: '.ldelema';
IL_LDSTR: '.ldstr';
IL_LDC: '.ldc';
IL_LDFN: '.ldfn';
IL_LDNULL: '.ldnull';
IL_LDIND: '.ldind';

IL_STFLD: '.stfld';
IL_STLOC: '.stloc';
IL_STARG: '.starg';
IL_STELEM: '.stelem';
IL_STTA: '.stta';

IL_ADD: '.add';
IL_SUB: '.sub';
IL_MUL: '.mul';
IL_DIV: '.div';
IL_MOD: '.mod';
IL_AND: '.and';
IL_OR: '.or';
IL_XOR: '.xor';
IL_NOT: '.not';
IL_NEG: '.neg';
IL_CONV: '.conv';

IL_CALL: '.call';
IL_CALLVIRT: '.callvirt';
IL_RET: '.ret';
IL_CMP: '.cmp';
IL_JCC: '.jcc';
IL_JMP: '.jmp';
IL_THROW: '.throw';
IL_TRY: '.try';
IL_CATCH: '.catch';
IL_FINALLY: '.finally';

IL_ARC: '.arc';
IL_NEW: '.new';
IL_NEWARRAY: '.newarr';

IL_CAST: '.cast';
IL_BOX: '.box';
IL_UNBOX: '.unbox';

IL_DUP: '.dup';
IL_POP: '.pop';
IL_NOP: '.nop';

STRING: '"'.*?'"';
HEX: '0x'[0-9a-fA-F]+;
INT: '-'?[0-9]+;
NUMBER: '-'?[0-9]+('.'[0-9]+)?;
GUID: [0-9a-fA-F]+ '-' ([0-9a-fA-F]+ '-')+ [0-9a-fA-F]+;

IDENTIFIER: [a-zA-Z_@][a-zA-Z0-9_]*;