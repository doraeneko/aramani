using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;


namespace DotNetAnalyser.Domains
{

    class AbstractEvalStack<T> : AbstractTuple<T>
        where T: class, IDomainElement<T>, new()
    {

        int stackPointer;
        
        public AbstractEvalStack(int size)
            : base(size)
        {
            stackPointer = 0;
        }

        public void Push(T element)
        {
            // TODO: Checks
            this[stackPointer++] = (T)element.Clone();
        }

        public T Pop()
        {
            // TODO: Checks
            var result = this[stackPointer];
            this[stackPointer] = result.CreateTopElement();
            stackPointer--;
            return result;
        }
     
    }


}

