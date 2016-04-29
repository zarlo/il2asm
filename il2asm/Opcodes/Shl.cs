using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace il2asm.Opcodes
{
    public class Shl : IOpcode
    {
        public Shl()
        {
            OP.Add(OpCodes.Shl);
        }

        public override void Compile(Instruction i, AsmBuilder ab, List<string> Offsets, MethodDefinition md)
        {            
            ab.Pop("ebx"); //bits
            ab.Pop("eax"); //value a
            ab.Mov("ecx", "ebx");
            ab.Shl("eax", "cl");
            ab.Push("eax");
        }
    }
}
