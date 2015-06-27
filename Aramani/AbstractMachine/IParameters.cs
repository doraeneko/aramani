using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aramani.AbstractMachine
{

    public interface IParameters<T>
    {

        void SetParameter(int index, T newValue);

        T GetParameter(int index);

    }
}