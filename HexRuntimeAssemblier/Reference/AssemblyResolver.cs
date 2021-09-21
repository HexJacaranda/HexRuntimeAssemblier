using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using HexRuntimeAssemblier.Interfaces;
using HexRuntimeAssemblier.Meta;
using HexRuntimeAssemblier.Serialization;

namespace HexRuntimeAssemblier.Reference
{
    public class AssemblyResolver : IAssemblyResolver
    {
        private AssemblyHeaderMD mHeader;
        private Dictionary<MDRecordKinds, Dictionary<string, uint>> mQueryTable = new();
        protected AssemblyResolver()
        {

        }
        protected class AssemblyQueryBuilder
        {
            private readonly Stream mStream;
            private readonly BinaryReader mReader;
            private string[] mStringTable;
            public AssemblyQueryBuilder(Stream stream)
            {
                mStream = stream;
                mReader = new BinaryReader(mStream);
            }
            private void SkipReferenceTableHead(MDRecordKinds _)
            {
                mReader.ReadInt32();
                mReader.ReadInt32();
            }
            private MDRecordKinds PreReadDefinitionTableHead()
                => (MDRecordKinds)mReader.ReadInt16();
            private void SkipDefinitionTableHead()
            {
                int count = mReader.ReadInt32();
                if (count < 0)
                    throw new BadAssemblyException("Assembly may be corruptted or incompatible");
                mStream.Seek(count * sizeof(uint), SeekOrigin.Current);
            }
            /// <summary>
            /// Set to offset location, execute the action and get back
            /// </summary>
            /// <param name="offset"></param>
            /// <param name="fetch"></param>
            private void FetchRoute(long offset, Action fetch)
            {
                long origin = mStream.Position;
                mStream.Position = offset;
                fetch();
                mStream.Position = origin;
            }
            private Dictionary<string, uint> ReadDefinitionTable<T>(Func<T, uint> tokenGetter)
            {
                int count = mReader.ReadInt32();
                if (count < 0)
                    throw new BadAssemblyException("Assembly may be corruptted or incompatible");

                Dictionary<string, uint> fullyQualifiedNamesMapping = new();
                int[] offsets = new int[count];
                for (int i = 0; i < count; i++)
                    offsets[i] = mReader.ReadInt32();

                var deserializer = AssemblySerializerHelper.GetDeserializer(typeof(T)) as Func<BinaryReader, T>;
                FetchRoute(mStream.Position, () =>
                {                   
                    for (int i = 0; i < count; i++)
                    {
                        FetchRoute(offsets[i], () =>
                        {
                            var meta = deserializer(mReader);
                            //Get token of FQN
                            uint token = tokenGetter(meta);
                            //Set to mapping table
                            string FQN = mStringTable[(int)token];
                            fullyQualifiedNamesMapping[FQN] = (uint)i;
                        });
                    }
                });

                return fullyQualifiedNamesMapping;
            }
            private string[] ReadStringTable()
            {
                //Record
                long restoreLocation = mStream.Position;

                for (int i = (int)MDRecordKinds.String; i < (int)MDRecordKinds.KindLimit; ++i)
                {
                    if (PreReadDefinitionTableHead() == MDRecordKinds.String)
                        goto LOAD_STRING;
                    else
                        SkipDefinitionTableHead();
                }
                throw new BadAssemblyException("Assembly doesn't contain string table");

                LOAD_STRING:

                int count = mReader.ReadInt32();
                if (count < 0)
                    throw new BadAssemblyException("Assembly may be corruptted or incompatible");

                int[] offsets = new int[count];
                for (int i = 0; i < count; i++)
                    offsets[i] = mReader.ReadInt32();

                string[] ret = new string[count];
                for (int i = 0; i < count; i++)
                {
                    FetchRoute(offsets[i], () =>
                    {
                        int length = mReader.ReadInt32();
                        byte[] unicodeBytes = mReader.ReadBytes(2 * length);
                        ret[i] = Encoding.Unicode.GetString(unicodeBytes);
                    });
                }

                //Restore to the beginning for full resolving
                mStream.Position = restoreLocation;
                return ret;
            }
            public AssemblyResolver Build()
            {
                AssemblyResolver resolver = new();
                {
                    var headerReader = AssemblySerializerHelper.GetDeserializer(typeof(AssemblyHeaderMD)) as Func<BinaryReader, AssemblyHeaderMD>;
                    resolver.mHeader = headerReader(mReader);
                }
                {
                    //Hard Coded skipping
                    SkipReferenceTableHead(MDRecordKinds.TypeRef);
                    //Actually member reference
                    SkipReferenceTableHead(MDRecordKinds.FieldRef);
                    //Assembly reference
                    SkipReferenceTableHead(MDRecordKinds.KindLimit);
                }

                //Actually, the order of def table heads is not fixed, they can be arranged in any order
                //So the first thing is to find the string table
                mStringTable = ReadStringTable();

                {
                    //For exporting, we only need TypeDef, MethodDef and FieldDef
                    for (int i = (int)MDRecordKinds.String; i < (int)MDRecordKinds.KindLimit; ++i)
                    {
                        MDRecordKinds kind = PreReadDefinitionTableHead();
                        switch (kind)
                        {
                            case MDRecordKinds.TypeDef:
                                resolver.mQueryTable[MDRecordKinds.TypeDef] = ReadDefinitionTable<TypeMD>(x => x.FullyQualifiedNameToken);
                                break;
                            case MDRecordKinds.MethodDef:
                                resolver.mQueryTable[MDRecordKinds.MethodDef] = ReadDefinitionTable<MethodMD>(x => x.FullyQualifiedNameToken);
                                break;
                            case MDRecordKinds.FieldDef:
                                resolver.mQueryTable[MDRecordKinds.FieldDef] = ReadDefinitionTable<FieldMD>(x => x.FullyQualifiedNameToken);
                                break;
                            default:
                                SkipDefinitionTableHead();
                                break;
                        }
                    }
                }

                //Don't forget to set the necessary info
                resolver.AssemblyName = mStringTable[resolver.mHeader.NameToken];
                resolver.AssemblyGuid = resolver.mHeader.GUID;

                return resolver;
            }
        }
        public static AssemblyResolver Build(Stream assemblyStream)
            => new AssemblyQueryBuilder(assemblyStream).Build();

        public uint QueryFieldDefinition(string fullyQualifiedName)
            => mQueryTable[MDRecordKinds.FieldDef][fullyQualifiedName];

        public uint QueryMethodDefinition(string fullyQualifiedName)
            => mQueryTable[MDRecordKinds.MethodDef][fullyQualifiedName];

        public uint QueryTypeDefinition(string fullyQualifiedName)
             => mQueryTable[MDRecordKinds.TypeDef][fullyQualifiedName];

        public string AssemblyName { get; private set; }

        public Guid AssemblyGuid { get; private set; }
    }
}
