using HexRuntimeAssemblier.Interfaces;
using HexRuntimeAssemblier.Meta;
using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace HexRuntimeAssemblier.Serialization
{
    /// <summary>
    /// Provides the method needed for special meta elements
    /// </summary>
    public class MetaWriter
    {
        public static void Write(BinaryWriter writer, string value)
        {
            writer.Write(value.Length);
            var stringBytes = Encoding.Unicode.GetBytes(value);            
            writer.Write(stringBytes);
        }
        public static void Write(BinaryWriter writer, Guid value)
            => writer.Write(value.ToByteArray());
    }

    public class AssemblySerializerHelper
    {
        private readonly static Dictionary<Type, MethodInfo> mWriterMethods = null;
        private readonly static Dictionary<Type, Delegate> mCache = new();
        static AssemblySerializerHelper()
        {
            mWriterMethods = typeof(BinaryWriter).GetMethods()
                .Where(x => x.Name == nameof(BinaryWriter.Write))
                .Where(x => x.GetParameters().Length == 1)
                .ToDictionary(x => x.GetParameters().First().ParameterType, x => x);

            var overrideMethods = typeof(MetaWriter).GetMethods()
                                    .Where(x => x.Name == nameof(MetaWriter.Write))
                                    .Where(x => x.GetParameters().Length == 2);

            foreach (var method in overrideMethods)
                mWriterMethods[method.GetParameters()[1].ParameterType] = method;
        }
        private static Delegate GetSerializerWithoutLock(Type metaType)
        {
            if (!mCache.TryGetValue(metaType, out var serializer))
                mCache[metaType] = serializer = GenerateSerializerFor(metaType);
            return serializer;
        }
        private static Delegate GenerateSerializerFor(Type metaType)
        {
            DynamicMethod method = new(
                metaType.FullName,
                MethodAttributes.Static | MethodAttributes.Public,
                CallingConventions.Standard,
                null,
                new Type[] { typeof(BinaryWriter), metaType },
                typeof(AssemblySerializerHelper).Module,
                true);
            var il = method.GetILGenerator();
            
            foreach (var field in metaType.GetFields(BindingFlags.Instance | BindingFlags.Public))
                EmitStore(il, field.FieldType, () => EmitLoadField(il, field));

            il.Emit(OpCodes.Ret);
            return method.CreateDelegate(typeof(Action<,>).MakeGenericType(typeof(BinaryWriter), metaType));
        }
        private static void EmitLoadField(ILGenerator il, FieldInfo field)
        {
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldfld, field);
        }
        private static void EmitStoreBuiltIn(ILGenerator il, MethodInfo method, Action loadPrimitive)
        {
            il.Emit(OpCodes.Ldarg_0);
            loadPrimitive();
            il.Emit(OpCodes.Call, method);
        }
        private static void EmitIf(ILGenerator il, Action emitCompare, Action emitTrueAction, Action emitFalseAction)
        {
            var falseLable = il.DefineLabel();
            var endLable = il.DefineLabel();

            emitCompare();
            il.Emit(OpCodes.Brfalse_S, falseLable);
            emitTrueAction();
            il.Emit(OpCodes.Br_S, endLable);
            il.MarkLabel(falseLable);
            emitFalseAction();
            il.MarkLabel(endLable);
        }
        private static void EmitStoreArray(ILGenerator il, LocalBuilder array, Action<LocalBuilder> bodyEmit)
        {
            //Store guard
            EmitIf(il,
                () =>
                {
                    il.Emit(OpCodes.Ldloc, array);
                    il.Emit(OpCodes.Ldnull);
                    il.Emit(OpCodes.Ceq);
                },
                () => 
                {
                    //Store zero length
                    EmitStore(il, typeof(int), () => il.Emit(OpCodes.Ldc_I4_0));
                },
                () => 
                {
                    //Store length
                    EmitStore(il, typeof(int), () =>
                    {
                        il.Emit(OpCodes.Ldloc, array);
                        il.Emit(OpCodes.Ldlen);
                        il.Emit(OpCodes.Conv_I4);
                    });

                    //Store content
                    var compareLabel = il.DefineLabel();
                    var bodyLabel = il.DefineLabel();
                    var index = il.DeclareLocal(typeof(int));
                    //i = 0
                    il.Emit(OpCodes.Ldc_I4_0);
                    il.Emit(OpCodes.Stloc, index);
                    il.Emit(OpCodes.Br_S, compareLabel);
                    il.MarkLabel(bodyLabel);

                    //bodyEmit(index);

                    //i++
                    il.Emit(OpCodes.Ldloc, index);
                    il.Emit(OpCodes.Ldc_I4_1);
                    il.Emit(OpCodes.Add);
                    il.Emit(OpCodes.Stloc, index);

                    //i < array.Length
                    il.MarkLabel(compareLabel);
                    il.Emit(OpCodes.Ldloc, index);
                    il.Emit(OpCodes.Ldloc, array);
                    il.Emit(OpCodes.Ldlen);
                    il.Emit(OpCodes.Conv_I4);
                    il.Emit(OpCodes.Blt_S, bodyLabel);
                });
        }
        private static void EmitStore(ILGenerator il, Type metaType, Action emitValueLoading)
        {
            if (mWriterMethods.TryGetValue(metaType, out var writer))
                EmitStoreBuiltIn(il, writer, emitValueLoading);
            else
            {
                //Complex type
                if (metaType.IsArray)
                {
                    var array = il.DeclareLocal(metaType);
                    emitValueLoading();
                    il.Emit(OpCodes.Stloc, array);

                    var elementType = metaType.GetElementType();
                    EmitStoreArray(il, array,
                        (index) => EmitStore(il, elementType, () =>
                        {
                            il.Emit(OpCodes.Ldloc, array);
                            il.Emit(OpCodes.Ldloc, index);
                            il.Emit(OpCodes.Ldelem, elementType);
                        }));
                }
                else if (metaType.IsEnum)
                {
                    var underlyingType = metaType.GetEnumUnderlyingType();
                    if (!mWriterMethods.TryGetValue(underlyingType, out writer))
                        throw new MetaElementNotSupportedException($"Type - {underlyingType.FullName} is not supported");
                    EmitStoreBuiltIn(il, writer, emitValueLoading);
                }
                else
                {
                    var targetMethod = GetSerializerWithoutLock(metaType);
                    il.Emit(OpCodes.Ldarg_0);
                    emitValueLoading();
                    il.Emit(OpCodes.Call, targetMethod.Method);
                }
            }
        }
        public static Delegate GetSerializer(Type metaType)
        {
            lock (mCache)
                return GetSerializerWithoutLock(metaType);
        }
    }
    public class AssemblySerializer
    {
        private readonly Stream mOutputStream;
        private readonly BinaryWriter mWriter;
        private readonly IAssemblyBuilder mBuilder;
        private void RelocateFixUp(long position, Action action)
        {
            long restoreLocation = mOutputStream.Position;
            mOutputStream.Seek(position, SeekOrigin.Begin);
            action();
            mOutputStream.Seek(restoreLocation, SeekOrigin.Begin);
        }
        private void Write<T>(T target)
        {
            var serializer = AssemblySerializerHelper.GetSerializer(typeof(T))
                as Action<BinaryWriter, T>;
            serializer(mWriter, target);
        }
        private void Write(object target)
        {
            var serializer = AssemblySerializerHelper.GetSerializer(target.GetType());
            serializer.DynamicInvoke(mWriter, target);
        }
        private void WriteReferenceTable(ReferenceTable table, int offsetOfContentOffset)
        {
            int baseOffset = (int)mOutputStream.Position;
            foreach (var meta in table.ReferenceTokenMetas)
                Write(meta);

            RelocateFixUp(offsetOfContentOffset, () => mWriter.Write(baseOffset));
        }
        private int WriteReferenceTableHead(ReferenceTable table)
        {
            int current = (int)mOutputStream.Position;
            mWriter.Write(int.MinValue);
            mWriter.Write(table.ReferenceTokenMetas.Count);
            return current;
        }
        private void WriteDefinitionTable(DefinitionTable table, int offsetOfRecordOffsets)
        {
            int[] offsets = new int[table.DefinitionTokenMetas.Count];
            for (int i = 0; i < table.DefinitionTokenMetas.Count; i++)
            {
                offsets[i] = (int)mOutputStream.Position;
                Write(table.DefinitionTokenMetas[i]);
            }

            RelocateFixUp(offsetOfRecordOffsets, () =>
            {
                for (int i = 0; i < offsets.Length; i++)
                    mWriter.Write(offsets[i]);
            });
        }
        private int WriteDefinitionTableHead(DefinitionTable table)
        {
            mWriter.Write((short)table.Kind);
            mWriter.Write(table.DefinitionTokenMetas.Count);
            int current = (int)mOutputStream.Position;

            for (int i = 0; i < table.DefinitionTokenMetas.Count; i++)
                mWriter.Write(int.MinValue);

            return current;
        }
        private int WriteStringTableHead(StringTable table)
        {
            mWriter.Write((short)table.Kind);
            mWriter.Write(table.Contents.Count);
            int current = (int)mOutputStream.Position;

            for (int i = 0; i < table.Contents.Count; i++)
                mWriter.Write(int.MinValue);

            return current;
        }
        private void WriteStringTable(StringTable table, int offsetOfRecordOffsets)
        {
            int[] offsets = new int[table.Contents.Count];
            for (int i = 0; i < table.Contents.Count; i++)
            {
                offsets[i] = (int)mOutputStream.Position;
                MetaWriter.Write(mWriter, table.Contents[i]);
            }

            RelocateFixUp(offsetOfRecordOffsets, () =>
            {
                for (int i = 0; i < offsets.Length; i++)
                    mWriter.Write(offsets[i]);
            });
        }
        public AssemblySerializer(Stream output, IAssemblyBuilder builder)
        {
            mOutputStream = output;
            mBuilder = builder;
            mWriter = new BinaryWriter(mOutputStream);
        }
        public void Serialize()
        {
            Write(mBuilder.AssemblyHeader);

            {
                var typeRef = mBuilder.ReferenceTables[MDRecordKinds.TypeRef];
                int typeRefRelocate = WriteReferenceTableHead(typeRef);

                var memberRef = mBuilder.ReferenceTables[MDRecordKinds.FieldRef];
                int memberRefRelocate = WriteReferenceTableHead(memberRef);

                var assemblyRef = mBuilder.AssemblyReferenceTable;
                var assemblyRefRelocate = WriteReferenceTableHead(assemblyRef);

                {
                    List<int> offsetTable = new();
                    offsetTable.Add(WriteStringTableHead(mBuilder.MetaStringTable));
                    for (int i = (int)MDRecordKinds.Argument; i < (int)MDRecordKinds.KindLimit; ++i)
                        offsetTable.Add(WriteDefinitionTableHead(mBuilder.DefinitionTables[(MDRecordKinds)i]));


                    WriteStringTable(mBuilder.MetaStringTable, offsetTable[0]);
                    for (int i = (int)MDRecordKinds.Argument; i < (int)MDRecordKinds.KindLimit; ++i)
                        WriteDefinitionTable(mBuilder.DefinitionTables[(MDRecordKinds)i], offsetTable[i]);
                }

                WriteReferenceTable(typeRef, typeRefRelocate);
                WriteReferenceTable(memberRef, memberRefRelocate);
                WriteReferenceTable(assemblyRef, assemblyRefRelocate);
            }
        }
    }
}
