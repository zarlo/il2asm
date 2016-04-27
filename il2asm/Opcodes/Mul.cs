using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace il2asm.Opcodes
{
    public class Mul : IOpcode
    {
        public Mul()
        {
            OP.Add(OpCodes.Mul);
        }

        public override void Compile(Instruction i, AsmBuilder ab, List<string> Offsets, MethodDefinition md)
        {
            ab.Pop("eax"); //value a
            ab.Pop("ebx"); //value b
            ab.Imul("ebx");               // imul ecx, eax, ebx; result in ecx
            ab.Push("EAX");
        }
    }
}
