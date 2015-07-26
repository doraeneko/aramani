using System;
using System.Collections.Generic;

using AramaniIR.Variables;

namespace AramaniIR.Commands
{
    abstract class Location : IOperands
    {

        bool isIndirectAccess;
        public bool IsIndirectAccess 
        { 
            get { return isIndirectAccess; } 
        }

    }
}
