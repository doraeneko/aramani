using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;


namespace Aramani
{
    public static class StackHelper
    {
        public static bool IsLoadInstruction(this Instruction instruction)
        {
            switch (instruction.OpCode.Code)
            {
                case Code.Starg_S:
                case Code.Stloc_0:
                case Code.Stloc_1:
                case Code.Stloc_2:
                case Code.Stloc_3:
                case Code.Nop:
                case Code.Break:
                    break;
                case Code.Ldc_I4_M1:
                case Code.Ldc_I4_0:
                case Code.Ldc_I4_1:
                case Code.Ldc_I4_2:
                case Code.Ldc_I4_3:
                case Code.Ldc_I4_4:
                case Code.Ldc_I4_5:
                case Code.Ldc_I4_6:
                case Code.Ldc_I4_7:
                case Code.Ldc_I4_8:
                case Code.Ldc_I4_S:
                case Code.Ldc_I4:
                case Code.Ldc_I8:
                case Code.Ldc_R4:
                case Code.Ldc_R8:
                case Code.Ldarg_0:
                case Code.Ldarg_1:
                case Code.Ldarg_2:
                case Code.Ldarg_3:
                case Code.Ldloc_0:
                case Code.Ldloc_1:
                case Code.Ldloc_2:
                case Code.Ldloc_3:
                case Code.Ldarg_S:
                case Code.Ldarga_S:
                case Code.Ldloc_S:
                case Code.Ldloca_S:
                case Code.Ldnull:
                case Code.Ldind_I1:
                case Code.Ldind_U1:
                case Code.Ldind_I2:
                case Code.Ldind_U2:
                case Code.Ldind_I4:
                case Code.Ldind_U4:
                case Code.Ldind_I8:
                case Code.Ldind_I:
                case Code.Ldind_R4:
                case Code.Ldind_R8:
                case Code.Ldind_Ref:
                case Code.Ldfld:
                case Code.Ldflda:
                case Code.Ldstr:
                case Code.Ldobj:
                case Code.Ldftn:
                case Code.Ldvirtftn:
                case Code.Ldarg:
                case Code.Ldarga:
                case Code.Ldloc:
                case Code.Ldloca:
                case Code.Ldsfld:
                case Code.Ldsflda:
                case Code.Ldlen:
                case Code.Ldelema:
                case Code.Ldelem_I1:
                case Code.Ldelem_U1:
                case Code.Ldelem_I2:
                case Code.Ldelem_U2:
                case Code.Ldelem_I4:
                case Code.Ldelem_U4:
                case Code.Ldelem_I8:
                case Code.Ldelem_I:
                case Code.Ldelem_R4:
                case Code.Ldelem_R8:
                case Code.Ldelem_Ref:
                case Code.Ldelem_Any:
                    return true;
                case Code.Stelem_I:
                    break;
                case Code.Stloc_S:
                    break;
                case Code.Dup:
                    break;
                case Code.Pop:
                    break;
                case Code.Jmp:
                    break;
                case Code.Call:
                    break;
                case Code.Calli:
                    break;
                case Code.Ret:
                    break;
                case Code.Br_S:
                    break;
                case Code.Brfalse_S:
                    break;
                case Code.Brtrue_S:
                    break;
                case Code.Beq_S:
                    break;
                case Code.Bge_S:
                    break;
                case Code.Bgt_S:
                    break;
                case Code.Ble_S:
                    break;
                case Code.Blt_S:
                    break;
                case Code.Bne_Un_S:
                    break;
                case Code.Bge_Un_S:
                    break;
                case Code.Bgt_Un_S:
                    break;
                case Code.Ble_Un_S:
                    break;
                case Code.Blt_Un_S:
                    break;
                case Code.Br:
                    break;
                case Code.Brfalse:
                    break;
                case Code.Brtrue:
                    break;
                case Code.Beq:
                    break;
                case Code.Bge:
                    break;
                case Code.Bgt:
                    break;
                case Code.Ble:
                    break;
                case Code.Blt:
                    break;
                case Code.Bne_Un:
                    break;
                case Code.Bge_Un:
                    break;
                case Code.Bgt_Un:
                    break;
                case Code.Ble_Un:
                    break;
                case Code.Blt_Un:
                    break;
                case Code.Switch:
                    break;

                case Code.Stind_Ref:
                case Code.Stind_I1:
                case Code.Stind_I2:
                case Code.Stind_I4:
                case Code.Stind_I8:
                case Code.Stind_R4:
                case Code.Stind_R8:
                    break;
                case Code.Add:
                    break;
                case Code.Sub:
                    break;
                case Code.Mul:
                    break;
                case Code.Div:
                    break;
                case Code.Div_Un:
                    break;
                case Code.Rem:
                    break;
                case Code.Rem_Un:
                    break;
                case Code.And:
                    break;
                case Code.Or:
                    break;
                case Code.Xor:
                    break;
                case Code.Shl:
                    break;
                case Code.Shr:
                    break;
                case Code.Shr_Un:
                    break;
                case Code.Neg:
                    break;
                case Code.Not:
                    break;
                case Code.Conv_I1:
                    break;
                case Code.Conv_I2:
                    break;
                case Code.Conv_I4:
                    break;
                case Code.Conv_I8:
                    break;
                case Code.Conv_R4:
                    break;
                case Code.Conv_R8:
                    break;
                case Code.Conv_U4:
                    break;
                case Code.Conv_U8:
                    break;
                case Code.Callvirt:
                    break;
                case Code.Cpobj:
                    break;

                    break;

                    break;
                case Code.Newobj:
                    break;
                case Code.Castclass:
                    break;
                case Code.Isinst:
                    break;
                case Code.Conv_R_Un:
                    break;
                case Code.Unbox:
                    break;
                case Code.Throw:
                    break;
                case Code.Stfld:
                    break;
                case Code.Stsfld:
                    break;
                case Code.Stobj:
                    break;
                case Code.Conv_Ovf_I1_Un:
                    break;
                case Code.Conv_Ovf_I2_Un:
                    break;
                case Code.Conv_Ovf_I4_Un:
                    break;
                case Code.Conv_Ovf_I8_Un:
                    break;
                case Code.Conv_Ovf_U1_Un:
                    break;
                case Code.Conv_Ovf_U2_Un:
                    break;
                case Code.Conv_Ovf_U4_Un:
                    break;
                case Code.Conv_Ovf_U8_Un:
                    break;
                case Code.Conv_Ovf_I_Un:
                    break;
                case Code.Conv_Ovf_U_Un:
                    break;
                case Code.Box:
                    break;
                case Code.Newarr:
                    break;
//                    break;
                case Code.Stelem_I1:
                    break;
                case Code.Stelem_I2:
                    break;
                case Code.Stelem_I4:
                    break;
                case Code.Stelem_I8:
                    break;
                case Code.Stelem_R4:
                    break;
                case Code.Stelem_R8:
                    break;
                case Code.Stelem_Ref:
                    break;

                case Code.Stelem_Any:
                    break;
                case Code.Unbox_Any:
                    break;
                case Code.Conv_Ovf_I1:
                    break;
                case Code.Conv_Ovf_U1:
                    break;
                case Code.Conv_Ovf_I2:
                    break;
                case Code.Conv_Ovf_U2:
                    break;
                case Code.Conv_Ovf_I4:
                    break;
                case Code.Conv_Ovf_U4:
                    break;
                case Code.Conv_Ovf_I8:
                    break;
                case Code.Conv_Ovf_U8:
                    break;
                case Code.Refanyval:
                    break;
                case Code.Ckfinite:
                    break;
                case Code.Mkrefany:
                    break;
                case Code.Ldtoken:
                    break;
                case Code.Conv_U2:
                    break;
                case Code.Conv_U1:
                    break;
                case Code.Conv_I:
                    break;
                case Code.Conv_Ovf_I:
                    break;
                case Code.Conv_Ovf_U:
                    break;
                case Code.Add_Ovf:
                    break;
                case Code.Add_Ovf_Un:
                    break;
                case Code.Mul_Ovf:
                    break;
                case Code.Mul_Ovf_Un:
                    break;
                case Code.Sub_Ovf:
                    break;
                case Code.Sub_Ovf_Un:
                    break;
                case Code.Endfinally:
                    break;
                case Code.Leave:
                    break;
                case Code.Leave_S:
                    break;
                case Code.Stind_I:
                    break;
                case Code.Conv_U:
                    break;
                case Code.Arglist:
                    break;
                case Code.Ceq:
                    break;
                case Code.Cgt:
                    break;
                case Code.Cgt_Un:
                    break;
                case Code.Clt:
                    break;
                case Code.Clt_Un:
                    break;
                case Code.Starg:
                    break;

                case Code.Stloc:
                    break;
                case Code.Localloc:
                    break;
                case Code.Endfilter:
                    break;
                case Code.Unaligned:
                    break;
                case Code.Volatile:
                    break;
                case Code.Tail:
                    break;
                case Code.Initobj:
                    break;
                case Code.Constrained:
                    break;
                case Code.Cpblk:
                    break;
                case Code.Initblk:
                    break;
                case Code.No:
                    break;
                case Code.Rethrow:
                    break;
                case Code.Sizeof:
                    break;
                case Code.Refanytype:
                    break;
                case Code.Readonly:
                    break;
                default:
                    break;
            }
            return false;
        }


    }
}
