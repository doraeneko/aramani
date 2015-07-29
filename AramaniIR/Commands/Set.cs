using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aramani.IR.Variables;

namespace Aramani.IR.Commands
{
    class Set : Command
    {

        public Location Source { get; set; }

        public StackVariable Target { get; set; }

    }
}
