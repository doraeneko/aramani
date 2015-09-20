using System;
using System.Collections.Generic;

using Aramani.IR.Variables;

namespace Aramani.IR.Commands
{
    public class VariableLocation : Location
    {

        public Variable SourceVariable { get; set; }

        public VariableLocation (Variable sourceVariable)
        {
            SourceVariable = sourceVariable;
        }

        public override ICollection<Variable> GetOperands()
        {
            return new Variable[] { SourceVariable };
        }

        public override int OperandCount()
        {
            return 1;    
        }

        public override bool HasOperands()
        {
            return true;
        }

        public override string Description
        {
            get
            {
                return SourceVariable.Description;
            }
        }
    }
}
