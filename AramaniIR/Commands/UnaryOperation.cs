using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aramani.IR.Variables;

namespace Aramani.IR.Commands
{
    public class UnaryOperation : Command
    {

        public enum UnaryOp
        {
            UNKNOWN,
            ID,
            NEG,
            NOT,
            ISNULL,
            ISNOTNULL,
            // TODO
        }

        public UnaryOperation(StackVariable target, UnaryOp kind, StackVariable operand)
        {
            Kind = kind;
            Operand = operand;
            Target = target;
        }

        public UnaryOp Kind { get; set; }

        public StackVariable Operand { get; set; }

        public StackVariable Target { get; set; }

        public override string Description
        {
            get
            {
                return
                    Target.Description + " := "
                    + Kind + " " 
                    + Operand.Description + ";\n";
            }
        }

    }
}
