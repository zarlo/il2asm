using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace il2asm
{
    public class AsmBuilder
    {
        private StringBuilder sb = new StringBuilder();
        private StringBuilder sbEnd = new StringBuilder();

        public AsmBuilder()
        {
            sbEnd.AppendLine();
            sbEnd.AppendLine("SECTION .data");
        }

        public void Label(string name)
        {
            sb.AppendLine(name + ":");
        }

        public void Imul(string v)
        {
            sb.AppendLine("imul " + v);
        }

        public void Global(string name)
        {
            sb.AppendLine("global " + name);
        }

        public void Comment(string Comment)
        {
            sb.AppendLine(";" + Comment);
        }

        public void Jmp(string name)
        {
            sb.AppendLine("jmp " + name);
        }

        public void GlobalVar(string name, string value,string type)
        {
            sbEnd.AppendLine(name + ": " + type + " " + value);
        }

        public void Jmpe(string name)
        {
            sb.AppendLine("je " + name);
        }

        public void Jge(string name)
        {
            sb.AppendLine("jge " + name);
        }

        public void Jle(string name)
        {
            sb.AppendLine("jle " + name);
        }

        public void Jg(string name)
        {
            sb.AppendLine("jg " + name);
        }

        public void Jl(string name)
        {
            sb.AppendLine("jl " + name);
        }

        public void Pushf()
        {
            sb.AppendLine("pushf");
        }
        public void Call(string name)
        {
            sb.AppendLine("call " + name);
        }

        public void Line()
        {
            sb.AppendLine();
        }

        public void Ret()
        {
            sb.AppendLine("ret");
        }

        public void Pop(string value)
        {
            sb.AppendLine("pop " + value);
        }

        public void Push(string value)
        {
            sb.AppendLine("push " + value);
        }

        public void Mov(string valueA, string valueB)
        {
            sb.AppendLine("mov " + valueA + ", " + valueB);
        }

        public void Sub(string valueA, string valueB)
        {
            sb.AppendLine("sub " + valueA + ", " + valueB);
        }

        public void Add(string valueA, string valueB)
        {
            sb.AppendLine("add " + valueA + ", " + valueB);
        }

        public void Mul(string valueA, string valueB)
        {
            sb.AppendLine("mul " + valueA + ", " + valueB);
        }

        public void Cmp(string valueA, string valueB)
        {
            sb.AppendLine("cmp " + valueA + ", " + valueB);
        }

        public override string ToString() => sb.ToString() + sbEnd.ToString();
    }
}
