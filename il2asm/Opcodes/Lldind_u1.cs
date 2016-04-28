using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace il2asm.Opcodes
{
    public class Lldind_U1 : IOpcode
    {
        public Lldind_U1()
        {
            OP.Add(OpCodes.Ldind_U1);
        }

        public override void Compile(Instruction i, AsmBuilder ab, List<string> Offsets, MethodDefinition md)
        {
            ab.Pop("ebx"); //addres
            ab.Mov("al", "byte[ebx]");
            ab.Push("eax");
        }
    }
}
