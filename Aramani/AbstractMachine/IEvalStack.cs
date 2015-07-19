using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aramani.AbstractMachine
{

    public interface IEvalStack<STACKELEMENTDOMAIN> : IToUnknown
    {

        void Push(STACKELEMENTDOMAIN element);

        void PushUnknownElement();

        STACKELEMENTDOMAIN Top();

        STACKELEMENTDOMAIN Pop();

    }
}