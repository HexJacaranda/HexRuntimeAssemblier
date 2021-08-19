using System;
using System.Collections.Generic;

using MDToken = System.UInt32;
namespace HexRuntimeAssemblier
{
    enum MDRecordKinds
	{
		String,
		Argument,
		GenericParameter,
		TypeDef,
		AttributeDef,
		MethodDef,
		FieldDef,
		PropertyDef,
		EventDef,
		//Used for counting
		KindLimit,
		TypeRef,
		FieldRef,
		MethodRef,
		PropertyRef,
		EventRef
	};
	class AssemblyHeaderMD
	{
		public MDToken NameToken;
		public Int32 MajorVersion;
		public Int32 MinorVersion;
		public MDToken GroupNameToken;
		public Guid GUID;
	};

	class RefTableHeaderMD
	{
		public Int32 TypeRefTableOffset;
		public Int32 TypeRefCount;
		public Int32 MemberRefTableOffset;
		public Int32 MemberRefCount;
		public Int32 AssemblyRefTableOffset;
		public Int32 AssemblyRefCount;
	};


	class MDIndexTable
	{
		public MDRecordKinds Kind;
		public IList<Int32> Offsets;
	}

	class TypeRefMD
	{
		public MDToken AssemblyToken;
		public MDToken TypeDefToken;
	}


	class MemberRefMD
	{
		public MDToken TypeRefToken;
		public MDRecordKinds MemberDefKind;
		public MDToken MemberDefToken;
	}

	class AssemblyRefMD
	{
		public const MDToken Self = 0u;
		public Guid GUID;
		public MDToken AssemblyName;
	}

	class StringMD
	{
		public char[] Sequence;
	}

	class AtrributeMD
	{
		public MDRecordKinds ParentKind;
		public MDToken ParentToken;
		public MDToken TypeRefToken;
		public List<byte> Attribute;
	}

	class GenericParamterMD
	{
		public MDToken NameToken;
	}

	[Flags]
	enum FieldFlag : ushort
	{
		Volatile = 0x0001,
		Static = 0x0002,
		Constant = 0x0004,
		ReadOnly = 0x0008,
		ThreadLocal = 0x0010
	}

	class FieldMD
	{
		public MDToken ParentTypeRefToken;
		public MDToken TypeRefToken;
		public MDToken NameToken;
		public byte Accessibility;
		public FieldFlag Flags;
		public IList<MDToken> AttributeTokens;
	}

	class PropertyMD
	{
		public MDToken ParentTypeRefToken;
		public MDToken TypeRefToken;
		public MDToken SetterToken;
		public MDToken GetterToken;
		public MDToken BackingFieldToken;
		public MDToken NameToken;
		public byte Accessibility;
		public ushort Flags;
		public IList<MDToken> AttributeTokens;
	}

	class EventMD
	{
		public MDToken ParentTypeRefToken;
		public MDToken TypeRefToken;
		public MDToken AdderToken;
		public MDToken RemoverToken;
		public MDToken BackingFieldToken;
		public MDToken NameToken;
		public byte Accessibility;
		public ushort Flags;
		public IList<MDToken> AttributeTokens;
	}

	class ArgumentMD
	{
		public MDToken TypeRefToken;
		public MDToken NameToken;
		public MDToken DefaultStringValueToken;
		public Int64 DefaultValue;
		public IList<MDToken> AttributeTokens;
	}

	class MethodSignatureMD
	{
		public MDToken ReturnTypeRefToken;
		public IList<MDToken> ArgumentTokens;
	}

	class LocalVariableMD
	{
		public MDToken TypeRefToken;
		public MDToken NameToken;
	}

	class ILMD
	{

		public IList<LocalVariableMD> LocalVariables;
		public IList<byte> IL;
	}

	class NativeLinkMD
	{

	}

	class MethodMD
	{
		public MDToken ParentTypeRefToken;
		public MDToken NameToken;
		public byte Accessibility;

		public ushort Flags;

		public MethodSignatureMD Signature;
		public MDToken OverridesMethodRef;
		public ILMD ILCodeMD;
		public IList<NativeLinkMD> NativeLinks;
		public IList<MDToken> AttributeTokens;
	}

	[Flags]
	enum TypeFlag : ushort
    {
		Sealed = 0x0001,
		Abstract = 0x0002,
		Struct = 0x0004,
		Interface = 0x0008,
		Attribute = 0x0010,
		Generic = 0x0020,
		Nested = 0x0040
    }

	class TypeMD
	{
		public MDToken ParentAssemblyToken;
		public MDToken ParentTypeRefToken;
		public MDToken NameToken;
		public MDToken EnclosingTypeRefToken;
		public MDToken CanonicalTypeRefToken;
		public MDToken NamespaceToken;
		public byte CoreType;
		public byte Accessibility;

		public TypeFlag Flags;

		public IList<MDToken> FieldTokens;
		public IList<MDToken> MethodTokens;
		public IList<MDToken> PropertyTokens;
		public IList<MDToken> EventTokens;
		public IList<MDToken> InterfaceTokens;
		public IList<MDToken> GenericParameterTokens;
		public IList<MDToken> AttributeTokens;
	}
}
