using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace il2asm.Core.Attributes
{
    public class PlugMask : Attribute
    {
        public Type[] Types { get; set; }

        public PlugMask(params Type[] t)
        {
            Types = t;
        }
    }
}
