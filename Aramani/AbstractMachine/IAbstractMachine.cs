using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aramani.AbstractMachine
{

    interface IAbstractMachine
        <STACKDOMAIN, LOCALVARDOMAIN, PARAMETERDOMAIN, FIELDVALUEDOMAIN, INSTANCEDOMAIN>:
        IEvalStack<STACKDOMAIN>, 
        IFields<FIELDVALUEDOMAIN, INSTANCEDOMAIN>,
        ILocalVariables<LOCALVARDOMAIN>,
        IParameters<PARAMETERDOMAIN>
    {

        // additional methods? 

    }

}
