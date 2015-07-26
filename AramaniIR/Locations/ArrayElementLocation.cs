using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AramaniIR.Variables;

namespace AramaniIR.Commands
{
    class ArrayElementIndex : Location
    {
        public StackVariable ArrayBase { get; set; }

        public StackVariable Index { get; set; }

        public virtual ICollection<Variable> GetOperators()
        {
            return new StackVariable[] { ArrayBase, Index };
        }

        public sealed int OperandCount()
        {
            return 2;
        }

        public sealed bool HasOperators()
        {
            return true;
        }
    }
}
