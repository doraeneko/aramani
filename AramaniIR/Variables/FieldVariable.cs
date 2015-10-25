using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aramani.IR.Variables
{
   

    public class FieldVariable : Variable
    {
        Mono.Cecil.FieldReference CilField;

        public FieldVariable(Mono.Cecil.FieldReference cilField)
        {
            CilField = cilField;
        }

        public override string Description
        {
            get
            {
                return "(" + CilField.FullName + ")";
            }
        }
    }
}
