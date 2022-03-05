using System.IO;

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
                    case Assemblier.OpLdIndContext @ldind: ParseLdInd(@ldind); break;
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
                    case Assemblier.OpCastContext @cast: ParseCast(@cast); break;
                    case Assemblier.OpBoxContext @box: ParseBox(@box); break;
                    case Assemblier.OpUnBoxContext @unbox: ParseUnBox(@unbox); break;
                    case Assemblier.OpDupContext @dup: ParseDup(@dup); break;
                    case Assemblier.OpPopContext @pop: ParsePop(@pop); break;
                    case Assemblier.OpNopContext @nop: ParseNop(@nop); break;
                    case Assemblier.OpLabelContext @label: ParseLabel(label); break;
                }
            }
            FixUpOffset();
        }
        private void FixUpOffset()
        {
            foreach ((var index, var label) in mOffsetTable)
            {
                mILStream.Seek(index, SeekOrigin.Begin);
                mILWriter.Write(mLabelMap[label]);
            }
        }
        private void ParseLabel(Assemblier.OpLabelContext context)
        {
            var labelName = context.IDENTIFIER().GetText();
            int index = (int)mILStream.Length;
            if (!mLabelMap.TryAdd(labelName, index))
                throw new DuplicateLabelException($"Label - {labelName} has already been defined");
        }
        private void ParseLdFld(Assemblier.OpLdFldContext context)
        {
            mILWriter.Write((byte)OpCode.LdFld);
            mILWriter.Write(mResolver.ResolveFieldRef(context.fieldRef()));
        }
        private void ParseLdFldA(Assemblier.OpLdFldAContext context)
        {
            mILWriter.Write((byte)OpCode.LdFldA);
            mILWriter.Write(mResolver.ResolveFieldRef(context.fieldRef()));
        }
        private void ParseLdLoc(Assemblier.OpLdLocContext context)
        {
            mILWriter.Write((byte)OpCode.LdLoc);
            mILWriter.Write(mLocalMap[context.IDENTIFIER().GetText()]);
        }
        private void ParseLdLocA(Assemblier.OpLdLocAContext context)
        {
            mILWriter.Write((byte)OpCode.LdLocA);
            mILWriter.Write(mLocalMap[context.IDENTIFIER().GetText()]);
        }
        private void ParseLdArg(Assemblier.OpLdArgContext context)
        {
            mILWriter.Write((byte)OpCode.LdArg);
            mILWriter.Write(mArgumentMap[context.IDENTIFIER().GetText()]);
        }
        private void ParseLdArgA(Assemblier.OpLdArgAContext context)
        {
            mILWriter.Write((byte)OpCode.LdArgA);
            mILWriter.Write(mArgumentMap[context.IDENTIFIER().GetText()]);
        }
        private void ParseLdInd(Assemblier.OpLdIndContext context)
        {
            mILWriter.Write((byte)OpCode.LdInd);
        }
        private void ParseLdElem(Assemblier.OpLdElemContext _)
        {
            mILWriter.Write((byte)OpCode.LdElem);
        }
        private void ParseLdElemA(Assemblier.OpLdElemAContext _)
        {
            mILWriter.Write((byte)OpCode.LdElemA);
        }
        private void ParseLdStr(Assemblier.OpLdStrContext context)
        {
            mILWriter.Write((byte)OpCode.LdStr);
            mILWriter.Write(mResolver.GetTokenFromString(context.STRING().GetText().Trim('"')));
        }
        private void ParseLdC(Assemblier.OpLdCContext context)
        {
            mILWriter.Write((byte)OpCode.LdC);
            var text = context.opConstant().GetText();
            switch(context.primitiveType().GetUnderlyingTokenType())
            {
                case Assemblier.PRIMITIVE_CHAR:
                    mILWriter.Write((byte)CoreTypes.Char);
                    mILWriter.Write(char.Parse(text));
                    break;
                case Assemblier.PRIMITIVE_BOOL:
                    mILWriter.Write((byte)CoreTypes.Bool);
                    mILWriter.Write(bool.Parse(text));
                    break;
                case Assemblier.PRIMITIVE_BYTE:
                    mILWriter.Write((byte)CoreTypes.I1);
                    mILWriter.Write(sbyte.Parse(text));
                    break;
                case Assemblier.PRIMITIVE_UBYTE:
                    mILWriter.Write((byte)CoreTypes.U1);
                    mILWriter.Write(byte.Parse(text));
                    break;
                case Assemblier.PRIMITIVE_SHORT:
                    mILWriter.Write((byte)CoreTypes.I2);
                    mILWriter.Write(short.Parse(text));
                    break;
                case Assemblier.PRIMITIVE_USHORT:
                    mILWriter.Write((byte)CoreTypes.U2);
                    mILWriter.Write(ushort.Parse(text));
                    break;
                case Assemblier.PRIMITIVE_INT:
                    mILWriter.Write((byte)CoreTypes.I4);
                    mILWriter.Write(int.Parse(text));
                    break;
                case Assemblier.PRIMITIVE_UINT:
                    mILWriter.Write((byte)CoreTypes.U4);
                    mILWriter.Write(uint.Parse(text));
                    break;
                case Assemblier.PRIMITIVE_LONG:
                    mILWriter.Write((byte)CoreTypes.I8);
                    mILWriter.Write(long.Parse(text));
                    break;
                case Assemblier.PRIMITIVE_ULONG:
                    mILWriter.Write((byte)CoreTypes.U8);
                    mILWriter.Write(ulong.Parse(text));
                    break;
                case Assemblier.PRIMITIVE_R4:
                    mILWriter.Write((byte)CoreTypes.R4);
                    mILWriter.Write(float.Parse(text));
                    break;
                case Assemblier.PRIMITIVE_R8:
                    mILWriter.Write((byte)CoreTypes.R8);
                    mILWriter.Write(double.Parse(text));
                    break;
                default:
                    throw new UnknownTokenException("Constant format invalid");
            }
        }
        private void ParseLdFn(Assemblier.OpLdFnContext context)
        {
            mILWriter.Write((byte)OpCode.LdFn);
            mILWriter.Write(mResolver.ResolveMethodRef(context.methodRef()));
        }
        private void ParseLdNull(Assemblier.OpLdNullContext _)
        {
            mILWriter.Write((byte)OpCode.LdNull);
        }
        private void ParseStFld(Assemblier.OpStFldContext context)
        {
            mILWriter.Write((byte)OpCode.StFld);
            mILWriter.Write(mResolver.ResolveFieldRef(context.fieldRef()));
        }
        private void ParseStLoc(Assemblier.OpStLocContext context)
        {
            mILWriter.Write((byte)OpCode.StLoc);
            mILWriter.Write(mLocalMap[context.IDENTIFIER().GetText()]);
        }
        private void ParseStArg(Assemblier.OpStArgContext context)
        {
            mILWriter.Write((byte)OpCode.StArg);
            mILWriter.Write(mArgumentMap[context.IDENTIFIER().GetText()]);
        }
        private void ParseStElem(Assemblier.OpStElemContext _)
        {
            mILWriter.Write((byte)OpCode.StElem);
        }
        private void ParseStTA(Assemblier.OpStTAContext _)
        {
            mILWriter.Write((byte)OpCode.StTA);
        }
        private void ParseAdd(Assemblier.OpAddContext _)
        {
            mILWriter.Write((byte)OpCode.Add);
        }
        private void ParseSub(Assemblier.OpSubContext _)
        {
            mILWriter.Write((byte)OpCode.Sub);
        }
        private void ParseMul(Assemblier.OpMulContext _)
        {
            mILWriter.Write((byte)OpCode.Mul);
        }
        private void ParseDiv(Assemblier.OpDivContext _)
        {
            mILWriter.Write((byte)OpCode.Div);
        }
        private void ParseMod(Assemblier.OpModContext _)
        {
            mILWriter.Write((byte)OpCode.Mod);
        }
        private void ParseAnd(Assemblier.OpAndContext _)
        {
            mILWriter.Write((byte)OpCode.And);
        }
        private void ParseOr(Assemblier.OpOrContext _)
        {
            mILWriter.Write((byte)OpCode.Or);
        }
        private void ParseXor(Assemblier.OpXorContext _)
        {
            mILWriter.Write((byte)OpCode.Xor);
        }
        private void ParseNot(Assemblier.OpNotContext _)
        {
            mILWriter.Write((byte)OpCode.Not);
        }
        private void ParseNeg(Assemblier.OpNegContext _)
        {
            mILWriter.Write((byte)OpCode.Neg);
        }
        private void ParseConv(Assemblier.OpConvContext context)
        {
            mILWriter.Write((byte)OpCode.Conv);
            switch (context.primitiveType().GetUnderlyingTokenType())
            {
                case Assemblier.PRIMITIVE_BOOL:
                    mILWriter.Write((byte)CoreTypes.Bool);break;
                case Assemblier.PRIMITIVE_CHAR:
                    mILWriter.Write((byte)CoreTypes.Char); break;
                case Assemblier.PRIMITIVE_BYTE:
                    mILWriter.Write((byte)CoreTypes.I1); break;
                case Assemblier.PRIMITIVE_UBYTE:
                    mILWriter.Write((byte)CoreTypes.U1); break;
                case Assemblier.PRIMITIVE_SHORT:
                    mILWriter.Write((byte)CoreTypes.I2); break;
                case Assemblier.PRIMITIVE_USHORT:
                    mILWriter.Write((byte)CoreTypes.U2); break;
                case Assemblier.PRIMITIVE_INT:
                    mILWriter.Write((byte)CoreTypes.I4); break;
                case Assemblier.PRIMITIVE_UINT:
                    mILWriter.Write((byte)CoreTypes.U4); break;
                case Assemblier.PRIMITIVE_LONG:
                    mILWriter.Write((byte)CoreTypes.I8); break;
                case Assemblier.PRIMITIVE_ULONG:
                    mILWriter.Write((byte)CoreTypes.U8); break;
                case Assemblier.PRIMITIVE_R4:
                    mILWriter.Write((byte)CoreTypes.R4); break;
                case Assemblier.PRIMITIVE_R8:
                    mILWriter.Write((byte)CoreTypes.R8); break;
                default:
                    throw new UnknownTokenException("Convert primitive type invalid");
            }
        }
        private void ParseCall(Assemblier.OpCallContext context)
        {
            mILWriter.Write((byte)OpCode.Call);
            mILWriter.Write(mResolver.ResolveMethodRef(context.methodRef()));
        }
        private void ParseCallVirt(Assemblier.OpCallVirtContext context)
        {
            mILWriter.Write((byte)OpCode.CallVirt);
            mILWriter.Write(mResolver.ResolveMethodRef(context.methodRef()));
        }
        private void ParseRet(Assemblier.OpRetContext _)
        {
            mILWriter.Write((byte)OpCode.Ret);
        }
        private void ParseCmp(Assemblier.OpCmpContext context)
        {
            mILWriter.Write((byte)OpCode.Cmp);
            switch(context.opCmpCond().GetUnderlyingTokenType())
            {
                case Assemblier.IL_CMP_EQ: mILWriter.Write((byte)CmpCondition.EQ); break;
                case Assemblier.IL_CMP_NE: mILWriter.Write((byte)CmpCondition.NE); break;
                case Assemblier.IL_CMP_GT: mILWriter.Write((byte)CmpCondition.GT); break;
                case Assemblier.IL_CMP_LT: mILWriter.Write((byte)CmpCondition.LT); break;
                case Assemblier.IL_CMP_GE: mILWriter.Write((byte)CmpCondition.GE); break;
                case Assemblier.IL_CMP_LE: mILWriter.Write((byte)CmpCondition.LE); break;
                default: throw new UnknownTokenException("Unknown condition type");
            }
        }
        private void ParseJcc(Assemblier.OpJccContext context)
        {
            mILWriter.Write((byte)OpCode.Jcc);

            var index = (int)mILStream.Position;
            var labelName = context.IDENTIFIER().GetText();
            mOffsetTable.Add((index, labelName));
            //Write empty bytes
            mILWriter.Write(int.MaxValue);
        }
        private void ParseJmp(Assemblier.OpJmpContext context)
        {
            mILWriter.Write((byte)OpCode.Jmp);
            var index = (int)mILStream.Position;
            var labelName = context.IDENTIFIER().GetText();
            mOffsetTable.Add((index, labelName));
            //Write empty bytes
            mILWriter.Write(int.MaxValue);
        }
        private void ParseThrow(Assemblier.OpThrowContext context)
        {
            //TODO
            mILWriter.Write((byte)OpCode.Throw);
        }
        private void ParseTry(Assemblier.OpTryContext context)
        {
            //TODO
            mILWriter.Write((byte)OpCode.Try);
        }
        private void ParseCatch(Assemblier.OpCatchContext context)
        {
            //TODO
            mILWriter.Write((byte)OpCode.Catch);
        }
        private void ParseFinally(Assemblier.OpFinallyContext context)
        {
            //TODO
            mILWriter.Write((byte)OpCode.Finally);
        }
        private void ParseNew(Assemblier.OpNewContext context)
        {
            mILWriter.Write((byte)OpCode.New);
            mILWriter.Write(mResolver.ResolveMethodRef(context.methodRef()));
        }
        private void ParseNewArr(Assemblier.OpNewArrContext context)
        {
            mILWriter.Write((byte)OpCode.NewArr);
            mILWriter.Write(mResolver.ResolveTypeRef(context.typeRef()));
        }
        private void ParseCast(Assemblier.OpCastContext context)
        {
            mILWriter.Write((byte)OpCode.Cast);
            mILWriter.Write(mResolver.ResolveTypeRef(context.typeRef()));
        }
        private void ParseBox(Assemblier.OpBoxContext context)
        {
            mILWriter.Write((byte)OpCode.Box);
        }
        private void ParseUnBox(Assemblier.OpUnBoxContext context)
        {
            mILWriter.Write((byte)OpCode.UnBox);
            mILWriter.Write(mResolver.ResolveTypeRef(context.typeRef()));
        }
        private void ParseDup(Assemblier.OpDupContext _)
        {
            mILWriter.Write((byte)OpCode.Dup);
        }
        private void ParsePop(Assemblier.OpPopContext _)
        {
            mILWriter.Write((byte)OpCode.Pop);
        }
        private void ParseNop(Assemblier.OpNopContext _)
        {
            mILWriter.Write((byte)OpCode.Nop);
        }
    }
}