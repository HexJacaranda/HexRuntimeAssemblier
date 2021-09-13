using HexRuntimeAssemblier.Interfaces;
using HexRuntimeAssemblier.Meta;
using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;

namespace HexRuntimeAssemblier.Serialization
{
    public class AssemblySerializerHelper
    {
        private static Dictionary<Type, MethodInfo> mWriterMethods = new();
        private static Dictionary<Type, Delegate> mCache = new();
        public static Delegate GetSerializer(Type metaType)
        {
            lock (mCache)
                return GetSerializerWithoutLock(metaType);
        }
        private static Delegate GetSerializerWithoutLock(Type metaType)
        {
            if (!mCache.TryGetValue(metaType, out var serializer))
                mCache[metaType] = GenerateSerializerFor(metaType);
            return serializer;
        }
        private static Delegate GenerateSerializerFor(Type metaType)
        {
            DynamicMethod method = new(metaType.FullName, null, new Type[] { typeof(BinaryWriter), metaType });
            var il = method.GetILGenerator();
            
            var fields = metaType.GetFields();
            foreach (var field in fields)
            {
                var type = field.FieldType;
                if (type.IsPrimitive)
                {
                    EmitStorePrimitive(il, type, () =>
                    {
                        il.Emit(OpCodes.Ldarg_1);
                        il.Emit(OpCodes.Ldfld, field);
                    });
                }
                else if (type ==  typeof(string))
                {

                }
                else if (type.IsArray)
                {
                    il.BeginScope();

                    var array = il.DeclareLocal(type);
                    il.Emit(OpCodes.Ldarg_1);
                    il.Emit(OpCodes.Ldfld, field);
                    il.Emit(OpCodes.Stloc, array);

                    var elementType = type.GetElementType();
                    if (elementType.IsPrimitive)
                    {
                        EmitStoreArray(il, array, (index) =>
                        {
                            EmitStorePrimitive(il, elementType, () =>
                            {
                                il.Emit(OpCodes.Ldloc, array);
                                il.Emit(OpCodes.Ldloc, index);
                                il.Emit(OpCodes.Ldelem);
                            });
                        });
                    }
                    else
                    {
                        var targetMethod = GetSerializerWithoutLock(type.GetElementType());
                        EmitStoreArray(il, array, (index) =>
                        {
                            il.Emit(OpCodes.Ldarg_0);
                            il.Emit(OpCodes.Ldloc, array);
                            il.Emit(OpCodes.Ldloc, index);
                            il.Emit(OpCodes.Ldelem);
                            il.Emit(OpCodes.Call, targetMethod.Method);
                        });
                    }

                    il.EndScope();
                }
                else
                {
                    var targetMethod = GetSerializerWithoutLock(type.GetElementType());
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Ldarg_1);
                    il.Emit(OpCodes.Ldfld, field);
                    il.Emit(OpCodes.Call, targetMethod.Method);
                }
            }
            il.Emit(OpCodes.Ret);
            return method.CreateDelegate(typeof(Action<>).MakeGenericType(typeof(BinaryWriter), metaType));
        }
        private static void EmitStorePrimitive(ILGenerator il, Type primitiveType, Action loadPrimitive)
        {
            var targetMethod = mWriterMethods[primitiveType];
            il.Emit(OpCodes.Ldarg_0);
            loadPrimitive();
            il.Emit(OpCodes.Call, targetMethod);
        }
        private static void EmitStoreArray(ILGenerator il, LocalBuilder array, Action<LocalBuilder> bodyEmit)
        {
            //Store length
            EmitStorePrimitive(il, typeof(int), () =>
             {
                 il.Emit(OpCodes.Ldloc, array);
                 il.Emit(OpCodes.Ldlen);
                 il.Emit(OpCodes.Conv_I4);
             });

            //Store content
            var compareLabel = il.DefineLabel();
            var bodyLabel = il.DefineLabel();
            var index = il.DeclareLocal(typeof(int));

            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Stloc, index);
            il.Emit(OpCodes.Br_S, compareLabel);
            il.MarkLabel(bodyLabel);

            bodyEmit(index);

            il.MarkLabel(compareLabel);
            il.Emit(OpCodes.Ldloc, index);
            il.Emit(OpCodes.Ldloc, array);
            il.Emit(OpCodes.Ldlen);
            il.Emit(OpCodes.Conv_I4);
            il.Emit(OpCodes.Clt);
            il.Emit(OpCodes.Brtrue_S, bodyLabel);
        }
        private static void WriteString(BinaryWriter writer, string value)
        {
            var bytes = value.AsSpan();
            writer.Write(bytes)
        }
    }
    public class AssemblySerializer
    {
        private readonly Stream mOutputStream;
        private readonly BinaryWriter mWriter;
        private readonly IAssemblyBuilder mBuilder;
        private void RelocateFixUp(long position, Action action)
        {
            long restoreLocation = mOutputStream.Length;
            mOutputStream.Seek(position, SeekOrigin.Begin);
            action();
            mOutputStream.Seek(restoreLocation, SeekOrigin.Begin);
        }
        private void Write<T>(T target)
        {
            var serializer = AssemblySerializerHelper.GetSerializer(typeof(AssemblyHeaderMD))
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
            int baseOffset = (int)mOutputStream.Length;
            foreach (var meta in table.ReferenceTokenMetas)
                Write(meta);

            RelocateFixUp(offsetOfContentOffset, () => mWriter.Write(baseOffset));
        }
        private int WriteReferenceTableHead(ReferenceTable table)
        {
            mWriter.Write(table.ReferenceTokenMetas.Count);
            int current = (int)mOutputStream.Length;
            mWriter.Write(int.MinValue);
            return current;
        }
        private void WriteDefinitionTable(DefinitionTable table, int offsetOfRecordOffsets)
        {
            int[] offsets = new int[table.DefinitionTokenMetas.Count];
            for (int i = 0; i < table.DefinitionTokenMetas.Count; i++)
            {
                offsets[i] = (int)mOutputStream.Length;
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
            mWriter.Write((int)table.Kind);
            mWriter.Write(table.DefinitionTokenMetas.Count);
            int current = (int)mOutputStream.Length;

            for (int i = 0; i < table.DefinitionTokenMetas.Count; i++)
                mWriter.Write(int.MinValue);

            return current;
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

                WriteReferenceTable(typeRef, typeRefRelocate);
                WriteReferenceTable(memberRef, memberRefRelocate);
                WriteReferenceTable(assemblyRef, assemblyRefRelocate);
            }

            {

            }
        }
    }
}
