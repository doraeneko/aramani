using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aramani.AbstractMachine
{

    public interface IFields<FIELDVALUEDOMAIN, INSTANCEDOMAIN>
    {

        void SetStaticField(Mono.Cecil.FieldReference fieldRef, FIELDVALUEDOMAIN newValue);
        FIELDVALUEDOMAIN GetStaticField(Mono.Cecil.FieldReference fieldRef);

        void SetInstanceField(Mono.Cecil.FieldReference fieldRef, INSTANCEDOMAIN instance, FIELDVALUEDOMAIN newValue);
        FIELDVALUEDOMAIN GetInstanceField(Mono.Cecil.FieldReference fieldRef, INSTANCEDOMAIN instance);

        FIELDVALUEDOMAIN GetLocalVariable(int index);

    }
}