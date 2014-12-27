using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using CODE = Mono.Cecil.Cil.Code;
using DotNetAnalyser.Domains;


namespace DotNetAnalyser.Analyser
{

    class AnalysisInterAccumulating<U>
        where U : IDomainElement<U>
    {

        public U Perform
            (MethodDefinition initialMethodDefinition,
             U startValue,
             IEffectComputer<U> transformer)
        {
            var value = (U)startValue.Clone();

            foreach (var instr in initialMethodDefinition.Body.Instructions)
            {
                startValue.UnionWith(transformer.ComputeEffect(instr, startValue));
            }
            return startValue;
        }
    }

}