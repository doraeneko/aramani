using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AramaniIR.Variables;

namespace AramaniIR.Commands
{
    class BinaryOperation : Command
    {

        public enum BinaryOp
        {
            ADD,
            DIV,
            REM,
            SUB,
            MUL,
            SHL,
            SHR,
            // TODO
        }

        public BinaryOp Kind { get; set; }

        public StackVariable FirstOperand { get; set; }

        public StackVariable SecondOperand { get; set; }

        public StackVariable Target { get; set; }

    }
}
