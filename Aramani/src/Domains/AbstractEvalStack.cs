using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;


namespace DotNetAnalyser.Domains
{

    /// <summary>
    /// Models an eval stack of a method frame.
    /// The top of the stack is always at index 0; this complicates push and pop,
    /// but facilitates the union and join operations (which are the same as 
    /// for normal elements of the tuple domain).
    /// </summary>
    /// <typeparam name="C"></typeparam>
    class AbstractEvalStack<C> : AbstractTuple<C>
        where C: class, IDomainElement<C>, new()
    {

        public AbstractEvalStack(int size)
            : base(size)
        {
        }

        public void Push(C element)
        {
            C buffer = (C)element.Clone();
            for (int i = 0; i < this.Arity; i++)
            {
                var temp = this[i];
                this[i] = buffer;
                buffer = temp;
            }
        }

        public C Pop()
        {
            var result = this[0];
            C buffer = this[0].CreateTopElement();
            for (int i = this.Arity - 1; i >= 0; i--)
            {
                var temp = this[i];
                this[i] = buffer;
                buffer = temp;
            }
            return result;
        }

        public override object Clone()
        {
            var result = new AbstractEvalStack<C>(Arity);
            for (int i = 0; i < Arity; i++)
            {
                result[i] = this[i].Clone() as C;
            }

            return result;
        }
    }
}
