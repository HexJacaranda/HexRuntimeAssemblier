using HexRuntimeAssemblier;
using HexRuntimeAssemblier.Interfaces;
using HexRuntimeAssemblier.Meta;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.IO;

namespace AssemblierTest
{
    public class BuilderTest
    {
        private readonly static ILoggerFactory Factory = LoggerFactory.Create(x => x.AddConsole());
        public static IAssemblyBuilder Build(string name,
            bool disableCoreType = true,
            bool coreLibrary = false)
        {
            IAssemblyBuilder builder = new AssemblyBuilder(
                CoreAssemblyConstant.Default,
                new AssemblyOptions()
                {
                    CoreLibrary = coreLibrary,
                    DisableCoreType = disableCoreType
                },
                Factory.CreateLogger<AssemblyBuilder>(),
                Array.Empty<IAssemblyResolver>());
            
            builder.Build(File.OpenRead(@$"..\..\..\TestIL\{name}.il"));
            return builder;
        }

        [Test]
        public void TestAssemblyHeader()
        {
            var builder = Build(nameof(TestAssemblyHeader));
            var header = builder.AssemblyHeader;

            Assert.That(header.GUID, Is.EqualTo(Guid.Empty));
            Assert.That(header.MajorVersion, Is.EqualTo(1));
            Assert.That(header.MinorVersion, Is.EqualTo(0));
            Assert.That(header.GroupNameToken.GetString(builder), Is.EqualTo("HexRT"));
            Assert.That(header.NameToken.GetString(builder), Is.EqualTo("Core"));
        }

        [Test]
        public void TestClass()
        {
            var builder = Build(nameof(TestClass));
            var type = builder.GetTypeDef("[Core][Test]Hello");

            var interfaceType = builder.GetTypeDef("[Core][Test]IA");
            Assert.IsTrue(interfaceType.Flags.HasFlag(TypeFlag.Interface));

            var structType = builder.GetTypeDef("[Core][Test]SA");
            Assert.IsTrue(structType.Flags.HasFlag(TypeFlag.Struct));

            var nestType = builder.GetTypeDef("[Core][Test]Hello.World");
            Assert.IsTrue(nestType.Flags.HasFlag(TypeFlag.Nested));
        }

        [Test]
        public void TestMethod()
        {
            var builder = Build(nameof(TestMethod));
            var method = builder.GetMethodDef("[Core][Test]Hello::.ctor()");

            Assert.IsFalse(method.Flags.HasFlag(MethodFlag.Static));
            Assert.That(method.FullyQualifiedNameToken.GetString(builder), Is.EqualTo("[Core][Test]Hello::.ctor()"));
            Assert.That(method.ParentTypeRefToken, Is.EqualTo(builder.GetTypeRefToken("[Core][Test]Hello")));
        }

        [Test]
        public void TestProperty()
        {
            var builder = Build(nameof(TestProperty));
            var property = builder.GetPropertyDef("[Core][Test]Hello::X");

            Assert.That(property.ParentTypeRefToken, Is.EqualTo(builder.GetTypeRefToken("[Core][Test]Hello")));

            Assert.That(property.SetterToken, Is.EqualTo(builder.GetMethodRefToken("[Core][Test]Hello::setX([Core][global]Int32)")));
            Assert.That(property.GetterToken, Is.EqualTo(builder.GetMethodRefToken("[Core][Test]Hello::getX()")));
        }

        [Test]
        public void TestField()
        {
            var builder = Build(nameof(TestField));
            var field = builder.GetFieldDef("[Core][Test]Hello::X");

            Assert.That(field.ParentTypeRefToken, Is.EqualTo(builder.GetTypeRefToken("[Core][Test]Hello")));

            Assert.True(field.Flags.HasFlag(FieldFlag.ThreadLocal));
            Assert.True(field.Flags.HasFlag(FieldFlag.Volatile));
            Assert.False(field.Flags.HasFlag(FieldFlag.Static));
        }

        [Test]
        public void TestGenericClass()
        {
            var builder = Build(nameof(TestGenericClass));

            Assert.DoesNotThrow(() => builder.GetTypeDef("[Core][Test]Hello<Canon>"));
            Assert.DoesNotThrow(() => builder.GetTypeDef("[Core][Test]Hello<Canon>.World"));
            Assert.DoesNotThrow(() => builder.GetTypeDef("[Core][Test]Hello<Canon>.World.This<Canon, Canon>"));

            var field = builder.GetFieldDef("[Core][Test]Hello<Canon>::X");
            Assert.That(field.ParentTypeRefToken, Is.EqualTo(builder.GetTypeRefToken("[Core][Test]Hello<Canon>")));
            Assert.That(field.TypeRefToken, Is.EqualTo(builder.GetTypeRefToken("[Core][Test]Hello<!T1>.World.This<!T1, [Core][global]Int32>")));
        }

        [Test]
        public void TestGenericMethod()
        {
            var builder = Build(nameof(TestGenericMethod));

            Assert.DoesNotThrow(() => builder.GetMethodDef("[Core][Test]Hello<Canon>::A<Canon>()"));
            Assert.DoesNotThrow(() => builder.GetMethodDef("[Core][Test]Hello<Canon>::B()"));
        }

        [Test]
        public void TestCoreLib()
        {
            Assert.DoesNotThrow(() => Build(nameof(TestCoreLib), false, true));
        }
    }
}