using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HexRuntimeAssemblier;
using NUnit.Framework;

namespace AssemblierTest
{
    public class AssemblierTest
    {
        [Test, Order(1)]
        public void TestCompilingCoreLibrary()
        {
            Assert.DoesNotThrow(() => StartUp.Main(new string[] { @"..\..\..\TestBuild\BuildCore.json" }));
        }

        [Test, Order(2)]
        public void TestReferenceLibrary()
        {
            Assert.DoesNotThrow(() => StartUp.Main(new string[] { @"..\..\..\TestBuild\Reference.json" }));
        }

        [Test, Order(3)]
        public void TestJITLibrary()
        {
            Assert.DoesNotThrow(() => StartUp.Main(new string[] { @"..\..\..\TestBuild\JIT.json" }));
        }
    }
}
