using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aramani.IR.Variables;
using Aramani.IR.Types;

namespace Aramani.IR.Commands
{
    public class Convert : Command
    {
        public StackVariable Source { get; set; }

        public StackVariable Target { get; set; }

        public GroundType TheType { get; set; }

        public bool OverflowCheck { get; set; }

        public Convert(StackVariable source,
                       StackVariable target,
                       GroundType theType,
                       bool overflowCheck)
        {
            Source = source;
            Target = target;
            TheType = theType;
            OverflowCheck = overflowCheck;
        }

        public override string Description
        {
            get
            {
                var convertType = (TheType == null) ? "unknown type" : TheType.ToString();
                var overflow = OverflowCheck ? "[check overflow]" : "";
                return
                    Target.Description + " := (" + convertType + ")"
                    + Source.Description + " " + overflow;
            }
        }
    }
}
