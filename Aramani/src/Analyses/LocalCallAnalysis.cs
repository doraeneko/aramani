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

namespace DotNetAnalyser.Analyser
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

        public ReferenceSet<MethodDefinition> ComputeEffect(Mono.Cecil.Cil.Instruction instr, ReferenceSet<MethodDefinition> input)
        {
            var opCode = instr.OpCode;
            switch (opCode.Code)
            {
                case CODE.Call:
                case CODE.Callvirt:
                case CODE.Newobj:
                case CODE.Ldftn:
                    AddOperandIfMethodReference(input, instr.Operand);
                    break;
                default:
                    break;
            }
            return input;
        }
    }
}