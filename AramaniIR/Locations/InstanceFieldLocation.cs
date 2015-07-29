using System;
using System.Collections.Generic;

using Aramani.IR.Variables;

namespace Aramani.IR.Commands
{
    class InstanceFieldLocation : Location
    {

        public StackVariable Instance { get; set; }

        public FieldVariable Field { get; set; }

        public override ICollection<Variable> GetOperands()
        {
            return new Variable[] { Instance, Field };
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
                return Instance.Description + "." + Field.Description;
            }
        }
    }
}
