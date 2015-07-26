using System;
using System.Collections.Generic;

using AramaniIR.Variables;

namespace AramaniIR.Commands
{
    abstract class New : Command
    {

        public StackVariable Target { get; set; }
       
    }
}
