using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace il2asm.Opcodes
{
    public class Ldstr : IOpcode
    {
        public Ldstr()
        {
            OP.Add(OpCodes.Ldstr);
        }

        public override void Compile(Instruction i, AsmBuilder ab, List<string> Offsets, MethodDefinition md)
        {
            if (!Compiler.ConstantIndex.Contains(i.Operand.ToString()))
            {
                string constant = "";
                foreach(var z in i.Operand.ToString())
                {
                    constant += (byte)z + ", ";
                }
                constant = constant.Trim().TrimEnd(',');
                ab.GlobalVar(Utils.SafeName("STR" + i.Operand.ToString()), (i.Operand.ToString().Length -1) + ",0 ,0 ,0 , " + constant , "db");
                Compiler.ConstantIndex.Add(i.Operand.ToString());
            }
            ab.Mov("eax", "dword " +  Utils.SafeName("STR" + i.Operand.ToString()) + "");
            ab.Push("eax");
        }
    }
}
