using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aramani.IR.Variables;

namespace Aramani.IR.Commands
{
    public class SetIndirect : Set
    {


        public SetIndirect(StackVariable source, Location target)
            : base(source, target)
        { }


        public virtual bool IsIndirectAccess
        {
            get { return true; }
        }
    }
}
