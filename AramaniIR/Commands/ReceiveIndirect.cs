using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aramani.IR.Variables;

namespace Aramani.IR.Commands
{
    public class ReceiveIndirect : Receive
    {
        public ReceiveIndirect(StackVariable target, Location source)
            : base(target, source)
        {
        }

        public override bool IsIndirectAccess
        {
            get { return true; }
        }
    }
}
