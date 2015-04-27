using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;


namespace DotNetAnalyser.Domains
{
    interface IEffectComputer<T>
        where T : IDomainElement<T>
    {
        /// <summary>
        /// Compute the effect of an instruction <paramref name="instr"/>
        /// having <paramref name="input"/> as precondition (or postcondition)
        /// </summary>
        /// <param name="inputs"></param>
       void ComputeEffect(Instruction instruction);
       
    }

    interface IMethodEffectComputer<T>
        where T : IDomainElement<T>
    {
        /// <summary>
        /// Compute the effect of an instruction <paramref name="instr"/>
        /// having <paramref name="input"/> as precondition (or postcondition)
        /// </summary>
        /// <param name="inputs"></param>
        T ComputeEffect(MethodReference reference, T input);

    }
}
