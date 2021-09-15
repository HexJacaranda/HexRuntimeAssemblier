﻿using HexRuntimeAssemblier.Interfaces;
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

    public class MetaReader
    {
        public static string ReadString(BinaryReader reader)
        {
            int length = reader.ReadInt32();
            byte[] bytes = reader.ReadBytes(length);
            return Encoding.Unicode.GetString(bytes);
        }
    }

    public class AssemblySerializerHelper
    {
        private readonly static Dictionary<Type, MethodInfo> mWriterMethods = null;
        private readonly static Dictionary<Type, Delegate> mSerializerCache = new();
        private readonly static Dictionary<Type, MethodInfo> mReaderMethods = null;
        private readonly static Dictionary<Type, Delegate> mDeserializerCache = new();
        private readonly static List<Type> mPrimitives = new()
        {
            typeof(bool),
            typeof(byte),
            typeof(char),
            typeof(short),
            typeof(int),
            typeof(long),
            typeof(float),
            typeof(double),
            typeof(string),
            typeof(ushort),
            typeof(uint),
            typeof(ulong)
        };
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

            mReaderMethods = mPrimitives.ToDictionary(x => x, x => typeof(BinaryWriter).GetMethod($"Read{x.Name}"));
            foreach (var method in typeof(MetaWriter).GetMethods()
                                    .Where(x => x.Name.StartsWith("Read"))
                                    .Where(x => x.GetParameters().Length == 1))
                mReaderMethods[method.ReturnType] = method;
        }
        #region Serialize
        private static Delegate GetSerializerWithoutLock(Type metaType)
        {
            if (!mSerializerCache.TryGetValue(metaType, out var serializer))
                mSerializerCache[metaType] = serializer = GenerateSerializerFor(metaType);
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

                    bodyEmit(index);

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
            lock (mSerializerCache)
                return GetSerializerWithoutLock(metaType);
        }
        #endregion
        #region Deserialize
        private static Delegate GetDeserializerWithoutLock(Type metaType)
        {
            if (!mDeserializerCache.TryGetValue(metaType, out var deserializer))
                mDeserializerCache[metaType] = deserializer = GenerateSerializerFor(metaType);
            return deserializer;
        }
        private static Delegate GenerateDeserializerFor(Type metaType)
        {
            DynamicMethod method = new(
                $"Deserialize{metaType.FullName}",
                MethodAttributes.Static | MethodAttributes.Public,
                CallingConventions.Standard,
                metaType,
                new Type[] { typeof(BinaryReader) },
                typeof(AssemblySerializerHelper).Module,
                true);

            var il = method.GetILGenerator();

            var returnObject = il.DeclareLocal(metaType);
            il.Emit(OpCodes.Newobj, metaType.GetConstructor(Array.Empty<Type>()));
            il.Emit(OpCodes.Stloc, returnObject);

            foreach (var field in metaType.GetFields(BindingFlags.Instance | BindingFlags.Public))
                EmitLoad(il, field.FieldType, () => { });

            il.Emit(OpCodes.Ldloc, returnObject);
            il.Emit(OpCodes.Ret);

            return method.CreateDelegate(typeof(Func<,>).MakeGenericType(typeof(BinaryReader), metaType));
        }
        private static void EmitLoadArray(ILGenerator il, Type elementType, LocalBuilder array, Action<LocalBuilder> bodyEmit)
        {
            var count = il.DeclareLocal(typeof(int));
            EmitLoad(il, typeof(int), () =>
            {
                il.Emit(OpCodes.Dup);
                il.Emit(OpCodes.Stloc, count);

                EmitIf(il,
                    () =>
                    {
                        il.Emit(OpCodes.Ldc_I4_0);
                        il.Emit(OpCodes.Cgt);
                    },
                    () =>
                    {
                        il.Emit(OpCodes.Ldloc, count);
                        il.Emit(OpCodes.Newarr, elementType);
                        il.Emit(OpCodes.Stloc, array);

                        //Store content
                        var compareLabel = il.DefineLabel();
                        var bodyLabel = il.DefineLabel();
                        //i = 0
                        il.Emit(OpCodes.Ldc_I4_0);
                        il.Emit(OpCodes.Stloc, count);
                        il.Emit(OpCodes.Br_S, compareLabel);
                        il.MarkLabel(bodyLabel);

                        bodyEmit(count);

                        //i++
                        il.Emit(OpCodes.Ldloc, count);
                        il.Emit(OpCodes.Ldc_I4_1);
                        il.Emit(OpCodes.Add);
                        il.Emit(OpCodes.Stloc, count);

                        //i < array.Length
                        il.MarkLabel(compareLabel);
                        il.Emit(OpCodes.Ldloc, count);
                        il.Emit(OpCodes.Ldloc, array);
                        il.Emit(OpCodes.Ldlen);
                        il.Emit(OpCodes.Conv_I4);
                        il.Emit(OpCodes.Blt_S, bodyLabel);
                    },
                    () =>
                    {
                        il.Emit(OpCodes.Ldnull);
                    });
            });
        }
        private static void EmitLoad(ILGenerator il, Type metaType, Action emitSetter)
        {
            if (mWriterMethods.TryGetValue(metaType, out var reader))
            {
                il.Emit(OpCodes.Ldarg_0);
                il.Emit(OpCodes.Call, reader);
            }
            else
            {
                if (metaType.IsArray)
                {
                    var array = il.DeclareLocal(metaType);
                    var elementType = metaType.GetElementType();
                    EmitLoadArray(il, elementType, array,
                        (index) =>
                        {
                            il.Emit(OpCodes.Ldloc, array);
                            il.Emit(OpCodes.Ldloc, index);
                            EmitLoad(il, elementType, () => il.Emit(OpCodes.Stelem));
                        });
                }
                else if (metaType.IsEnum)
                {
                    var underlyingType = metaType.GetEnumUnderlyingType();
                    if (!mReaderMethods.TryGetValue(underlyingType, out reader))
                        throw new MetaElementNotSupportedException($"Type - {underlyingType.FullName} is not supported");
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Call, reader);
                }
                else
                {
                    var targetMethod = GetSerializerWithoutLock(metaType);
                    il.Emit(OpCodes.Ldarg_0);
                    il.Emit(OpCodes.Call, targetMethod.Method);
                }
            }

            emitSetter();
        }
        public static Delegate GetDeserializer(Type metaType)
        {
            lock (mDeserializerCache)
                return GetSerializerWithoutLock(metaType);
        }
        #endregion
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
