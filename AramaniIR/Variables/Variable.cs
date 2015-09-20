using System;
using System.Collections.Generic;
using Aramani.Base;

namespace Aramani.IR.Variables
{
    public abstract class Variable : IDescription
    {

        public Aramani.IR.Types.GroundType Type { get; set; }

        public abstract string Description
        {
            get;
        }

    }
}
