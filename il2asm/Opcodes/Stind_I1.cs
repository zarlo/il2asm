using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace il2asm.Opcodes
{
    public class Stind_I1 : IOpcode
    {
        public Stind_I1()
        {
            OP.Add(OpCodes.Stind_I1);
        }

        public override void Compile(Instruction i, AsmBuilder ab, List<string> Offsets, MethodDefinition md)
        {
            ab.Pop("eax"); //value
            ab.Pop("ebx"); //addres
            ab.Mov("byte[ebx]", "al");
        }
    }
}
