using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace il2asm.Core.Attributes
{
    public class Plug : Attribute
    {
        public Type Target { get; set; }

        public Plug(Type t)
        {
            Target = t;
        }
    }
}
