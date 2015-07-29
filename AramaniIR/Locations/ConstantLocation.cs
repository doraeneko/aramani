using System;
using System.Collections.Generic;
using Aramani.Base;

using Aramani.IR.Variables;

namespace Aramani.IR.Commands
{
    abstract class ConstantLocation<T> : Location
    {

        public T Constant { get; set; }

        public override ICollection<Variable> GetOperands()
        {
            return null;
        }

        public override int OperandCount()
        {
            return 0;
        }

        public override bool HasOperands()
        {
            return false;
        }

        public override string Description
        {
            get
            {
                return "" + Constant;
            }
        }

    }
}
