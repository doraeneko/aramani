using System;
using System.Collections.Generic;

using Aramani.IR.Variables;

namespace Aramani.IR.Commands
{
    class VariableLocation : Location
    {

        public Variable SourceVariable { get; set; }

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
