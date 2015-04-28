/*
 * 
 * 
 */
using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using CODE = Mono.Cecil.Cil.Code;
using DotNetAnalyser.Domains;

namespace DotNetAnalyser.Analyses
{
    class LocalCallAnalysis : IEffectComputer<ReferenceSet<MethodDefinition>>
    {

        void AddOperandIfMethodReference(ReferenceSet<MethodDefinition> domainElement, object operand)
        {
            var operandAsMethodReference = operand as MethodReference;
            if (operandAsMethodReference != null)
            {
                // operandAsMethodReference = operandAsMethodReference.Resolve();
                var resolvedMethod = operandAsMethodReference.Resolve();
                if (resolvedMethod != null)
                    domainElement.Add(resolvedMethod);

            }
        }

        public void ComputeEffect(Mono.Cecil.Cil.Instruction instruction)
        {
            var opCode = instruction.OpCode;
            switch (opCode.Code)
            {
                case CODE.Call:
                case CODE.Callvirt:
                case CODE.Newobj:
                case CODE.Ldftn:
                   
                    break;
                default:
                    break;
            }

        }
    }
}