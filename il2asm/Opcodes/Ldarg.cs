using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace il2asm.Opcodes
{
    public class Ldarg : IOpcode
    {

        public Ldarg()
        {
            OP.Add(OpCodes.Ldarg);
            OP.Add(OpCodes.Ldarg_0);
            OP.Add(OpCodes.Ldarg_1);
            OP.Add(OpCodes.Ldarg_2);
            OP.Add(OpCodes.Ldarg_3);
            OP.Add(OpCodes.Ldarg_S);
        }

        public override void Compile(Instruction i, AsmBuilder ab, List<string> Offsets, MethodDefinition md)
        {

            if (!char.IsDigit(i.OpCode.Name.Split('.').Last()[0]))
            {
                ab.Mov("eax", "[ebp + " + (8 + ((int)i.Operand* 4)) + "]");
                ab.Push("eax");
            }
            else
            {
                ab.Mov("eax", "[ebp + " + (8 + (int.Parse(i.OpCode.Name.Split('.').Last()) * 4)) + "]");
                ab.Push("eax");
            }
           
        }
    }
}
