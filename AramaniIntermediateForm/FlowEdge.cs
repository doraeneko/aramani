
using System;
using System.Linq;
using System.Collections.Generic;
using Mono.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

using CODE = Mono.Cecil.Cil.Code;

namespace Aramani.IntermediateForm
{

    public enum FlowEdgeKind
    {
        NORMAL,
        COND_IF,
        COND_ELSE
    }

    public class FlowEdge
    {
        public FlowEdgeKind Kind;
        public BasicBlock Source, Target;
        public Mono.Cecil.Cil.Instruction BranchInstruction;
    }
}