using TestLib;

namespace Test
{
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
            Console.SetColor(ConsoleColors.Black, ConsoleColors.Green);
            Console.Write("Log");
            Console.ResetColor();
            Console.Write("] ");
            Console.WriteLine(s);
        }          
    }    
}
