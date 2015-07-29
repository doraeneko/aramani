using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aramani.IR.Variables;

namespace Aramani.IR.Commands
{
    class SetIndirect : Command
    {

        public StackVariable Source { get; set; }

        public StackVariable Target { get; set; }

    }
}
