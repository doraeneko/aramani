using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aramani.IR.Variables;

namespace Aramani.IR.Commands
{
    public class Set : Command
    {
        public StackVariable Source { get; set; }

        public Location Target { get; set; }

        public Set(StackVariable source, Location target)
        {
            Source = source;
            Target = target;
        }

        public override string Description
        {
            get
            {
                return
                    Target.Description + " := "
                    + Source.Description;
            }
        }
    }
}
