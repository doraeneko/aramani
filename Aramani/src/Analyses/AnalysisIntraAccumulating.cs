using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using CODE = Mono.Cecil.Cil.Code;
using DotNetAnalyser.Domains;


namespace DotNetAnalyser.Analyses
{

    class AnalysisIntraAccumulating<U>
        where U : IDomainElement<U>
    {

        public U Perform
            (MethodDefinition methodDefinition,
             U startValue,
             IEffectComputer<U> transformer)
        {
            if (methodDefinition.Body != null)
            {
                foreach (var instr in methodDefinition.Body.Instructions)
                {
                    startValue.UnionWith(transformer.ComputeEffect(instr, startValue));
                }
            }
            return startValue;
        }
    }

}