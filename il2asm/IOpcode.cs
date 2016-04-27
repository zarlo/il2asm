using Mono.Cecil;
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

        public abstract void Compile(Instruction i, AsmBuilder ab, List<string> Offsets, MethodDefinition md);

        public static List<IOpcode> Opcodes = new List<IOpcode>();

        public static void AutoCompile(Instruction i, AsmBuilder ab, List<string> Offsets, MethodDefinition md)
        {
            ab.Comment(i.ToString());
            bool Found = false;
            if (Offsets.Contains(Utils.SafeName(md.FullName) + i.ToString().Split(':')[0]))
            {
                ab.Label(Utils.SafeName(md.FullName) + i.ToString().Split(':')[0]);
            }

            foreach (var z in Opcodes)
            {
                if(z.OP.Contains(i.OpCode))
                {
                    z.Compile(i, ab, Offsets, md);
                    Found = true;
                    break;
                }
            }
            if(!Found)
            {
                Console.WriteLine("Missing opcode: " + i.ToString());
            }

            ab.Line();
        }

        public static void BuildOpcodeIndex()
        {
            Opcodes.Clear();

            foreach(var i in Assembly.GetExecutingAssembly().GetModules())
            {
                foreach(var z in i.GetTypes())
                {
                    if (!z.IsAbstract && z.BaseType == typeof(IOpcode))
                    {
                        Opcodes.Add(Activator.CreateInstance(z) as IOpcode);
                    }
                }
            }
        }
    }
}
