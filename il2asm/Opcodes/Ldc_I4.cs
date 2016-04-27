using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace il2asm.Opcodes
{
    public class Ldc_I4 : IOpcode
    {
        public Ldc_I4()
        {
            OP.Add(OpCodes.Ldc_I4);
            OP.Add(OpCodes.Ldc_I4_0);
            OP.Add(OpCodes.Ldc_I4_1);
            OP.Add(OpCodes.Ldc_I4_2);
            OP.Add(OpCodes.Ldc_I4_3);
            OP.Add(OpCodes.Ldc_I4_4);
            OP.Add(OpCodes.Ldc_I4_5);
            OP.Add(OpCodes.Ldc_I4_6);
            OP.Add(OpCodes.Ldc_I4_7);
            OP.Add(OpCodes.Ldc_I4_8);
            OP.Add(OpCodes.Ldc_I4_S);
        }

        public override void Compile(Instruction i, AsmBuilder ab, List<string> Offsets, MethodDefinition md)
        {
            if (!char.IsDigit(i.OpCode.Name.Split('.').Last()[0]))
            {
                ab.Push(i.Operand.ToString());
            }
            else
            {
                ab.Push(i.OpCode.Name.Split('.').Last());
            }
        }
    }
}
