using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil.Cil;
using il2asm.Attributes;

namespace il2asm.Opcodes
{
    [OpcodeCompiler]
    public class Nop : IOpcode
    {
        public Nop()
        {
            OP.Add(OpCodes.Nop);
        }

        public override void Compile(OpCode op, AsmBuilder sb)
        {
           
        }
    }
}
