using System;
using System.Collections.Generic;

using Aramani.IR.Variables;
using Aramani.Base;

namespace Aramani.IR.Commands
{
    public class AddressOfLocation : Location
    {

        Location innerLocation { get; set; }

        public AddressOfLocation(Location innerLocation)
        {
            this.innerLocation = innerLocation;
        }
        public override ICollection<Variable> GetOperands()
        {
            return innerLocation.GetOperands();
        }

        public override int OperandCount()
        {
            return innerLocation.OperandCount();
        }

        public override bool HasOperands()
        {
            return innerLocation.HasOperands();
        }

        public override string Description {
            get
            {
                return "&(" + innerLocation.Description + ")";
            }
        }

    }
}
