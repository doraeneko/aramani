using System;
using System.Collections.Generic;
using Aramani.Base;
using Aramani.IR.Variables;

namespace Aramani.IR.Commands
{
    abstract class FunctionAddressConstant : Location
    {

        // todo
        public object Constant { get; set; }

        public virtual ICollection<Variable> GetOperators()
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
                return "&" + Constant;
            }
        }
    }
}
