using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aramani.IR.Variables;

namespace Aramani.IR.Commands
{
    public class ReceiveDirect : Receive
    {
        public ReceiveDirect(StackVariable target, Location source)
            : base(target, source)
        {}
        
        public override bool IsIndirectAccess
        {
            get { return false; }
        }

    }
}