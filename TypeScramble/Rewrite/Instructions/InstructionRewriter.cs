﻿using dnlib.DotNet;
using dnlib.DotNet.Emit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeScramble.Rewrite.Instructions {
    abstract class InstructionRewriter {
        public abstract void ProcessInstruction(ITypeService service, MethodDef method, IList<Instruction> body, ref int index, Instruction i);
        public abstract Type TargetType { get; }
    }

    abstract class InstructionRewriter<T> : InstructionRewriter {

        public override void ProcessInstruction(ITypeService service, MethodDef method, IList<Instruction> body, ref int index, Instruction i) {
            ProcessOperand(service, method, body, ref index, (T)i.Operand);
        }
        public override Type TargetType => typeof(T);

        public abstract void ProcessOperand(ITypeService service, MethodDef method, IList<Instruction> body, ref int index, T operand);
    }
}
