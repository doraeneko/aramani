using System;
using System.Collections.Generic;

using Aramani.IR.Variables;

namespace Aramani.IR.Commands
{
    abstract class New : Command
    {

        public StackVariable Target { get; set; }

    }
}
