
namespace Test
{
    unsafe class Program
    {

        static byte* fb = (byte*)0XB8000;

        static void Main()
        {
            Utils.Clear();
            Print();
        }

        static void Print()
        {
            int c = 0;
            for (int i = 65; i < 90; i++)
            {
                fb[c] = (byte)i;
                fb[c + 1] = (byte)0X0F;
                c += 2;
            }
        }
        

       
    }


    public unsafe static class Utils
    {
        static byte* fb = (byte*)0XB8000;

        public static void Clear()
        {
            for (int i = 0; i < (80 * 25) * 2; i++)
            {
                fb[i] = 0;
            }
        }
    }
}
