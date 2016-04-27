using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace il2asm.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class OpcodeCompiler : Attribute
    {
       
    }
}
