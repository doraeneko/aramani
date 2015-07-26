using System;
using System.Collections.Generic;

using AramaniIR.Variables;

namespace AramaniIR.Commands
{
    class InstanceFieldLocation : Location
    {

        public StackVariable Instance { get; set; }

        public FieldVariable Field { get; set; }

        public virtual ICollection<Variable> GetOperators()
        {
            return new Variable[] { Instance, Field };
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
