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


    [Serializable]
    public class BadModifierException : Exception
    {
        public BadModifierException() { }
        public BadModifierException(string message) : base(message) { }
        public BadModifierException(string message, Exception inner) : base(message, inner) { }
        protected BadModifierException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class BadDeclarationLevelException : Exception
    {
        public BadDeclarationLevelException() { }
        public BadDeclarationLevelException(string message) : base(message) { }
        public BadDeclarationLevelException(string message, Exception inner) : base(message, inner) { }
        protected BadDeclarationLevelException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class TypeResolveException : Exception
    {
        public TypeResolveException() { }
        public TypeResolveException(string message) : base(message) { }
        public TypeResolveException(string message, Exception inner) : base(message, inner) { }
        protected TypeResolveException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }


    [Serializable]
    public class UnknownTokenException : Exception
    {
        public UnknownTokenException() { }
        public UnknownTokenException(string message) : base(message) { }
        public UnknownTokenException(string message, Exception inner) : base(message, inner) { }
        protected UnknownTokenException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class DuplicateLabelException : Exception
    {
        public DuplicateLabelException() { }
        public DuplicateLabelException(string message) : base(message) { }
        public DuplicateLabelException(string message, Exception inner) : base(message, inner) { }
        protected DuplicateLabelException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class UnexpectedParseRuleException : Exception
    {
        public UnexpectedParseRuleException() { }
        public UnexpectedParseRuleException(string message) : base(message) { }
        public UnexpectedParseRuleException(string message, Exception inner) : base(message, inner) { }
        protected UnexpectedParseRuleException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class SymbolNotFoundException : Exception
    {
        public SymbolNotFoundException() { }
        public SymbolNotFoundException(string message) : base(message) { }
        public SymbolNotFoundException(string message, Exception inner) : base(message, inner) { }
        protected SymbolNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
