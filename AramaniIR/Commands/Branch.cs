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
        public Aramani.IR.BasicBlocks.BasicBlock Target { get; set; }
    }

    public class BranchConditional : Branch
    {
        public StackVariable Condition { get; set; }

        public BranchConditional(StackVariable condition)
            : base()
        {
            Condition = condition;
        }

        public override string Description
        {
            get
            {
                var target = (Target == null) ? "not set" : ("" + Target.ID);
                return
                    "if " + Condition.Description 
                    + " then goto " + target;
            }
        }
    }

    public class BranchUnconditional : Branch
    {

        public BranchUnconditional() 
            : base()
        {
        }

        public override string Description
        {
            get
            {
                var target = (Target == null) ? "not set" : ("" + Target.ID);
                return
                    "goto " + target;
            }
        }
    }
}
