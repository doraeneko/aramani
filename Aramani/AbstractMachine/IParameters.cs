using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aramani.AbstractMachine
{

    public interface IParameters<PARAMETERDOMAIN> : IToUnknown
    {

        void SetParameter(int index, PARAMETERDOMAIN newValue);

        PARAMETERDOMAIN GetParameter(int index);

    }
}