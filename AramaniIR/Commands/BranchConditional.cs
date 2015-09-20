using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Aramani.IR.Variables;

using Aramani.IR.Commands;
using Aramani.IR.BasicBlocks;

namespace Aramani.IR.Commands
{
    class BranchConditional : Command
    {
        StackVariable Condition;        
        BasicBlock TrueTarget;
        BasicBlock FalseTarget;
    }
}
