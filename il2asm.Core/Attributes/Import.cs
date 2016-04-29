using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace il2asm.Core.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class Import : Attribute
    {
        public string Dll { get; set; }

        public Import(string dl)
        {
            Dll = dl;
        }

    }
}
