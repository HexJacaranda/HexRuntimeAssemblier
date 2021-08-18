using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HexRuntimeAssemblier
{

    [Serializable]
    public class ReferenceTargetNotFoundException : Exception
    {
        public ReferenceTargetNotFoundException() { }
        public ReferenceTargetNotFoundException(string message) : base(message) { }
        public ReferenceTargetNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected ReferenceTargetNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
