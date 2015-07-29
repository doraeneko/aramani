using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aramani.IR.Variables
{
    class ParameterVariable : IndexedVariable
    {

        public override string Description
        {
            get
            {
                return "parameter[" + ID + "]";
            }
        }
    }
}
