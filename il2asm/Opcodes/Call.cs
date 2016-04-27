using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace il2asm.Opcodes
{
    public class Call : IOpcode
    {
        public Call()
        {
            OP.Add(OpCodes.Call);
        }

        public override void Compile(Instruction i, AsmBuilder ab, List<string> Offsets, MethodDefinition md)
        {
            ab.Call(Utils.SafeName(i.Operand.ToString()));
        }
    }
}
