using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aramani.IR.Variables
{
    public class LocalVariable : IndexedVariable
    {

        public override string Description
        {
            get
            {
                return "local_" + ID + "";
            }
        }
    }
}
