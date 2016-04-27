using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace il2asm
{
    public class Utils
    {
        public static string SafeName(string nm)
        {
            return nm
                .Replace(" ", "_")
                .Replace(":", "_")
                .Replace("(", "")
                .Replace(")", "")
                .Replace("[", "_")
                .Replace("*", "ASTRIC")
                .Replace("]", "_");
        }
    }
}
