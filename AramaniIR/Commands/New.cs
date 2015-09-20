using System;
using System.Collections.Generic;

using Aramani.IR.Variables;
using Aramani.IR.Types;
using Aramani.IR.Routines;

namespace Aramani.IR.Commands
{
    abstract class New : Call
    {
        public StackVariable Target { get; set; }
    }
}
