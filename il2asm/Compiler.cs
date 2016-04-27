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

            /*
                push ebp ;save base ptr
                mov ebp, esp ;move stack ptr to bace ptr
                sub esp, 2 ;make room for all vars
                push edi
                push esi
            */

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
                CompileOpcode(i, Offsets, md);
            }
            
        }

        public void CompileOpcode(Instruction i, List<string> Offsets, MethodDefinition md)
        {
            ab.Comment(i.ToString());

            if(Offsets.Contains(Utils.SafeName(md.FullName) + i.ToString().Split(':')[0]))
            {
                ab.Label(Utils.SafeName(md.FullName) + i.ToString().Split(':')[0]);
            }
         

            if(i.OpCode.Name.StartsWith(OpCodes.Ldc_I4.Name, StringComparison.Ordinal))
            {
                if (!char.IsDigit(i.OpCode.Name.Split('.').Last()[0]))
                {                   
                    ab.Push(i.Operand.ToString());
                }
                else
                {
                    ab.Push(i.OpCode.Name.Split('.').Last());
                }

            }

            if (i.OpCode.Name.StartsWith(OpCodes.Stloc.Name, StringComparison.Ordinal))
            {
                if (!char.IsDigit(i.OpCode.Name.Split('.').Last()[0]))
                {
                    //pop eax
                    //mov dword[esp + 4], eax
                    ab.Pop("eax");
                    ab.Mov("dword[ebp - " + (4 * (Convert.ToInt32(i.Operand.ToString()) + 1)) + "]", "eax");
                }
                else
                {
                    //pop eax
                    //mov dword[esp + 4], eax
                    ab.Pop("eax");
                    ab.Mov("dword[ebp - " + (4 * (Convert.ToInt32(i.OpCode.Name.Split('.').Last()) + 1)) + "]", "eax");
                }

            }

            if (i.OpCode.Name.StartsWith(OpCodes.Ldloc.Name, StringComparison.Ordinal))
            {
                if (!char.IsDigit(i.OpCode.Name.Split('.').Last()[0]))
                {                    
                    ab.Mov("eax", "dword[ebp - " + (4 * (Convert.ToInt32(i.Operand.ToString()) + 1)) + "]");
                    ab.Push("eax");
                }
                else
                {
                    ab.Mov("eax", "dword[ebp - " + (4 * (Convert.ToInt32(i.OpCode.Name.Split('.').Last()) + 1)) + "]");
                    ab.Push("eax");
                }

            }

            if (i.OpCode == OpCodes.Stind_I1)
            {
                ab.Pop("eax"); //value
                ab.Pop("ebx"); //addres
                ab.Mov("byte[ebx]" , "al");
            }

            if (i.OpCode == OpCodes.Add)
            {
                ab.Pop("eax"); //value a
                ab.Pop("ebx"); //value b
                ab.Add("eax", "ebx");
                ab.Push("eax");
            }

            if (i.OpCode == OpCodes.Ldsfld)
            {
                ab.Mov("eax", "dword ["+ Utils.SafeName(i.Operand.ToString()) +"]");
                ab.Push("eax");
            }

            if (i.OpCode == OpCodes.Stsfld)
            {
                ab.Pop("eax");
                ab.Mov("dword [" + Utils.SafeName(i.Operand.ToString()) + "]", "eax");
                
            }

            if (i.OpCode == OpCodes.Sub)
            {
                ab.Pop("eax"); //value a
                ab.Pop("ebx"); //value b
                ab.Sub("eax", "ebx");
                ab.Push("eax");
            }

            if (i.OpCode == OpCodes.Call)
            {               
                ab.Call(Utils.SafeName(i.Operand.ToString()));
            }

            if (i.OpCode == OpCodes.Br_S)
            {
                ab.Jmp(Utils.SafeName(md.FullName) +i.Operand.ToString().Split(':')[0]);
            }

            if (i.OpCode == OpCodes.Clt)
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

            if (i.OpCode == OpCodes.Brtrue_S)
            {
                ab.Pop("eax"); //flag
                ab.Cmp("eax", "1");
                ab.Jmpe(Utils.SafeName(md.FullName) + i.Operand.ToString().Split(':')[0]);
            }

            if (i.OpCode == OpCodes.Ret)
            {
                /*
                    pop edi
                    pop esi
                    mov esp, ebp
                    pop ebp
                */

                ab.Pop("edi");
                ab.Pop("esi");
                ab.Mov("esp", "ebp");
                ab.Pop("ebp");

                ab.Line();

                ab.Ret();
            }

            ab.Line();           
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
