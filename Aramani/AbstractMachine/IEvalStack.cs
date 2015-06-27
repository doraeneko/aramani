using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aramani.AbstractMachine
{

    public interface IEvalStack<T>
    {
        void Push(T element);

        T Top();

        T Pop();

    }
}