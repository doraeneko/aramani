using System;
using System.Collections.Generic;

using Aramani.IR.Variables;
using Aramani.Base;

namespace Aramani.IR.Commands
{
    abstract class Location : IDescription, IOperands
    {

        bool isIndirectAccess;
        public bool IsIndirectAccess 
        { 
            get { return isIndirectAccess; } 
        }


        public abstract ICollection<Variable> GetOperands();

        public abstract int OperandCount();

        public abstract bool HasOperands();

        public abstract string Description { get; }

    }
}
