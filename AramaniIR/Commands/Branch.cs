using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aramani.IR.Variables;

namespace Aramani.IR.Commands
{

    abstract class Branch : Command
    {
    }

    class BranchConditional : Branch
    {
        StackVariable Condition { get; set; }
        BasicBlocks.BasicBlock TrueTarget { get; set; }
        BasicBlocks.BasicBlock FalseTarget { get; set; }

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

    class BranchUnconditional : Branch
    {
        BasicBlocks.BasicBlock Target { get; set; }

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
