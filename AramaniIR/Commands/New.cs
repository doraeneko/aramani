using System;
using System.Collections.Generic;

using Aramani.IR.Variables;
using Aramani.IR.Types;
using Aramani.IR.Routines;

namespace Aramani.IR.Commands
{
    public class New : Call
    {

        public New(Routine routine, ICollection<StackVariable> arguments, StackVariable returnVariable)
            : base(routine, arguments, returnVariable, false)
        {
        }

        public StackVariable Target { get; set; }
    }
}
