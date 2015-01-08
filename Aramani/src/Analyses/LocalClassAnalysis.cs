using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using CODE = Mono.Cecil.Cil.Code;
using DotNetAnalyser.Domains;

namespace DotNetAnalyser.Analyses
{
    class LocalClassAnalysis : IEffectComputer<AbstractMethodFrame<ReferenceSet<TypeDefinition>>>
    {


        public AbstractMethodFrame<ReferenceSet<TypeDefinition>> ComputeEffect 
               (Mono.Cecil.Cil.Instruction instr, 
                AbstractMethodFrame<ReferenceSet<TypeDefinition>> value)
        {
            var opCode = instr.OpCode;
            switch (opCode.Code)
            {
                case CODE.Nop:
                case CODE.Break:
                    break;
                case CODE.Ldarg_0:
                    value.Stack.Push(value.MethodArguments[0]);
                    break;
                case CODE.Ldarg_1:
                    value.Stack.Push(value.MethodArguments[1]);
                    break;
                case CODE.Ldarg_2:
                    value.Stack.Push(value.MethodArguments[2]);
                    break;
                case CODE.Ldarg_3:
                    value.Stack.Push(value.MethodArguments[3]);
                    break;
                case CODE.Ldloc_0:
                    value.Stack.Push(value.LocalVariables[0]);
                    break;
                case CODE.Ldloc_1:
                    value.Stack.Push(value.LocalVariables[1]);
                    break;
                case CODE.Ldloc_2:
                    value.Stack.Push(value.LocalVariables[2]);
                    break;
                case CODE.Ldloc_3:
                    value.Stack.Push(value.LocalVariables[3]);
                    break;
                case CODE.Stloc_0:
                    value.LocalVariables[0] = 
                        value.Stack.Pop();
                    break;
                case CODE.Stloc_1:
                    value.LocalVariables[1] = 
                        value.Stack.Pop();
                    break;
                case CODE.Stloc_2:
                    value.LocalVariables[2] = 
                        value.Stack.Pop();
                    break;
                case CODE.Stloc_3:
                    value.LocalVariables[3] = 
                        value.Stack.Pop();
                    break;
                case CODE.Ldarg_S:
                    // extract operand
                    value.Stack.Push(value.MethodArguments[opCode.Op1]);
                    break;
                case CODE.Ldarga_S:
                    // extract operand
                    value.Stack.Push(value.MethodArguments[opCode.Op1]);
                    break;
                case CODE.Starg_S:
                    var el1 = value.Stack.Pop();
                    value.MethodArguments(opCode.Op1) = el1;
                    break;
                case CODE.Ldloc_S:
                case CODE.Ldloca_S:
                    value.Stack.Push(value.LocalVariables[opCode.Op1]);
                    break;
                case CODE.Stloc_S:
                    var el1 = value.Stack.Pop();
                    value.LocalVariables(opCode.Op1) = el1;
                    break;
                case CODE.Ldnull:
                    break;
                case CODE.Ldc_I4_M1:
                    break;
                case CODE.Ldc_I4_0:
                    break;
                case CODE.Ldc_I4_1:
                    break;
                case CODE.Ldc_I4_2:
                    break;
                case CODE.Ldc_I4_3:
                    break;
                case CODE.Ldc_I4_4:
                    break;
                case CODE.Ldc_I4_5:
                    break;
                case CODE.Ldc_I4_6:
                    break;
                case CODE.Ldc_I4_7:
                    break;
                case CODE.Ldc_I4_8:
                    break;
                case CODE.Ldc_I4_S:
                    break;
                case CODE.Ldc_I4:
                    break;
                case CODE.Ldc_I8:
                    break;
                case CODE.Ldc_R4:
                    break;
                case CODE.Ldc_R8:
                    break;
                case CODE.Dup:
                    break;
                case CODE.Pop:
                    break;
                case CODE.Jmp:
                    break;
                case CODE.Call:
                    break;
                case CODE.Calli:
                    break;
                case CODE.Ret:
                    break;
                case CODE.Br_S:
                    break;
                case CODE.Brfalse_S:
                    break;
                case CODE.Brtrue_S:
                    break;
                case CODE.Beq_S:
                    break;
                case CODE.Bge_S:
                    break;
                case CODE.Bgt_S:
                    break;
                case CODE.Ble_S:
                    break;
                case CODE.Blt_S:
                    break;
                case CODE.Bne_Un_S:
                    break;
                case CODE.Bge_Un_S:
                    break;
                case CODE.Bgt_Un_S:
                    break;
                case CODE.Ble_Un_S:
                    break;
                case CODE.Blt_Un_S:
                    break;
                case CODE.Br:
                    break;
                case CODE.Brfalse:
                    break;
                case CODE.Brtrue:
                    break;
                case CODE.Beq:
                    break;
                case CODE.Bge:
                    break;
                case CODE.Bgt:
                    break;
                case CODE.Ble:
                    break;
                case CODE.Blt:
                    break;
                case CODE.Bne_Un:
                    break;
                case CODE.Bge_Un:
                    break;
                case CODE.Bgt_Un:
                    break;
                case CODE.Ble_Un:
                    break;
                case CODE.Blt_Un:
                    break;
                case CODE.Switch:
                    break;
                case CODE.Ldind_I1:
                    break;
                case CODE.Ldind_U1:
                    break;
                case CODE.Ldind_I2:
                    break;
                case CODE.Ldind_U2:
                    break;
                case CODE.Ldind_I4:
                    break;
                case CODE.Ldind_U4:
                    break;
                case CODE.Ldind_I8:
                    break;
                case CODE.Ldind_I:
                    break;
                case CODE.Ldind_R4:
                    break;
                case CODE.Ldind_R8:
                    break;
                case CODE.Ldind_Ref:
                    break;
                case CODE.Stind_Ref:
                    break;
                case CODE.Stind_I1:
                    break;
                case CODE.Stind_I2:
                    break;
                case CODE.Stind_I4:
                    break;
                case CODE.Stind_I8:
                    break;
                case CODE.Stind_R4:
                    break;
                case CODE.Stind_R8:
                    break;
                case CODE.Add:
                case CODE.Sub:
                case CODE.Mul:
                case CODE.Div:
                case CODE.Div_Un:
                case CODE.Rem:
                case CODE.Rem_Un:
                case CODE.And:
                case CODE.Or:
                case CODE.Xor:
                case CODE.Shl:
                case CODE.Shr:
                case CODE.Shr_Un:
                case CODE.Neg:
                case CODE.Not:
                    var el1 = value.Stack.Pop();
                    var el2 = value.Stack.Pop();
                    el1.UnionWith(el2);
                    value.Stack.Push(el1);
                    break;
                case CODE.Conv_I1:
                    // NATIVE INT?
                    break;
                case CODE.Conv_I2:
                    break;
                case CODE.Conv_I4:
                    break;
                case CODE.Conv_I8:
                    break;
                case CODE.Conv_R4:
                    break;
                case CODE.Conv_R8:
                    break;
                case CODE.Conv_U4:
                    break;
                case CODE.Conv_U8:
                    break;
                case CODE.Callvirt:
                    break;
                case CODE.Cpobj:
                    break;
                case CODE.Ldobj:
                    break;
                case CODE.Ldstr:
                    break;
                case CODE.Newobj:
                    break;
                case CODE.Castclass:
                    break;
                case CODE.Isinst:
                    break;
                case CODE.Conv_R_Un:
                    break;
                case CODE.Unbox:
                    break;
                case CODE.Throw:
                    break;
                case CODE.Ldfld:
                    break;
                case CODE.Ldflda:
                    break;
                case CODE.Stfld:
                    break;
                case CODE.Ldsfld:
                    break;
                case CODE.Ldsflda:
                    break;
                case CODE.Stsfld:
                    break;
                case CODE.Stobj:
                    break;
                case CODE.Conv_Ovf_I1_Un:
                    break;
                case CODE.Conv_Ovf_I2_Un:
                    break;
                case CODE.Conv_Ovf_I4_Un:
                    break;
                case CODE.Conv_Ovf_I8_Un:
                    break;
                case CODE.Conv_Ovf_U1_Un:
                    break;
                case CODE.Conv_Ovf_U2_Un:
                    break;
                case CODE.Conv_Ovf_U4_Un:
                    break;
                case CODE.Conv_Ovf_U8_Un:
                    break;
                case CODE.Conv_Ovf_I_Un:
                    break;
                case CODE.Conv_Ovf_U_Un:
                    break;
                case CODE.Box:
                    break;
                case CODE.Newarr:
                    break;
                case CODE.Ldlen:
                    break;
                case CODE.Ldelema:
                    break;
                case CODE.Ldelem_I1:
                    break;
                case CODE.Ldelem_U1:
                    break;
                case CODE.Ldelem_I2:
                    break;
                case CODE.Ldelem_U2:
                    break;
                case CODE.Ldelem_I4:
                    break;
                case CODE.Ldelem_U4:
                    break;
                case CODE.Ldelem_I8:
                    break;
                case CODE.Ldelem_I:
                    break;
                case CODE.Ldelem_R4:
                    break;
                case CODE.Ldelem_R8:
                    break;
                case CODE.Ldelem_Ref:
                    break;
                case CODE.Stelem_I:
                    break;
                case CODE.Stelem_I1:
                    break;
                case CODE.Stelem_I2:
                    break;
                case CODE.Stelem_I4:
                    break;
                case CODE.Stelem_I8:
                    break;
                case CODE.Stelem_R4:
                    break;
                case CODE.Stelem_R8:
                    break;
                case CODE.Stelem_Ref:
                    break;
                case CODE.Ldelem_Any:
                    break;
                case CODE.Stelem_Any:
                    break;
                case CODE.Unbox_Any:
                    break;
                case CODE.Conv_Ovf_I1:
                    break;
                case CODE.Conv_Ovf_U1:
                    break;
                case CODE.Conv_Ovf_I2:
                    break;
                case CODE.Conv_Ovf_U2:
                    break;
                case CODE.Conv_Ovf_I4:
                    break;
                case CODE.Conv_Ovf_U4:
                    break;
                case CODE.Conv_Ovf_I8:
                    break;
                case CODE.Conv_Ovf_U8:
                    break;
                case CODE.Refanyval:
                    break;
                case CODE.Ckfinite:
                    break;
                case CODE.Mkrefany:
                    break;
                case CODE.Ldtoken:
                    break;
                case CODE.Conv_U2:
                    break;
                case CODE.Conv_U1:
                    break;
                case CODE.Conv_I:
                    break;
                case CODE.Conv_Ovf_I:
                    break;
                case CODE.Conv_Ovf_U:
                    break;
                case CODE.Add_Ovf:
                    break;
                case CODE.Add_Ovf_Un:
                    break;
                case CODE.Mul_Ovf:
                    break;
                case CODE.Mul_Ovf_Un:
                    break;
                case CODE.Sub_Ovf:
                    break;
                case CODE.Sub_Ovf_Un:
                    break;
                case CODE.Endfinally:
                    break;
                case CODE.Leave:
                    break;
                case CODE.Leave_S:
                    break;
                case CODE.Stind_I:
                    break;
                case CODE.Conv_U:
                    break;
                case CODE.Arglist:
                    break;
                case CODE.Ceq:
                    break;
                case CODE.Cgt:
                    break;
                case CODE.Cgt_Un:
                    break;
                case CODE.Clt:
                    break;
                case CODE.Clt_Un:
                    break;
                case CODE.Ldftn:
                    break;
                case CODE.Ldvirtftn:
                    break;
                case CODE.Ldarg:
                    break;
                case CODE.Ldarga:
                    break;
                case CODE.Starg:
                    break;
                case CODE.Ldloc:
                    break;
                case CODE.Ldloca:
                    break;
                case CODE.Stloc:
                    break;
                case CODE.Localloc:
                    break;
                case CODE.Endfilter:
                    break;
                case CODE.Unaligned:
                    break;
                case CODE.Volatile:
                    break;
                case CODE.Tail:
                    break;
                case CODE.Initobj:
                    break;
                case CODE.Constrained:
                    break;
                case CODE.Cpblk:
                    break;
                case CODE.Initblk:
                    break;
                case CODE.No:
                    break;
                case CODE.Rethrow:
                    break;
                case CODE.Sizeof:
                    break;
                case CODE.Refanytype:
                    break;
                case CODE.Readonly:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return value;
        }
    }
}
