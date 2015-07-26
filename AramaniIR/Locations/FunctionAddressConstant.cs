using System;
using System.Collections.Generic;

using AramaniIR.Variables;

namespace AramaniIR.Commands
{
    abstract class FunctionAddressConstant : Location
    {

        // todo
        public object Constant { get; set; }

        public virtual ICollection<Variable> GetOperators()
        {
            return null;
        }

        public sealed int OperandCount()
        {
            return 0;
        }

        public sealed bool HasOperators()
        {
            return false;
        }

    }
}
