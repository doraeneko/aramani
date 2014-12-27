using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil.Cil;


namespace DotNetAnalyser.Domains
{
    interface IEffectComputer<T>
    {
        /// <summary>
        /// Compute the effect of an instruction <paramref name="instr"/>
        /// having <paramref name="input"/> as precondition (or postcondition)
        /// </summary>
        /// <param name="inputs"></param>
        T ComputeEffect(Instruction instr, T input);
       
    }
}
