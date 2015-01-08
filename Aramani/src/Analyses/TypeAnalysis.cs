/*
 * 
 * 
 */
using System;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using CODE = Mono.Cecil.Cil.Code;
using DotNetAnalyser.Domains;

namespace DotNetAnalyser.Analyses
{
	class TypeAnalysis : IEffectComputer<TypeSet>
	{

		public TypeSet ComputeEffect(Mono.Cecil.Cil.Instruction instr, TypeSet input)
		{
			var opCode = instr.OpCode;

			switch(opCode.Code) {
			case CODE.Ceq:
				break;
			case CODE.Newobj:
	                        // get the constructor
				var operand = instr.Operand;
				var constructor = operand as Mono.Cecil.MethodReference;
				if (constructor != null) {
					// get the declaring type
					var declaringType = constructor.DeclaringType;
					if (declaringType != null)
						input.Add (declaringType);
				}
				break;
			default:
				break;
			}
			return input;
		}
	}
}