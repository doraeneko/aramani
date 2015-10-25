using System;
using System.Collections.Generic;

using Aramani.IR.Variables;
using Aramani.IR.Routines;

namespace Aramani.IR.Commands
{
    public class NewObject : New
    {
        public NewObject(Routine routine, ICollection<StackVariable> arguments, StackVariable returnVariable)
            : base(routine, arguments, returnVariable)
        {
        }

        public override string Description
        {
            get
            {
                return "(NEW) " + base.Description;
            }
        }
    }
}
