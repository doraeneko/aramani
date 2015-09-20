using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aramani.IR.Variables
{
   

    public class FieldVariable : Variable
    {
        Mono.Cecil.FieldReference cilField;

        public override string Description
        {
            get
            {
                return "." + cilField + "";
            }
        }
    }
}
