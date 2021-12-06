using HexRuntimeAssemblier.Interfaces;
using HexRuntimeAssemblier.Meta;

namespace AssemblierTest
{
    public static class BuilderHelper
    {
        public static string GetString(this uint token, IAssemblyBuilder builder)
            => builder.MetaStringTable.Contents[(int)token];
        public static TypeMD GetTypeDef(this IAssemblyBuilder builder, string FQN)
        {
            uint token = builder.DefinitionTables[MDRecordKinds.TypeDef].GetDefinitionToken(FQN, null);
            return builder.DefinitionTables[MDRecordKinds.TypeDef][token] as TypeMD;
        }
        public static MethodMD GetMethodDef(this IAssemblyBuilder builder, string FQN)
        {
            uint token = builder.DefinitionTables[MDRecordKinds.MethodDef].GetDefinitionToken(FQN, null);
            return builder.DefinitionTables[MDRecordKinds.MethodDef][token] as MethodMD;
        }
        public static FieldMD GetFieldDef(this IAssemblyBuilder builder, string FQN)
        {
            uint token = builder.DefinitionTables[MDRecordKinds.FieldDef].GetDefinitionToken(FQN, null);
            return builder.DefinitionTables[MDRecordKinds.FieldDef][token] as FieldMD;
        }
        public static PropertyMD GetPropertyDef(this IAssemblyBuilder builder, string FQN)
        {
            uint token = builder.DefinitionTables[MDRecordKinds.PropertyDef].GetDefinitionToken(FQN, null);
            return builder.DefinitionTables[MDRecordKinds.PropertyDef][token] as PropertyMD;
        }
        public static EventMD GetEventDef(this IAssemblyBuilder builder, string FQN)
        {
            uint token = builder.DefinitionTables[MDRecordKinds.EventDef].GetDefinitionToken(FQN, null);
            return builder.DefinitionTables[MDRecordKinds.EventDef][token] as EventMD;
        }
        public static uint GetTypeRefToken(this IAssemblyBuilder builder, string FQN)
            => builder.ReferenceTables[MDRecordKinds.TypeRef].GetReferenceToken(FQN, null);

        public static uint GetFieldRefToken(this IAssemblyBuilder builder, string FQN)
            => builder.ReferenceTables[MDRecordKinds.FieldRef].GetReferenceToken(FQN, null);

        public static uint GetMethodRefToken(this IAssemblyBuilder builder, string FQN)
            => builder.ReferenceTables[MDRecordKinds.FieldRef].GetReferenceToken(FQN, null);
    }
}
