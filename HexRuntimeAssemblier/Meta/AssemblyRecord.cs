using System;
using System.Collections.Generic;
using HexRuntimeAssemblier.IL;
using MDToken = System.UInt32;

namespace HexRuntimeAssemblier.Meta
{
	public class Token
    {
		public const MDToken Null = uint.MaxValue;
    }
	public enum MDRecordKinds : short
	{
		String,
		Argument,
		GenericParameter,
		TypeDef,
		GenericInstantiationDef,
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
	public class AssemblyHeaderMD
	{
		public MDToken NameToken;
		public Int32 MajorVersion;
		public Int32 MinorVersion;
		public MDToken GroupNameToken;
		public Guid GUID;
	};

	public class RefTableHeaderMD
	{
		public Int32 TypeRefTableOffset;
		public Int32 TypeRefCount;
		public Int32 MemberRefTableOffset;
		public Int32 MemberRefCount;
		public Int32 AssemblyRefTableOffset;
		public Int32 AssemblyRefCount;
	};


	public class MDIndexTable
	{
		public MDRecordKinds Kind;
		public Int32[] Offsets;
	}

	public class TypeRefMD
	{
		public MDToken AssemblyToken;
		public MDRecordKinds DefKind;
		public MDToken Token;
	}


	public class MemberRefMD
	{
		public MDToken TypeRefToken;
		/// <summary>
		/// Should be aligned with MDRecordKinds
		/// </summary>
		public short MemberDefKind;
		public MDToken MemberDefToken;
	}

	public class AssemblyRefMD
	{
		public const MDToken Self = 0u;
		public Guid GUID;
		public MDToken AssemblyName;
	}

	public class StringMD
	{
		public char[] Sequence;
	}

	public class AtrributeMD
	{
		public MDRecordKinds ParentKind;
		public MDToken ParentToken;
		public MDToken TypeRefToken;
		public List<byte> Attribute;
	}

	public class GenericParamterMD
	{
		public MDToken NameToken;
	}

	public class GenericInstantiationMD
	{
		public MDToken CanonicalTypeDefToken;
		public MDToken[] GenericParameterTokens;
	};

	[Flags]
	public enum FieldFlag : ushort
	{
		Volatile = 0x0001,
		Static = 0x0002,
		Constant = 0x0004,
		ReadOnly = 0x0008,
		ThreadLocal = 0x0010
	}

	public class FieldMD
	{
		public MDToken ParentTypeRefToken;
		public MDToken TypeRefToken;
		public MDToken NameToken;
		public MDToken FullyQualifiedNameToken = Token.Null;
		public byte Accessibility;
		public FieldFlag Flags;
		public MDToken[] AttributeTokens;
	}

	public class PropertyMD
	{
		public MDToken ParentTypeRefToken;
		public MDToken TypeRefToken;
		public MDToken SetterToken;
		public MDToken GetterToken;
		public MDToken BackingFieldToken;
		public MDToken NameToken;
		public MDToken[] AttributeTokens;
	}

	public class EventMD
	{
		public MDToken ParentTypeRefToken;
		public MDToken TypeRefToken;
		public MDToken AdderToken;
		public MDToken RemoverToken;
		public MDToken BackingFieldToken;
		public MDToken NameToken;
		public MDToken[] AttributeTokens;
	}

	public class ArgumentMD
	{
		public MDToken TypeRefToken;
		public MDToken NameToken;
		public UInt64 DefaultStringValueToken;
		public MDToken[] AttributeTokens;
	}

	public class MethodSignatureMD
	{
		public MDToken ReturnTypeRefToken;
		public MDToken[] ArgumentTokens;
	}

	public class LocalVariableMD
	{
		public MDToken TypeRefToken;
		public MDToken NameToken;
	}

	public class ILMD
	{
		public LocalVariableMD[] LocalVariables;
		public byte[] IL;
	}

	public class NativeLinkMD
	{

	}

	[Flags]
	public enum MethodFlag : ushort
	{
		Virtual = 0x0001,
		Static = 0x0002,
		Override = 0x0004,
		Final = 0x0008,
		Generic = 0x0010,
		RTSpecial = 0x0020
	}

	public class MethodMD
	{
		public MDToken ParentTypeRefToken;
		public MDToken NameToken;
		public MDToken FullyQualifiedNameToken = Token.Null;
		public byte Accessibility;
		public MethodFlag Flags;
		public MethodSignatureMD Signature;
		public MDToken OverridesMethodRef = Token.Null;
		public ILMD ILCodeMD;
		public NativeLinkMD[] NativeLinks;
		public MDToken[] GenericParameterTokens;
		public MDToken[] AttributeTokens;
	}

	[Flags]
	public enum TypeFlag : ushort
    {
		Sealed = 0x0001,
		Abstract = 0x0002,
		Struct = 0x0004,
		Interface = 0x0008,
		Attribute = 0x0010,
		Generic = 0x0020,
		Nested = 0x0040
    }

	public class TypeMD
	{
		public MDToken ParentAssemblyToken = Token.Null;
		public MDToken ParentTypeRefToken = Token.Null;
		public MDToken NameToken = Token.Null;
		public MDToken FullyQualifiedNameToken = Token.Null;
		public MDToken EnclosingTypeRefToken = Token.Null;
		public MDToken CanonicalTypeRefToken = Token.Null;	
		public CoreTypes CoreType;
		public byte Accessibility;

		public TypeFlag Flags;

		public MDToken[] FieldTokens;
		public MDToken[] MethodTokens;
		public MDToken[] PropertyTokens;
		public MDToken[] EventTokens;
		public MDToken[] InterfaceTokens;
		public MDToken[] GenericParameterTokens;
		public MDToken[] AttributeTokens;
	}
}
