using il2asm.Core.Attributes;

namespace Test
{
    public unsafe class Program
    {

        public static byte* fb = (byte*)0XB8000;
        public static int x = 0;
        public static int y = 0;

        public static void Main()
        {
            Utils.Clear();
            Print();
            PutC('\n');
            PutS("test");
            PutC('\n');
            PutS("test");

        }

        public static void PutS(string s)
        {
            var len = s.Length;
            for (int i = 0; i < len; i++)
            {              
                PutC(s[i]);
            }
        }

        public static void Print()
        {            
            for (int i = 65; i < 90; i++)
            {
                PutC((byte)i);
            }

            PutC('\n');

            for (int i = 65; i < 90; i++)
            {
                PutC((byte)i);
            }
        }

        public static void PutC(char i)
        {
            if(x == 160)
            {
                y++;
                x = 0;
            }
            if (i == '\n')
            {
                y++;
                x = 0;
                return;
            }
            int offset = x + (y * 160);
            fb[offset] = (byte)i;
            fb[offset + 1] = (byte)0X0F;
            x += 2;
        }

        public static void PutC(byte i)
        {
            PutC((char)i);
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

    [Plug(typeof(string))]
    public unsafe class StringPlug
    {

        [PlugMask(typeof(int))]
        public static char get_Chars(byte * value, int index)
        {
            var val = value[index + 4];
            return (char)val;
        }

        [PlugMask]
        public static int get_Length(int * value)
        {           
            var val = value[0];
           
            return val;
        }
    }
}
