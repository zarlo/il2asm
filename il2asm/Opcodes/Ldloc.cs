using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace il2asm.Opcodes
{
    public class Ldloc : IOpcode
    {
        public Ldloc()
        {
            OP.Add(OpCodes.Ldloc);
            OP.Add(OpCodes.Ldloc_0);
            OP.Add(OpCodes.Ldloc_1);
            OP.Add(OpCodes.Ldloc_2);
            OP.Add(OpCodes.Ldloc_3);
            OP.Add(OpCodes.Ldloc_S);
        }

        public override void Compile(Instruction i, AsmBuilder ab, List<string> Offsets, MethodDefinition md)
        {
            if (!char.IsDigit(i.OpCode.Name.Split('.').Last()[0]))
            {
                ab.Mov("eax", "dword[ebp - " + (4 * (Convert.ToInt32(i.Operand.ToString().Split('_').Last()) + 1)) + "]");
                ab.Push("eax");
            }
            else
            {
                ab.Mov("eax", "dword[ebp - " + (4 * (Convert.ToInt32(i.OpCode.Name.Split('.').Last()) + 1)) + "]");
                ab.Push("eax");
            }
        }
    }
}
