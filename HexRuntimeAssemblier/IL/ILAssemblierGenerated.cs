namespace HexRuntimeAssemblier.IL
{
    partial class ILAssemblier
    {
        private void ParseIL(Assemblier.IlInstructionContext[] ils)
        {
            foreach (var il in ils)
            {
                switch (il.GetUnderlyingType())
                {
                    case Assemblier.OpLdFldContext @ldfld: ParseLdFld(@ldfld); break;
                    case Assemblier.OpLdFldAContext @ldflda: ParseLdFldA(@ldflda); break;
                    case Assemblier.OpLdLocContext @ldloc: ParseLdLoc(@ldloc); break;
                    case Assemblier.OpLdLocAContext @ldloca: ParseLdLocA(@ldloca); break;
                    case Assemblier.OpLdArgContext @ldarg: ParseLdArg(@ldarg); break;
                    case Assemblier.OpLdArgAContext @ldarga: ParseLdArgA(@ldarga); break;
                    case Assemblier.OpLdElemContext @ldelem: ParseLdElem(@ldelem); break;
                    case Assemblier.OpLdElemAContext @ldelema: ParseLdElemA(@ldelema); break;
                    case Assemblier.OpLdStrContext @ldstr: ParseLdStr(@ldstr); break;
                    case Assemblier.OpLdCContext @ldc: ParseLdC(@ldc); break;
                    case Assemblier.OpLdFnContext @ldfn: ParseLdFn(@ldfn); break;
                    case Assemblier.OpLdNullContext @ldnull: ParseLdNull(@ldnull); break;
                    case Assemblier.OpStFldContext @stfld: ParseStFld(@stfld); break;
                    case Assemblier.OpStLocContext @stloc: ParseStLoc(@stloc); break;
                    case Assemblier.OpStArgContext @starg: ParseStArg(@starg); break;
                    case Assemblier.OpStElemContext @stelem: ParseStElem(@stelem); break;
                    case Assemblier.OpStTAContext @stta: ParseStTA(@stta); break;
                    case Assemblier.OpAddContext @add: ParseAdd(@add); break;
                    case Assemblier.OpSubContext @sub: ParseSub(@sub); break;
                    case Assemblier.OpMulContext @mul: ParseMul(@mul); break;
                    case Assemblier.OpDivContext @div: ParseDiv(@div); break;
                    case Assemblier.OpModContext @mod: ParseMod(@mod); break;
                    case Assemblier.OpAndContext @and: ParseAnd(@and); break;
                    case Assemblier.OpOrContext @or: ParseOr(@or); break;
                    case Assemblier.OpXorContext @xor: ParseXor(@xor); break;
                    case Assemblier.OpNotContext @not: ParseNot(@not); break;
                    case Assemblier.OpNegContext @neg: ParseNeg(@neg); break;
                    case Assemblier.OpConvContext @conv: ParseConv(@conv); break;
                    case Assemblier.OpCallContext @call: ParseCall(@call); break;
                    case Assemblier.OpCallVirtContext @callvirt: ParseCallVirt(@callvirt); break;
                    case Assemblier.OpRetContext @ret: ParseRet(@ret); break;
                    case Assemblier.OpCmpContext @cmp: ParseCmp(@cmp); break;
                    case Assemblier.OpJccContext @jcc: ParseJcc(@jcc); break;
                    case Assemblier.OpJmpContext @jmp: ParseJmp(@jmp); break;
                    case Assemblier.OpThrowContext @throw: ParseThrow(@throw); break;
                    case Assemblier.OpTryContext @try: ParseTry(@try); break;
                    case Assemblier.OpCatchContext @catch: ParseCatch(@catch); break;
                    case Assemblier.OpFinallyContext @finally: ParseFinally(@finally); break;
                    case Assemblier.OpNewContext @new: ParseNew(@new); break;
                    case Assemblier.OpNewArrContext @newarr: ParseNewArr(@newarr); break;
                    case Assemblier.OpDupContext @dup: ParseDup(@dup); break;
                    case Assemblier.OpPopContext @pop: ParsePop(@pop); break;
                    case Assemblier.OpNopContext @nop: ParseNop(@nop); break;
                }
            }
        }
        private void ParseLdFld(Assemblier.OpLdFldContext context)
        {
            mILWriter.Write((byte)OpCode.LdFld);
        }
        private void ParseLdFldA(Assemblier.OpLdFldAContext context)
        {
            mILWriter.Write((byte)OpCode.LdFldA);
        }
        private void ParseLdLoc(Assemblier.OpLdLocContext context)
        {
            mILWriter.Write((byte)OpCode.LdLoc);
        }
        private void ParseLdLocA(Assemblier.OpLdLocAContext context)
        {
            mILWriter.Write((byte)OpCode.LdLocA);
        }
        private void ParseLdArg(Assemblier.OpLdArgContext context)
        {
            mILWriter.Write((byte)OpCode.LdArg);
        }
        private void ParseLdArgA(Assemblier.OpLdArgAContext context)
        {
            mILWriter.Write((byte)OpCode.LdArgA);
        }
        private void ParseLdElem(Assemblier.OpLdElemContext context)
        {
            mILWriter.Write((byte)OpCode.LdElem);
        }
        private void ParseLdElemA(Assemblier.OpLdElemAContext context)
        {
            mILWriter.Write((byte)OpCode.LdElemA);
        }
        private void ParseLdStr(Assemblier.OpLdStrContext context)
        {
            mILWriter.Write((byte)OpCode.LdStr);
        }
        private void ParseLdC(Assemblier.OpLdCContext context)
        {
            mILWriter.Write((byte)OpCode.LdC);
        }
        private void ParseLdFn(Assemblier.OpLdFnContext context)
        {
            mILWriter.Write((byte)OpCode.LdFn);
        }
        private void ParseLdNull(Assemblier.OpLdNullContext context)
        {
            mILWriter.Write((byte)OpCode.LdNull);
        }
        private void ParseStFld(Assemblier.OpStFldContext context)
        {
            mILWriter.Write((byte)OpCode.StFld);
        }
        private void ParseStLoc(Assemblier.OpStLocContext context)
        {
            mILWriter.Write((byte)OpCode.StLoc);
        }
        private void ParseStArg(Assemblier.OpStArgContext context)
        {
            mILWriter.Write((byte)OpCode.StArg);
        }
        private void ParseStElem(Assemblier.OpStElemContext context)
        {
            mILWriter.Write((byte)OpCode.StElem);
        }
        private void ParseStTA(Assemblier.OpStTAContext context)
        {
            mILWriter.Write((byte)OpCode.StTA);
        }
        private void ParseAdd(Assemblier.OpAddContext context)
        {
            mILWriter.Write((byte)OpCode.Add);
        }
        private void ParseSub(Assemblier.OpSubContext context)
        {
            mILWriter.Write((byte)OpCode.Sub);
        }
        private void ParseMul(Assemblier.OpMulContext context)
        {
            mILWriter.Write((byte)OpCode.Mul);
        }
        private void ParseDiv(Assemblier.OpDivContext context)
        {
            mILWriter.Write((byte)OpCode.Div);
        }
        private void ParseMod(Assemblier.OpModContext context)
        {
            mILWriter.Write((byte)OpCode.Mod);
        }
        private void ParseAnd(Assemblier.OpAndContext context)
        {
            mILWriter.Write((byte)OpCode.And);
        }
        private void ParseOr(Assemblier.OpOrContext context)
        {
            mILWriter.Write((byte)OpCode.Or);
        }
        private void ParseXor(Assemblier.OpXorContext context)
        {
            mILWriter.Write((byte)OpCode.Xor);
        }
        private void ParseNot(Assemblier.OpNotContext context)
        {
            mILWriter.Write((byte)OpCode.Not);
        }
        private void ParseNeg(Assemblier.OpNegContext context)
        {
            mILWriter.Write((byte)OpCode.Neg);
        }
        private void ParseConv(Assemblier.OpConvContext context)
        {
            mILWriter.Write((byte)OpCode.Conv);
        }
        private void ParseCall(Assemblier.OpCallContext context)
        {
            mILWriter.Write((byte)OpCode.Call);
        }
        private void ParseCallVirt(Assemblier.OpCallVirtContext context)
        {
            mILWriter.Write((byte)OpCode.CallVirt);
        }
        private void ParseRet(Assemblier.OpRetContext context)
        {
            mILWriter.Write((byte)OpCode.Ret);
        }
        private void ParseCmp(Assemblier.OpCmpContext context)
        {
            mILWriter.Write((byte)OpCode.Cmp);
        }
        private void ParseJcc(Assemblier.OpJccContext context)
        {
            mILWriter.Write((byte)OpCode.Jcc);
        }
        private void ParseJmp(Assemblier.OpJmpContext context)
        {
            mILWriter.Write((byte)OpCode.Jmp);
        }
        private void ParseThrow(Assemblier.OpThrowContext context)
        {
            mILWriter.Write((byte)OpCode.Throw);
        }
        private void ParseTry(Assemblier.OpTryContext context)
        {
            mILWriter.Write((byte)OpCode.Try);
        }
        private void ParseCatch(Assemblier.OpCatchContext context)
        {
            mILWriter.Write((byte)OpCode.Catch);
        }
        private void ParseFinally(Assemblier.OpFinallyContext context)
        {
            mILWriter.Write((byte)OpCode.Finally);
        }
        private void ParseNew(Assemblier.OpNewContext context)
        {
            mILWriter.Write((byte)OpCode.New);
        }
        private void ParseNewArr(Assemblier.OpNewArrContext context)
        {
            mILWriter.Write((byte)OpCode.NewArr);
        }
        private void ParseDup(Assemblier.OpDupContext context)
        {
            mILWriter.Write((byte)OpCode.Dup);
        }
        private void ParsePop(Assemblier.OpPopContext context)
        {
            mILWriter.Write((byte)OpCode.Pop);
        }
        private void ParseNop(Assemblier.OpNopContext context)
        {
            mILWriter.Write((byte)OpCode.Nop);
        }
    }
}