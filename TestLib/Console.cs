using il2asm.Core.Attributes;

namespace TestLib
{
    [Plug(typeof(System.Console))]
    public unsafe static class Console
    {
        public static byte* fb = (byte*)0XB8000;
        public static int x = 0;
        public static int y = 0;
        public static byte C = 0x0F;
        public static byte BC = 0;
        public static byte FC = 14;

        public static void ResetColor()
        {
            C = 0x0F;
        }

        [PlugMask(typeof(System.ConsoleColor))]
        public static void set_BackgroundColor(byte c)
        {
            BC = c;
            SetColor(BC, FC);
        }

        [PlugMask(typeof(System.ConsoleColor))]
        public static void set_ForegroundColor(byte c)
        {
            FC = c;
            SetColor(BC, FC);
        }

        private static void SetColor(byte background, byte forground)
        {
            C = (byte)(((forground & 0x0F) << 4) | (background & 0x0F));
        }

        public static void SetColor(ConsoleColors background, ConsoleColors forground)
        {
            C = (byte)((((byte)forground & 0x0F) << 4) | ((byte)background & 0x0F));
        }

        public static void Clear()
        {
            for (int i = 0; i < (80 * 25) * 2; i++)
            {
                fb[i] = 0;
            }
        }

        [PlugMask(typeof(System.String))]
        public static void WriteLine(string s)
        {
            var len = s.Length;
            for (int i = 0; i < len; i++)
            {
                PutC(s[i]);
            }

            PutC('\n');
        }
        [PlugMask(typeof(System.String))]
        public static void Write(string s)
        {
            var len = s.Length;
            for (int i = 0; i < len; i++)
            {
                PutC(s[i]);
            }
        }

        private static void PutC(char i)
        {
            if (x == 160)
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
            fb[offset + 1] = C;
            x += 2;
        }       
    }
    
    public enum ConsoleColors
    {
        Black = 0,
        Blue = 1,
        Green = 2,
        Red = 4,
        Magenta = 5,
        Brown = 6,
        LightGrey = 7,
        DarkGrey = 8,
        LightBlue = 9,
        LightGreen = 10,
        LightCyan = 11,
        LightRed = 12,
        LightMagenta = 13,
        Yello = 14,
        White = 15
    }
}
