using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aramani.IR.Variables;

namespace Aramani.IR.Commands
{


    public class Return : Command
    {


        public Return()
            : base()
        {
        }

        public override string Description
        {
            get
            {
                return
                    "return";
            }
        }
    }
}
