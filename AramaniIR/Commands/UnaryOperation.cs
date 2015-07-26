using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AramaniIR.Variables;

namespace AramaniIR.Commands
{
    class UnaryOperation : Command
    {

        public enum UnaryOp
        {
            NEG,
            NOT,
            ISNULL,
            ISNOTNULL,
            // TODO
        }

        public UnaryOp Kind { get; set; }

        public StackVariable Operand { get; set; }

        public StackVariable Target { get; set; }

    }
}
