using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aramani.IR.Variables;

namespace Aramani.IR.Commands
{

    public abstract class Branch : Command
    {
    }

    public class BranchConditional : Branch
    {
        public StackVariable Condition { get; set; }
        public BasicBlocks.BasicBlock TrueTarget { get; set; }
        public BasicBlocks.BasicBlock FalseTarget { get; set; }

        public BranchConditional()
            : base()
        {

        }

        public override string Description
        {
            get
            {
                return
                    "if " + Condition.Description 
                    + " then goto " + TrueTarget.Description 
                    + " else goto " + FalseTarget.Description + "\n";
            }
        }
    }

    public class BranchUnconditional : Branch
    {
        public BasicBlocks.BasicBlock Target { get; set; }

        public BranchUnconditional() 
            : base()
        {

        }

        public override string Description
        {
            get
            {
                return
                    "goto " + Target.Description + "\n";
            }
        }
    }
}
