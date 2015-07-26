using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AramaniIR.Variables;

namespace AramaniIR.Commands
{
    class SetIndirect : Command
    {

        public StackVariable Source { get; set; }

        public StackVariable Target { get; set; }

    }
}
