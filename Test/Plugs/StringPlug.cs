using il2asm.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Plugs
{
    [Plug(typeof(string))]
    public unsafe class StringPlug
    {

        [PlugMask(typeof(int))]
        public static char get_Chars(byte* value, int index)
        {
            var val = value[index + 4];
            return (char)val;
        }

        [PlugMask]
        public static int get_Length(int* value)
        {
            var val = value[0];

            return val;
        }
    }
}
