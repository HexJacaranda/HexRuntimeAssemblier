using System;
using System.Linq;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace ILAssemblierParserCodeGenerator
{
    public class OpCodeJson
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
    public class EnumGenerator
    {
        private readonly OpCodeJson[] mOps;
        private const string EnumSignatre = @"namespace HexRuntimeAssemblier.IL
        {{
            enum OpCode : byte
            {{
                {0}
            }}
        }}";
        public EnumGenerator(OpCodeJson[] ops)
        {
            mOps = ops;
        }
        public string Generate()
        {
            string member = string.Join($",{Environment.NewLine}", mOps.Select(x => $"{x.Name} = {x.Value}"));
            return string.Format(EnumSignatre, member);
        }
    }
    public class ParserGenerator
    {
        private const string ParseILMethodSignature = @"private void ParseIL(Assemblier.IlInstructionContext[] ils)
        {{
            {0}
        }}";
        private const string ParseILCase = @"case Assemblier.Op{0}Context @{1}:Parse{0}(@{1});break;";
        private const string ParseILBody = @"foreach(var il in ils)
        {{
            switch(il.GetUnderlyingType())
            {{
                {0}
            }}
        }}";
        private const string ParseOpCode = @"private void Parse{0}(Assemblier.Op{0}Context context)
        {{
            mILWriter.Write((byte)OpCode.{0});
        }}";
        private const string ParserDeclaration = @"namespace HexRuntimeAssemblier.IL
        {{
            partial class ILAssemblier
            {{
                {0}
                {1}
            }}
        }}";

        private readonly OpCodeJson[] mOps;
        public ParserGenerator(OpCodeJson[] ops)
        {
            mOps = ops;
        }
        private string GenerateSwitchCase()
        {
            var caseBody = string.Join(Environment.NewLine, mOps.Select(x => string.Format(ParseILCase, x.Name, x.Name.ToLower())));
            var parseILMethodBody = string.Format(ParseILBody, caseBody);
            return string.Format(ParseILMethodSignature, parseILMethodBody);
        }
        public string Generate()
        {
            string parseOpMethods = string.Join(Environment.NewLine, mOps.Select(x => string.Format(ParseOpCode, x.Name)));
            return string.Format(ParserDeclaration, GenerateSwitchCase(), parseOpMethods);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            string inputFile = args[0];
            string outputDirectory = args[1];
            var inputs = JsonSerializer.Deserialize<OpCodeJson[]>(File.ReadAllText(inputFile));

            string enumCSFile = new EnumGenerator(inputs).Generate();
            File.WriteAllText(Path.Combine(outputDirectory, "Opcode.cs"), enumCSFile);

            string parserCSFile = new ParserGenerator(inputs).Generate();
            File.WriteAllText(Path.Combine(outputDirectory, "ILAssemblierGenerated.cs"), parserCSFile);
        }
    }
}
