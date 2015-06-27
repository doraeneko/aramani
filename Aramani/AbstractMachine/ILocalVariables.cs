using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aramani.AbstractMachine
{

    public interface ILocalVariables<T>
    {

        T SetLocalVariable(int offset, T newValue);

        T GetLocalVariable(int offset);
         
    }
}