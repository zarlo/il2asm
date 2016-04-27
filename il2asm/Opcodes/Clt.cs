using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace il2asm.Opcodes
{
    public class Clt : IOpcode
    {
        public Clt()
        {
            OP.Add(OpCodes.Clt);
        }

        public override void Compile(Instruction i, AsmBuilder ab, List<string> Offsets, MethodDefinition md)
        {
            ab.Pop("eax"); //value a
            ab.Pop("ebx"); //value b
            ab.Cmp("eax", "ebx");
            ab.Jl(i.ToString().Split(':')[0] + "_1");
            ab.Push("1");
            ab.Jmp(i.ToString().Split(':')[0] + "_0");
            ab.Label(i.ToString().Split(':')[0] + "_1");
            ab.Push("0");
            ab.Label(i.ToString().Split(':')[0] + "_0");
        }
    }
}
