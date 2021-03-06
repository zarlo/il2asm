﻿using il2asm.Core.Attributes;
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
        public static Dictionary<string, string> PlugIndex = new Dictionary<string, string>();
        public static List<string> ConstantIndex = new List<string>();

        public void Compile(string inFile, string outFile)
        {
            PlugIndex.Clear();
            ConstantIndex.Clear();
            IOpcode.BuildOpcodeIndex();

            var lst = new List<AssemblyDefinition>();

            if (!File.Exists(inFile))
            {
                Console.WriteLine("The file \"" + inFile + "\" does not exist.");
                return;
            }

            ab = new AsmBuilder();
            lst.Add(AssemblyDefinition.ReadAssembly("i2a.Bace.dll"));
            var asm = AssemblyDefinition.ReadAssembly(inFile);
            

            foreach (var z in asm.Modules)
            {
                for (int i = 1; i < z.AssemblyReferences.Count; i++)
                {
                    var refs = asm.MainModule.AssemblyReferences[i];
                    lst.Add(AssemblyDefinition.ReadAssembly(Path.Combine(new FileInfo(inFile).Directory.FullName, refs.Name + ".dll")));
                }
            }

            foreach (var i in asm.Modules)
            {
                foreach (var z in i.Types)
                {
                    var att = z.CustomAttributes.Where((x) => { return x.AttributeType.FullName == typeof(Import).FullName; });
                    if (att.Count() != 0)
                    {
                        var str = att.First().ConstructorArguments[0].Value.ToString();
                        lst.Add(AssemblyDefinition.ReadAssembly(Path.Combine(new FileInfo(inFile).Directory.FullName, str)));
                    }
                }
            }

                    lst.Add(asm);
            ab.Global("kmain");
            ab.Label("kmain");

            foreach(var i in lst)
            {
                ScanAssembly(i);
            }

            ab.Jmp(Utils.SafeName(asm.EntryPoint.FullName));
            ab.Ret();
            ab.Line();
            ab.Line();


            foreach (var z in lst)
            {
                foreach (var i in z.Modules)
                {
                    CompileModule(i);
                }
            }

            File.WriteAllText(outFile, ab.ToString());

        }

        private void ScanAssembly(AssemblyDefinition Assembly)
        {
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
                        if (!y.Name.Contains(".ctor"))
                        {
                            bool IsPlug = z.CustomAttributes.Where((x) => { return x.AttributeType.FullName == typeof(Plug).FullName; }).Count() != 0;

                            CustomAttribute plug = null;


                            if (IsPlug)
                            {
                                plug = z.CustomAttributes.Where((x) => { return x.AttributeType.FullName == typeof(Plug).FullName; }).First();
                                // ab.Comment("Found Plug for: " + (plug.ConstructorArguments[0].Value).ToString());
                                var tt = Type.GetType(plug.ConstructorArguments[0].Value.ToString());
                                string Paramaters = "";

                                var att = y.CustomAttributes.Where((x) => { return x.AttributeType.FullName == typeof(PlugMask).FullName; });
                                if (att.Count() != 0)
                                {
                                    var str = att.First().ConstructorArguments[0].Value as CustomAttributeArgument[];
                                    for (int j = 0; j < str.Length; j++)
                                    {
                                        Paramaters += Type.GetType(str[j].Value.ToString()).FullName + ",";
                                    }
                                }
                                else
                                {

                                    for (int j = 1; j < y.Parameters.Count; j++)
                                    {
                                        var hh = y.Parameters[j];
                                        Paramaters += hh.ParameterType.FullName + ",";
                                    }
                                }
                                Paramaters = Paramaters.TrimEnd(',');
                                PlugIndex.Add(Utils.SafeName(y.MethodReturnType.ReturnType.ToString() + " " + tt.FullName + "::" + y.Name + "(" + Paramaters + ")"), Utils.SafeName(y.FullName));
                            }
                        }
                    }
                }
            }
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
            bool IsPlug = td.CustomAttributes.Where((x) => { return x.AttributeType.FullName == typeof(Plug).FullName; }).Count() != 0;

            CustomAttribute plug = null;


            if (IsPlug)
            {
                plug = td.CustomAttributes.Where((x) => { return x.AttributeType.FullName == typeof(Plug).FullName; }).First();
                ab.Comment("Plug for: " + (plug.ConstructorArguments[0].Value).ToString());
            }
            ab.Comment("Class: " + td.FullName);
            ab.Line();



            foreach (var i in td.Fields)
            {
                string value = "";
                var x = new byte[] { 0, 0, 0, 0 };
                foreach (var z in x)
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
                    if (IsPlug)
                    {
                        CompileMethod(i,  Type.GetType(plug.ConstructorArguments[0].Value.ToString()));
                    }
                    else
                    {
                        CompileMethod(i);
                    }

                }
            }
        }

        public void CompileMethod(MethodDefinition md, Type plug = null)
        {
            if(plug != null)
            {
                ab.Comment("Pluging: " + md.Name + " for " + plug.Name);
                //System.Char System.String::get_Chars(System.Int32)
                //System.Char_System.String__get_CharsSystem.Int32
               
            }
            ab.Comment("Method: " + md.FullName);
            ab.Line();



            ab.Label(Utils.SafeName(md.FullName));
            
            ab.Line();

          /*  for (int i = 0; i < md.Parameters.Count; i++)
            {
                var z = md.Parameters[i];
                ab.Pop("eax");
                ab.Mov("[ebp + " + (4 + (i * 4)) + "]", "eax");
            }*/
            ab.Line();


            ab.Push("ebp");
            ab.Mov("ebp", "esp");
           // ab.Sub("esp", (md.Body.MaxStackSize * 4).ToString());
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

            if (i.OpCode == OpCodes.Brtrue)
            {
                Offsets.Add(Utils.SafeName(md.FullName) + i.Operand.ToString().Split(':')[0]);
            }

            if (i.OpCode == OpCodes.Brfalse)
            {
                Offsets.Add(Utils.SafeName(md.FullName) + i.Operand.ToString().Split(':')[0]);
            }

            if (i.OpCode == OpCodes.Brfalse_S)
            {
                Offsets.Add(Utils.SafeName(md.FullName) + i.Operand.ToString().Split(':')[0]);
            }
        }

    }
}
