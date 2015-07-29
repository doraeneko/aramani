using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aramani.IR.Variables;

namespace Aramani.IR.Commands
{

    interface IOperands
    {
        
        ICollection<Variable> GetOperands();

        int OperandCount();

        bool HasOperands();

    }

}