using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aramani.AbstractMachine
{

    public interface ILocalVariables<LOCALVARIABLEDOMAIN> : IToUnknown
    {

        LOCALVARIABLEDOMAIN SetLocalVariable(int offset, LOCALVARIABLEDOMAIN newValue);

        LOCALVARIABLEDOMAIN GetLocalVariable(int offset);
         
    }
}