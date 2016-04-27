using CommandLine;
using CommandLine.Text;
using il2asm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace i2a
{


    public class Options
    {
        [Option('i', "input", Required = true, HelpText = "Input file to read.")]
        public string InputFile { get; set; }

        [Option('o', "ourput", Required = true, HelpText = "Output file to write.")]
        public string OutputFile { get; set; }

        [Option('v', null, HelpText = "Print details  and version during execution.")]
        public bool Verbose { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            if (Parser.Default.ParseArguments(args, options))
            {
                if (options.Verbose)
                {
                    Console.WriteLine("Il2Asm version: 0.0.1");
                    Console.WriteLine("Input File: " + options.InputFile);
                }

                Compiler p = new Compiler();
                p.Compile(options.InputFile, options.OutputFile);

            }
            else
            {
                Console.WriteLine(HelpText.AutoBuild(options));
            }
        }
    }
}
