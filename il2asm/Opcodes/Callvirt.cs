using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace il2asm.Opcodes
{
    public class Callvirt : IOpcode
    {
        public Callvirt()
        {
            OP.Add(OpCodes.Callvirt);
        }

        public override void Compile(Instruction i, AsmBuilder ab, List<string> Offsets, MethodDefinition md)
        {            
            if (Compiler.PlugIndex.ContainsKey(Utils.SafeName(i.Operand.ToString())))
            {
                ab.Comment("Pluged");
                ab.Call(Compiler.PlugIndex[Utils.SafeName(i.Operand.ToString())]);
            }
            else
            {
                ab.Call(Utils.SafeName(i.Operand.ToString()));
            }

           

            if ((i.Operand as MethodReference).ReturnType.ToString() != typeof(void).ToString())
            {
                ab.Push("eax");
            }


        }
    }
}
