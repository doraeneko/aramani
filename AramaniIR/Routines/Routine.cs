using System;
using System.Collections.Generic;

using Aramani.IR.Variables;
using Aramani.IR.Types;
using Aramani.Base;

namespace Aramani.IR.Routines
{
    public class Routine : IDescription
    {
        TypeBinder typeClosure;
        Mono.Cecil.MethodDefinition ByteCodeDefinition { get; set; }
        List<BasicBlocks.BasicBlock> Blocks { get; set; }

        public Routine(Mono.Cecil.MethodDefinition bytecodeMethod)
        {
            ByteCodeDefinition = bytecodeMethod;
        }

        public virtual string Description
        {
            get
            {
                // TODO
                return ByteCodeDefinition.FullName;
            }
        }
    }
}
