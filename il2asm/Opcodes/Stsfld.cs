using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace il2asm.Opcodes
{
    public class Stsfld : IOpcode
    {
        public Stsfld()
        {
            OP.Add(OpCodes.Stsfld);
        }

        public override void Compile(Instruction i, AsmBuilder ab, List<string> Offsets, MethodDefinition md)
        {
            ab.Pop("eax");
            ab.Mov("dword [" + Utils.SafeName(i.Operand.ToString()) + "]", "eax");
        }
    }
}
