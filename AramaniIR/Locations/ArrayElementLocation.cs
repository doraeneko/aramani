using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aramani.IR.Variables;

namespace Aramani.IR.Commands
{
    class ArrayElementIndex : Location
    {
        public StackVariable ArrayBase { get; set; }

        public StackVariable Index { get; set; }

        public override ICollection<Variable> GetOperands()
        {
            return new StackVariable[] { ArrayBase, Index };
        }

        public override int OperandCount()
        {
            return 2;
        }

        public override bool HasOperands()
        {
            return true;
        }

        public override string Description
        {
            get
            {
                return ArrayBase.Description + "[" + Index.Description + "]";
            }
        }
    }
}
