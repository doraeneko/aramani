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
    class LocalCallAnalysis : IEffectComputer<ReferenceSet<MethodReference>>
    {

        void AddOperandIfMethodReference(ReferenceSet<MethodReference> domainElement, object operand)
        {
            var operandAsMethodReference = operand as MethodReference;
            if (operandAsMethodReference != null)
            {
                // operandAsMethodReference = operandAsMethodReference.Resolve();
                var elementMethod = operandAsMethodReference.GetElementMethod();
                if (elementMethod != null)
                    domainElement.Add(elementMethod);
                else
                    domainElement.Add(operandAsMethodReference);
            }
        }

        public ReferenceSet<MethodReference> ComputeEffect(Mono.Cecil.Cil.Instruction instr, ReferenceSet<MethodReference> input)
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