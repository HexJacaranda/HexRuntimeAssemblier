using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexRuntimeAssemblier
{
    public class AssemblyBuildConfiguration
    {
        public string[] Inputs { get; set; }
        public string[] References { get; set; }
        public string OutputDirectory { get; set; }
    }
}
