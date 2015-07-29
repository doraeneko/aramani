using System;
using System.Collections.Generic;
using Aramani.Base;

namespace Aramani.IR.Variables
{
    abstract class Variable : IDescription
    {

        public abstract string Description
        {
            get;
        }

    }
}
