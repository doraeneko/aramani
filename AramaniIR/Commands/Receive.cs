using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AramaniIR.Variables;

namespace AramaniIR.Commands
{
    abstract class Receive : Command
    {

        public StackVariable Source { get; set; }

        public Location Target { get; set; }

    }
}
