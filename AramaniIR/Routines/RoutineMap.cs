using System;
using System.Collections.Generic;

using Aramani.IR.Variables;
using Aramani.IR.Types;

namespace Aramani.IR.Routines
{
    class RoutineMap
    {
        TypeBinder typeClosure;
        Mono.Cecil.MethodDefinition ByteCodeDefinition { get; set; }
        List<BasicBlocks.BasicBlock> Blocks { get; set; }


    }
}
