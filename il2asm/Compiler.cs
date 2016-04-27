using Mono.Cecil;
using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace il2asm
{
    public class Compiler
    {
        public AsmBuilder ab = new AsmBuilder();

        private int CurrentStackSize = 0;

        public void Compile(string inFile, string outFile)
        {
            IOpcode.BuildOpcodeIndex();
            if (!File.Exists(inFile))
            {
                Console.WriteLine("The file \"" + inFile + "\" does not exist.");
                return;
            }

            ab = new AsmBuilder();
            var Assembly = AssemblyDefinition.ReadAssembly(inFile);

            ab.Global("kmain");
            ab.Label("kmain");

            foreach (var i in Assembly.Modules)
            {
                foreach (var z in i.Types)
                {
                    foreach (var y in z.Methods)
                    {
                        if (y.Name.Contains(".cctor"))
                        {
                            ab.Call(Utils.SafeName(y.FullName));
                        }
                    }
                }
            }

            ab.Jmp(Utils.SafeName(Assembly.EntryPoint.FullName));
            ab.Ret();
            ab.Line();
            ab.Line();

            foreach (var i in Assembly.Modules)
            {
                CompileModule(i);
            }

            File.WriteAllText(outFile, ab.ToString());

        }

        public void CompileModule(ModuleDefinition md)
        {
            ab.Comment("Module: " + md.FullyQualifiedName);
            ab.Line();

            foreach (var i in md.Types)
            {
                CompileType(i);
            }
        }

        public void CompileType(TypeDefinition td)
        {
            ab.Comment("Class: " + td.FullName);
            ab.Line();



            foreach (var i in td.Fields)
            {
                string value = "";
                var x = new byte[] { 0, 0, 0, 0};
                foreach(var z in x)
                {
                    value += z + ",";
                }

                ab.GlobalVar(Utils.SafeName(i.FullName), value.TrimEnd(','), "db");
            }

            foreach (var i in td.Properties)
            {
                string value = "";
                var x = new byte[] { 0, 0, 0, 0 };
                foreach (var z in x)
                {
                    value += z + ",";
                }

                ab.GlobalVar(Utils.SafeName(i.FullName), value.TrimEnd(','), "db");
            }

            foreach (var i in td.Methods)
            {
                if (!i.Name.Contains(".ctor"))
                {
                    CompileMethod(i);
                }
            }
        }

        public void CompileMethod(MethodDefinition md)
        {
            ab.Comment("Method: " + md.FullName);
            ab.Line();

            ab.Label(Utils.SafeName(md.FullName));
            ab.Line();


            ab.Push("ebp");
            ab.Mov("ebp", "esp");
            ab.Sub("esp", (md.Body.MaxStackSize * 4).ToString());
            ab.Push("edi");
            ab.Push("esi");

            var Offsets = new List<string>();

            foreach (var i in md.Body.Instructions)
            {
                BuildOffsets(i,ref Offsets, md);
            }

            foreach (var i in md.Body.Instructions)
            {
                IOpcode.AutoCompile(i, ab, Offsets, md);
            }
            
        }

        public void BuildOffsets(Instruction i, ref List<string> Offsets, MethodDefinition md)
        {
            if (i.OpCode == OpCodes.Br_S)
            {
                Offsets.Add(Utils.SafeName(md.FullName) + i.Operand.ToString().Split(':')[0]);
            }

            if (i.OpCode == OpCodes.Brtrue_S)
            {
                Offsets.Add(Utils.SafeName(md.FullName) + i.Operand.ToString().Split(':')[0]);
            }
        }

    }
}
