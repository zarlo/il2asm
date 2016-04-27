using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace il2asm.Opcodes
{
    public class Br_S : IOpcode
    {
        public Br_S()
        {
            OP.Add(OpCodes.Br_S);
        }

        public override void Compile(Instruction i, AsmBuilder ab, List<string> Offsets, MethodDefinition md)
        {
            ab.Jmp(Utils.SafeName(md.FullName) + i.Operand.ToString().Split(':')[0]);
        }
    }
}
