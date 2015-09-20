using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Aramani.IR.Variables;
using Aramani.IR.Routines;

namespace Aramani.IR.Commands
{
    class Call : Command
    {
        public StackVariable ReturnLocation;
        public ICollection<StackVariable> Arguments; // inclusive this for instance calls
        public Routine Callee;

        public override string Description
        {
            get
            {
                var result = ReturnLocation.Description + " := "
                             + Callee.Description + "(";
                if (Arguments != null)
                {
                    bool firstArgument = true;
                    foreach (var arg in Arguments)
                    {
                        if (!firstArgument)
                        {
                            result += " ";
                        }
                        result += arg.Description;
                        firstArgument = false;
                    }
                    result = result.Trim();
                }
                result += ")\n";
                return result;
            }
        }

    }
}
