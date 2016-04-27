using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace il2asm.Opcodes
{
    public class Ldsfld : IOpcode
    {
        public Ldsfld()
        {
            OP.Add(OpCodes.Ldsfld);
        }

        public override void Compile(Instruction i, AsmBuilder ab, List<string> Offsets, MethodDefinition md)
        {
            ab.Mov("eax", "dword [" + Utils.SafeName(i.Operand.ToString()) + "]");
            ab.Push("eax");
        }
    }
}
