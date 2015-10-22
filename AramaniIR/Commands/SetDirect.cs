using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aramani.IR.Variables;

namespace Aramani.IR.Commands
{
    public class SetDirect : Set
    {

        public SetDirect(StackVariable source, Location target)
            : base(source, target)
        { }

        public override bool IsIndirectAccess
        {
            get { return false; }
        }

    }
}