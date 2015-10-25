using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aramani.IR.Variables;

namespace Aramani.IR.Commands
{
    public class BinaryOperation : Command
    {

        public enum BinaryOp
        {
            UNKNOWN,
            ADD,
            DIV,
            REM,
            SUB,
            MUL,
            SHL,
            SHR,
            EQ,
            NEQ,
            LEQ,
            LE,
            GEQ,
            GE
            // TODO
        }

        public BinaryOperation(StackVariable target, StackVariable first, BinaryOp kind, StackVariable second)
        {
            Kind = kind;
            FirstOperand = first;
            SecondOperand = second;
            Target = target;
        }

        public BinaryOp Kind { get; set; }

        public StackVariable FirstOperand { get; set; }

        public StackVariable SecondOperand { get; set; }

        public StackVariable Target { get; set; }

        public override string Description
        {
            get
            {
                return 
                    Target.Description + " := " 
                    + FirstOperand.Description + " " + Kind + " "
                    + SecondOperand.Description;
            }
        }
    }
}
