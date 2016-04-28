using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace il2asm.Opcodes
{
    public class Lldind_I4 : IOpcode
    {
        public Lldind_I4()
        {
            OP.Add(OpCodes.Ldind_I4);
        }

        public override void Compile(Instruction i, AsmBuilder ab, List<string> Offsets, MethodDefinition md)
        {
            ab.Pop("ebx"); //addres
            ab.Mov("eax", "dword [ebx]");
            ab.Push("eax");
        }
    }
}
