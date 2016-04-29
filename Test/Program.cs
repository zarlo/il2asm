using System;
using il2asm.Core.Attributes;

namespace Test
{
    [Import("TestLib.dll")]
    public class Program
    {
        public static void Main()
        {
            Console.Clear();
            Log("Test OS booted ...");
            Log("Woot toot");
        }     
        
        public static void Log(string s)
        {
            Console.ResetColor();
            Console.Write("[");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Log");
            Console.ResetColor();
            Console.Write("] ");
            Console.WriteLine(s);
        }          
    }    
}
