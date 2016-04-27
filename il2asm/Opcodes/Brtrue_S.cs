using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace il2asm.Opcodes
{
    public class Brtrue_S : IOpcode
    {
        public Brtrue_S()
        {
            OP.Add(OpCodes.Brtrue_S);
        }

        public override void Compile(Instruction i, AsmBuilder ab, List<string> Offsets, MethodDefinition md)
        {
            ab.Pop("eax"); //flag
            ab.Cmp("eax", "1");
            ab.Jmpe(Utils.SafeName(md.FullName) + i.Operand.ToString().Split(':')[0]);
        }
    }
}
