using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aramani.IR.Variables;

namespace Aramani.IR.Commands
{
    public abstract class Receive : Command, IOperands
    {

        public virtual bool IsIndirectAccess
        {
            get { return false; }
        }

        public StackVariable Target { get; set; }

        public Location Source { get; set; }

        public Receive(StackVariable target, Location source)
        {
            Target = target;
            Source = source;
        }

        public ICollection<Variable> GetOperands()
        {
            var result = new List<Variable>();

            result.Add(Target);
            result.AddRange(Source.GetOperands());
            return result;
        }

        public int OperandCount()
        {
            return Source.OperandCount() + 1;
        }

        public bool HasOperands()
        {
            return true;
        }

        public override string Description
        {
            get
            {
                return
                    Target.Description + " := "
                    + (IsIndirectAccess ? "&" : "")
                    + Source.Description 
                    + "\n";
            }
        }
    }
}
