using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AramaniIR.Variables;

namespace AramaniIR.Commands
{

    interface IOperands
    {
        
        ICollection<Variable> GetOperands();

        int OperandCount();

        bool HasOperands();

    }

}