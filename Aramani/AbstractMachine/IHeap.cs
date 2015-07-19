using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aramani.AbstractMachine
{

    public interface IHeap<ADDRESSDOMAIN, VALUEDOMAIN> : IToUnknown
    {

        VALUEDOMAIN GetMemoryValue(ADDRESSDOMAIN addr);

        void SetMemoryValue(ADDRESSDOMAIN addr, VALUEDOMAIN value);

    }
}