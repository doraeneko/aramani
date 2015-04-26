using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetAnalyser.Domains
{
    class MethodFrame<S,L,P> : ReducedProduct<AbstractEvalStack<S>,L,P>
        where S : class, IDomainElement<S>, new()
        where L : class, IDomainElement<L>, new()
        where P : class, IDomainElement<P>, new()
    {

        #region /* Components */

        AbstractEvalStack<S> EvalStack { get { return this.Component1; } }
        L localVariables { get { return this.Component2; }}
        P parameters { get { return this.Component3; } }

        #endregion


        public MethodFrame
            (Mono.Cecil.MethodDefinition method,
             L initialLocalVariablesValues,
             P initialParametersValues)
            : base(new AbstractEvalStack<S>(method.Body.MaxStackSize), 
                   initialLocalVariablesValues, 
                   initialParametersValues)
        {

        }

    }
}
