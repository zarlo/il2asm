using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace il2asm.Opcodes
{
    public class Stloc : IOpcode
    {
        public Stloc()
        {
            OP.Add(OpCodes.Stloc);
            OP.Add(OpCodes.Stloc_0);
            OP.Add(OpCodes.Stloc_1);
            OP.Add(OpCodes.Stloc_2);
            OP.Add(OpCodes.Stloc_3);
            OP.Add(OpCodes.Stloc_S);
        }

        public override void Compile(Instruction i, AsmBuilder ab, List<string> Offsets, MethodDefinition md)
        {
            if (!char.IsDigit(i.OpCode.Name.Split('.').Last()[0]))
            {
                //pop eax
                //mov dword[esp + 4], eax
                ab.Pop("eax");
                ab.Mov("dword[ebp - " + (4 * (Convert.ToInt32(i.Operand.ToString().Split('_').Last()) + 1)) + "]", "eax");
            }
            else
            {
                //pop eax
                //mov dword[esp + 4], eax
                ab.Pop("eax");
                ab.Mov("dword[ebp - " + (4 * (Convert.ToInt32(i.OpCode.Name.Split('.').Last()) + 1)) + "]", "eax");
            }
        }
    }
}
