using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Aramani.IR.Variables;
using Aramani.IR.Routines;

namespace Aramani.IR.Commands
{
    public class Call : Command
    {
        public StackVariable ReturnVariable;
        public ICollection<StackVariable> Arguments; // inclusive this for instance calls
        public Routine Callee;
        public bool IsVirtual { get; set; }

        public Call(Routine routine, ICollection<StackVariable> arguments, StackVariable returnVariable, bool isVirtual)
        {
            Callee = routine;
            Arguments = arguments;
            ReturnVariable = returnVariable;
            IsVirtual = isVirtual;
        }

        public override string Description
        {
            get
            {
                var result = "";
                if (ReturnVariable != null)
                {
                    result = ReturnVariable.Description + " := ";
                }
                result += Callee.Description + "(";
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
                result += ")";
                return result;
            }
        }

    }
}
