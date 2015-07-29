using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aramani.IR.Variables;

namespace Aramani.IR.Commands
{
    abstract class Receive : Command, IOperands
    {

        public StackVariable Source { get; set; }

        public Location Target { get; set; }

        public ICollection<Variable> GetOperands()
        {
            var result = new List<Variable>();
            result.AddRange(Target.GetOperands());
            result.Add(Source);
            return result;
        }

        public int OperandCount()
        {
            return Target.OperandCount() + 1;
        }

        public bool HasOperands()
        {
            return true;
        }

        public string Description
        {
            get
            {
                return
                    Source.Description + " := "
                    + Target.Description + ";\n";
            }
        }
    }
}
