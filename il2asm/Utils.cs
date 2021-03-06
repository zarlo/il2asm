﻿using System;
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
            return nm.Replace("\r\n", "\n")
                .Replace(" ", "SPACE")
                .Replace(":", "DP")
                .Replace("(", "BO")
                .Replace(")", "BC")
                .Replace("[", "SBO")
                .Replace("*", "ASTRIC")
                .Replace("<", "GREATERTHAN")
                .Replace(">", "SMALLERTHAN")
                .Replace(",", "COMMA")
                .Replace("\n", "NEWLINE")
                .Replace("]", "SBC");
        }
    }
}
