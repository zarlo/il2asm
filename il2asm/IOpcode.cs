using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace il2asm
{
    public abstract class IOpcode
    {
        public List<OpCode> OP { get; set; } = new List<OpCode>();

        public abstract void Compile(OpCode op, AsmBuilder sb);

        public static List<IOpcode> Opcodes = new List<IOpcode>();

        public static void BuildOpcodeIndex()
        {
            foreach(var i in Assembly.GetExecutingAssembly().GetModules())
            {
                
            }
        }
    }
}
