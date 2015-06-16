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


        public ReferenceSet<TypeDefinition> CreateSingletonInt32()
        {
            return null;
        }

        public ReferenceSet<TypeDefinition> CreateSingletonInt64()
        {
            return null;
        }


        public ReferenceSet<TypeDefinition> CreateSingletonFloat32()
        {
            return null;
        }

        public ReferenceSet<TypeDefinition> CreateSingletonFloat64()
        {
            return null;
        }

        public ReferenceSet<TypeDefinition> CreateSingletonNativeInt()
        {
            return null;
        }


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
                    // TODO
                    break;
                case CODE.Ldc_I4_M1:
                case CODE.Ldc_I4_0:
                case CODE.Ldc_I4_1:
                case CODE.Ldc_I4_2:
                case CODE.Ldc_I4_3:
                case CODE.Ldc_I4_4:
                case CODE.Ldc_I4_5:
                case CODE.Ldc_I4_6:
                case CODE.Ldc_I4_7:
                case CODE.Ldc_I4_8:
                case CODE.Ldc_I4_S:
                case CODE.Ldc_I4:
                    value.Stack.Push(CreateSingletonInt32());
                    break;
                case CODE.Ldc_I8:
                    value.Stack.Push(CreateSingletonInt64());
                    break;
                case CODE.Ldc_R4:
                    value.Stack.Push(CreateSingletonFloat32());
                    break;
                case CODE.Ldc_R8:
                    value.Stack.Push(CreateSingletonFloat64());
                    break;
                case CODE.Dup:
                    var el = value.Stack.Pop();
                    value.Stack.Push(el);
                    value.Stack.Push(el);
                    break;
                case CODE.Pop:
                    value.Stack.Pop();
                    break;
                case CODE.Jmp:
                    // NOP
                    break;
                case CODE.Call:
                    // TODO: get summary
                    break;
                case CODE.Calli:
                    // TODO: get summary
                    break;
                case CODE.Ret:
                case CODE.Br_S:
                case CODE.Brfalse_S:
                case CODE.Brtrue_S:
                case CODE.Beq_S:
                case CODE.Bge_S:
                case CODE.Bgt_S:
                case CODE.Ble_S:
                case CODE.Blt_S:
                case CODE.Bne_Un_S:
                case CODE.Bge_Un_S:
                case CODE.Bgt_Un_S:
                case CODE.Ble_Un_S:
                case CODE.Blt_Un_S:
                case CODE.Br:
                case CODE.Brfalse:
                case CODE.Brtrue:
                case CODE.Beq:
                case CODE.Bge:
                case CODE.Bgt:
                case CODE.Ble:
                case CODE.Blt:
                case CODE.Bne_Un:
                case CODE.Bge_Un:
                case CODE.Bgt_Un:
                case CODE.Ble_Un:
                case CODE.Blt_Un:
                case CODE.Switch:
                    // IGNORE CONTROL FLOW FOR NOW
                    break;
                case CODE.Ldind_I1:

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
