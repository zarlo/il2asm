using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace il2asm.Opcodes
{
    public class Ret : IOpcode
    {
        public Ret()
        {
            OP.Add(OpCodes.Ret);
        }

        public override void Compile(Instruction i, AsmBuilder ab, List<string> Offsets, MethodDefinition md)
        {

            ab.Pop("edi");
            ab.Pop("esi");
            ab.Mov("esp", "ebp");
            ab.Pop("ebp");

            ab.Line();

            ab.Ret();
        }
    }
}
