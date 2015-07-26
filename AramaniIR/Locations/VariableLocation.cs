using System;
using System.Collections.Generic;

using AramaniIR.Variables;

namespace AramaniIR.Commands
{
    class VariableLocation : Location
    {

        public Variable SourceVariable { get; set; }

        public virtual ICollection<Variable> GetOperators()
        {
            return new Variable[] { SourceVariable };
        }

        public sealed bool HasOperators()
        {
            return true;
        }
    }
}
