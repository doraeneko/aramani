using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Aramani.Domains
{

    class MethodFrameDomain<T> : IEffectComputer<MethodFrameDomain<T>>
        where T : IMethodFrameManipulator<T>, IDomainElement<T>
    {

        T innerValue;

        public virtual void ComputeEffect
            (Instruction instruction, 
             bool UseElseBranch = false)
        {
            switch (instruction.OpCode.Code)
            {
                case Code.Nop:
                    break;
                case Code.Break:
                    break;
                case Code.Ldarg_0:
                    innerValue.LoadArgument(0);
                    break;
                case Code.Ldarg_1:
                    innerValue.LoadArgument(1);
                    break;
                case Code.Ldarg_2:
                    innerValue.LoadArgument(2);
                    break;
                case Code.Ldarg_3:
                    innerValue.LoadArgument(3);
                    break;
                case Code.Ldloc_0:
                    innerValue.LoadLocalVariable(0);
                    break;
                case Code.Ldloc_1:
                    innerValue.LoadLocalVariable(1);
                    break;
                case Code.Ldloc_2:
                    innerValue.LoadLocalVariable(2);
                    break;
                case Code.Ldloc_3:
                    innerValue.LoadLocalVariable(3);
                    break;
                case Code.Ldloc:
                    innerValue.LoadLocalVariable((int)instruction.Operand, false);
                    break;
                case Code.Ldloca:
                    innerValue.LoadLocalVariable((int)instruction.Operand, true);
                    break;
                case Code.Stloc_0:
                    innerValue.StoreLocalVariable(0);
                    break;
                case Code.Stloc_1:
                    innerValue.StoreLocalVariable(1);
                    break;
                case Code.Stloc_2:
                    innerValue.StoreLocalVariable(2);
                    break;
                case Code.Stloc_3:
                    innerValue.StoreLocalVariable(3);
                    break;
                case Code.Stloc:
                    innerValue.StoreLocalVariable((int)instruction.Operand);
                    break;
                case Code.Ldarg_S:
                case Code.Ldarg:
                    innerValue.LoadArgument
                        (((Mono.Cecil.ParameterDefinition)instruction.Operand).Index, false);
                    break;
                case Code.Ldarga_S:
                case Code.Ldarga:
                    innerValue.LoadArgument
                        (((Mono.Cecil.ParameterDefinition)instruction.Operand).Index, true);
                    break;
                case Code.Starg_S:
                case Code.Starg:
                    innerValue.StoreArgument
                        (((Mono.Cecil.ParameterDefinition)instruction.Operand).Index);
                    break;
                case Code.Ldloc_S:
                    innerValue.LoadLocalVariable
                        (((Mono.Cecil.ParameterDefinition)instruction.Operand).Index, false);
                    break;
                case Code.Ldloca_S:
                    innerValue.LoadLocalVariable
                        (((Mono.Cecil.ParameterDefinition)instruction.Operand).Index, true);
                    break;
                case Code.Stloc_S:
                    innerValue.StoreLocalVariable
                        (((Mono.Cecil.ParameterDefinition)instruction.Operand).Index);
                    break;
                case Code.Ldnull:
                    innerValue.LoadConstant(0);
                    break;
                case Code.Ldc_I4_M1:
                    innerValue.LoadConstant(-1);
                    break;
                case Code.Ldc_I4_0:
                    innerValue.LoadConstant(0);
                    break;
                case Code.Ldc_I4_1:
                    innerValue.LoadConstant(1);
                    break;
                case Code.Ldc_I4_2:
                    innerValue.LoadConstant(2);
                    break;
                case Code.Ldc_I4_3:
                    innerValue.LoadConstant(3);
                    break;
                case Code.Ldc_I4_4:
                    innerValue.LoadConstant(4);
                    break;
                case Code.Ldc_I4_5:
                    innerValue.LoadConstant(5);
                    break;
                case Code.Ldc_I4_6:
                    innerValue.LoadConstant(6);
                    break;
                case Code.Ldc_I4_7:
                    innerValue.LoadConstant(7);
                    break;
                case Code.Ldc_I4_8:
                    innerValue.LoadConstant(8);
                    break;
                case Code.Ldc_I4_S:
                    innerValue.LoadConstant(((int)instruction.Operand));
                    break;
                case Code.Ldc_I4:
                    innerValue.LoadConstant(((int)instruction.Operand));
                    break;
                case Code.Ldc_I8:
                    innerValue.LoadConstant(((long)instruction.Operand));
                    break;
                case Code.Ldc_R4:
                    innerValue.LoadConstant(((float)instruction.Operand));
                    break;
                case Code.Ldc_R8:
                    innerValue.LoadConstant(((double)instruction.Operand));
                    break;
                case Code.Dup:
                    innerValue.Dup();
                    break;
                case Code.Pop:
                    innerValue.Pop();
                    break;

                case Code.Ldind_I1:
                    innerValue.LoadIndirect(TypeDiscriminant.I1);
                    break;
                case Code.Ldind_U1:
                    innerValue.LoadIndirect(TypeDiscriminant.U1);
                    break;
                case Code.Ldind_I2:
                    innerValue.LoadIndirect(TypeDiscriminant.I2);
                    break;
                case Code.Ldind_U2:
                    innerValue.LoadIndirect(TypeDiscriminant.U2);
                    break;
                case Code.Ldind_I4:
                    innerValue.LoadIndirect(TypeDiscriminant.I4);
                    break;
                case Code.Ldind_U4:
                    innerValue.LoadIndirect(TypeDiscriminant.U4);
                    break;
                case Code.Ldind_I8:
                    innerValue.LoadIndirect(TypeDiscriminant.I8);
                    break;
                case Code.Ldind_I:
                    innerValue.LoadIndirect(TypeDiscriminant.NATIVE_INT);
                    break;
                case Code.Ldind_R4:
                    innerValue.LoadIndirect(TypeDiscriminant.R4);
                    break;
                case Code.Ldind_R8:
                    innerValue.LoadIndirect(TypeDiscriminant.R8);
                    break;
                case Code.Ldind_Ref:
                    innerValue.LoadIndirect(TypeDiscriminant.REF);
                    break;
                case Code.Stind_Ref:
                    innerValue.StoreIndirect(TypeDiscriminant.REF);
                    break;
                case Code.Stind_I1:
                    innerValue.StoreIndirect(TypeDiscriminant.I1);
                    break;
                case Code.Stind_I2:
                    innerValue.StoreIndirect(TypeDiscriminant.I2);
                    break;
                case Code.Stind_I4:
                    innerValue.StoreIndirect(TypeDiscriminant.I4);
                    break;
                case Code.Stind_I8:
                    innerValue.StoreIndirect(TypeDiscriminant.I8);
                    break;
                case Code.Stind_R4:
                    innerValue.StoreIndirect(TypeDiscriminant.R4);
                    break;
                case Code.Stind_R8:
                    innerValue.StoreIndirect(TypeDiscriminant.R8);
                    break;
                case Code.Stind_I:
                    innerValue.StoreIndirect(TypeDiscriminant.NATIVE_INT);
                    break;
                case Code.Add:
                    innerValue.ComputeBinaryOperation(BinaryOperation.ADD, false, false);
                    break;
                case Code.Add_Ovf:
                    innerValue.ComputeBinaryOperation(BinaryOperation.ADD, false, true);
                    break;
                case Code.Add_Ovf_Un:
                    innerValue.ComputeBinaryOperation(BinaryOperation.ADD, true, true);
                    break;
                case Code.Sub:
                    innerValue.ComputeBinaryOperation(BinaryOperation.SUB);
                    break;
                case Code.Sub_Ovf:
                    innerValue.ComputeBinaryOperation(BinaryOperation.SUB, false, true);
                    break;
                case Code.Sub_Ovf_Un:
                    innerValue.ComputeBinaryOperation(BinaryOperation.SUB, true, true);
                    break;
                case Code.Mul:
                    innerValue.ComputeBinaryOperation(BinaryOperation.MUL);
                    break;
                case Code.Mul_Ovf:
                    innerValue.ComputeBinaryOperation(BinaryOperation.MUL, false, true);
                    break;
                case Code.Mul_Ovf_Un:
                    innerValue.ComputeBinaryOperation(BinaryOperation.MUL, true, true);
                    break;
                case Code.Div:
                    innerValue.ComputeBinaryOperation(BinaryOperation.DIV);
                    break;
                case Code.Div_Un:
                    innerValue.ComputeBinaryOperation(BinaryOperation.DIV, true, false);
                    break;
                case Code.Rem:
                    innerValue.ComputeBinaryOperation(BinaryOperation.REM);
                    break;
                case Code.Rem_Un:
                    innerValue.ComputeBinaryOperation(BinaryOperation.REM, true, false);
                    break;
                case Code.And:
                    innerValue.ComputeBinaryOperation(BinaryOperation.AND);
                    break;
                case Code.Or:
                    innerValue.ComputeBinaryOperation(BinaryOperation.OR);
                    break;
                case Code.Xor:
                    innerValue.ComputeBinaryOperation(BinaryOperation.XOR);
                    break;
                case Code.Shl:
                    innerValue.ComputeBinaryOperation(BinaryOperation.SHL);
                    break;
                case Code.Shr:
                    innerValue.ComputeBinaryOperation(BinaryOperation.SHR);
                    break;
                case Code.Shr_Un:
                    innerValue.ComputeBinaryOperation(BinaryOperation.SHR_UNSIGNED);
                    break;
                case Code.Neg:
                    innerValue.ComputeUnaryOperation(UnaryOperation.NEG);
                    break;
                case Code.Not:
                    innerValue.ComputeUnaryOperation(UnaryOperation.NOT);
                    break;
                case Code.Conv_I1:
                    innerValue.Convert(TypeDiscriminant.I1);
                    break;
                case Code.Conv_I2:
                    innerValue.Convert(TypeDiscriminant.I2);
                    break;
                case Code.Conv_I4:
                    innerValue.Convert(TypeDiscriminant.I4);
                    break;
                case Code.Conv_I8:
                    innerValue.Convert(TypeDiscriminant.I8);
                    break;
                case Code.Conv_R4:
                    innerValue.Convert(TypeDiscriminant.R4);
                    break;
                case Code.Conv_R8:
                    innerValue.Convert(TypeDiscriminant.R8);
                    break;
                case Code.Conv_U4:
                    innerValue.Convert(TypeDiscriminant.U4);
                    break;
                case Code.Conv_U8:
                    innerValue.Convert(TypeDiscriminant.U8);
                    break;
                case Code.Conv_U2:
                    innerValue.Convert(TypeDiscriminant.U2);
                    break;
                case Code.Conv_U1:
                    innerValue.Convert(TypeDiscriminant.U1);
                    break;
                case Code.Conv_I:
                    innerValue.Convert(TypeDiscriminant.NATIVE_INT);
                    break;
                case Code.Conv_U:
                    innerValue.Convert(TypeDiscriminant.UNSIGNED_NATIVE_INT);
                    break;
                case Code.Cpobj:
                    innerValue.CopyReference();
                    break;
                case Code.Ldobj:
                    innerValue.LoadValue();
                    break;
                case Code.Ldstr:
                    innerValue.LoadString((string)instruction.Operand);
                    break;
                case Code.Newobj:
                    innerValue.NewObject((MethodReference)instruction.Operand);
                    break;
                case Code.Castclass:
                    innerValue.CastClass((TypeReference)instruction.Operand);
                    break;
                case Code.Isinst:
                    innerValue.IsInst((TypeReference)instruction.Operand);
                    break;
                case Code.Conv_R_Un:
                    // ?
                    innerValue.Convert(TypeDiscriminant.R4);
                    break;
                case Code.Unbox:
                    innerValue.Unbox();
                    break;
                case Code.Throw:
                    innerValue.Throw();
                    break;
                case Code.Rethrow;
                    innerValue.Rethrow();
                    break;
                case Code.Ldfld:
                    innerValue.LoadField((FieldReference)instruction.Operand, false);
                    break;
                case Code.Ldflda:
                    innerValue.LoadField((FieldReference)instruction.Operand, true);
                    break;
                case Code.Stfld:
                    innerValue.StoreField((FieldReference)instruction.Operand);
                    break;
                case Code.Ldsfld:
                    innerValue.LoadField((FieldReference)instruction.Operand, false);
                    break;
                case Code.Ldsflda:
                    innerValue.LoadField((FieldReference)instruction.Operand, true);
                    break;
                case Code.Stsfld:
                    innerValue.StoreField((FieldReference)instruction.Operand);
                    break;
                case Code.Stobj:
                    innerValue.StoreIndirect((TypeReference)instruction.Operand);
                    break;
                case Code.Conv_Ovf_I1_Un:
                    innerValue.Convert(TypeDiscriminant.I1, true, true);
                    break;
                case Code.Conv_Ovf_I2_Un:
                    innerValue.Convert(TypeDiscriminant.I2, true, true);
                    break;
                case Code.Conv_Ovf_I4_Un:
                    innerValue.Convert(TypeDiscriminant.I4, true, true);
                    break;
                case Code.Conv_Ovf_I8_Un:
                    innerValue.Convert(TypeDiscriminant.I8, true, true);
                    break;
                case Code.Conv_Ovf_U1_Un:
                    innerValue.Convert(TypeDiscriminant.U1, true, true);
                    break;
                case Code.Conv_Ovf_U2_Un:
                    innerValue.Convert(TypeDiscriminant.U2, true, true);
                    break;
                case Code.Conv_Ovf_U4_Un:
                    innerValue.Convert(TypeDiscriminant.U4, true, true);
                    break;
                case Code.Conv_Ovf_U8_Un:
                    innerValue.Convert(TypeDiscriminant.U8, true, true);
                    break;
                case Code.Conv_Ovf_I_Un:
                    innerValue.Convert(TypeDiscriminant.NATIVE_INT, true, true);
                    break;
                case Code.Conv_Ovf_U_Un:
                    innerValue.Convert(TypeDiscriminant.UNSIGNED_NATIVE_INT, true, true);
                    break;

                case Code.Conv_Ovf_I1:
                    innerValue.Convert(TypeDiscriminant.I1, false, true);
                    break;
                case Code.Conv_Ovf_U1:
                    innerValue.Convert(TypeDiscriminant.U1, false, true);
                    break;
                case Code.Conv_Ovf_I2:
                    innerValue.Convert(TypeDiscriminant.I2, false, true);
                    break;
                case Code.Conv_Ovf_U2:
                    innerValue.Convert(TypeDiscriminant.U2, false, true);
                    break;
                case Code.Conv_Ovf_I4:
                    innerValue.Convert(TypeDiscriminant.I4, false, true);
                    break;
                case Code.Conv_Ovf_U4:
                    innerValue.Convert(TypeDiscriminant.U4, false, true);
                    break;
                case Code.Conv_Ovf_I8:
                    innerValue.Convert(TypeDiscriminant.I8, false, true);
                    break;
                case Code.Conv_Ovf_U8:
                    innerValue.Convert(TypeDiscriminant.U8, false, true);
                    break;
                case Code.Conv_Ovf_I:
                    innerValue.Convert(TypeDiscriminant.NATIVE_INT, false, true);
                    break;
                case Code.Conv_Ovf_U:
                    innerValue.Convert(TypeDiscriminant.UNSIGNED_NATIVE_INT, false, true);
                    break;
                case Code.Box:
                    innerValue.Box();
                    break;
                case Code.Newarr:
                    innerValue.NewArray((TypeReference)instruction.Operand);
                    break;
                case Code.Ldlen:
                    innerValue.NewArray((TypeReference)instruction.Operand);
                    break;
                case Code.Ldelema:
                    innerValue.LoadArrayElementIndirect((TypeReference)instruction.Operand);
                    break;
                case Code.Ldelem_I1:
                    innerValue.LoadArrayElement(TypeDiscriminant.I1);
                    break;
                case Code.Ldelem_U1:
                    innerValue.LoadArrayElement(TypeDiscriminant.U1);
                    break;
                case Code.Ldelem_I2:
                    innerValue.LoadArrayElement(TypeDiscriminant.I2);
                    break;
                case Code.Ldelem_U2:
                    innerValue.LoadArrayElement(TypeDiscriminant.U2);
                    break;
                case Code.Ldelem_I4:
                    innerValue.LoadArrayElement(TypeDiscriminant.I4);
                    break;
                case Code.Ldelem_U4:
                    innerValue.LoadArrayElement(TypeDiscriminant.U4);
                    break;
                case Code.Ldelem_I8:
                    innerValue.LoadArrayElement(TypeDiscriminant.I8);
                    break;
                case Code.Ldelem_I:
                    innerValue.LoadArrayElement(TypeDiscriminant.NATIVE_INT);
                    break;
                case Code.Ldelem_R4:
                    innerValue.LoadArrayElement(TypeDiscriminant.R4);
                    break;
                case Code.Ldelem_R8:
                    innerValue.LoadArrayElement(TypeDiscriminant.R8);
                    break;
                case Code.Ldelem_Ref:
                    innerValue.LoadArrayElement(TypeDiscriminant.REF);
                    break;
                case Code.Stelem_I:
                    innerValue.StoreArrayElement(TypeDiscriminant.NATIVE_INT);
                    break;
                case Code.Stelem_I1:
                    innerValue.StoreArrayElement(TypeDiscriminant.I1);
                    break;
                case Code.Stelem_I2:
                    innerValue.StoreArrayElement(TypeDiscriminant.I2);
                    break;
                case Code.Stelem_I4:
                    innerValue.StoreArrayElement(TypeDiscriminant.I4);
                    break;
                case Code.Stelem_I8:
                    innerValue.StoreArrayElement(TypeDiscriminant.I8);
                    break;
                case Code.Stelem_R4:
                    innerValue.StoreArrayElement(TypeDiscriminant.R4);
                    break;
                case Code.Stelem_R8:
                    innerValue.StoreArrayElement(TypeDiscriminant.R8);
                    break;
                case Code.Stelem_Ref:
                    innerValue.StoreArrayElement(TypeDiscriminant.REF);
                    break;
                case Code.Ldelem_Any:
                    innerValue.LoadArrayElement(TypeDiscriminant.UNSPECIFIED);
                    break;
                case Code.Stelem_Any:
                    innerValue.StoreArrayElement((TypeReference)instruction.Operand);
                    break;
                case Code.Unbox_Any:
                    innerValue.Unbox((TypeReference)instruction.Operand);
                    break;
                case Code.Ckfinite:
                    innerValue.CheckForFiniteness();
                    break;
                case Code.Localloc:
                    innerValue.LocalAlloc((uint)instruction.Operand);
                    break;
                case Code.Unaligned:
                    innerValue.InstructionPrefix(InstructionPrefix.UNALIGNED);
                    break;
                case Code.Volatile:
                    innerValue.InstructionPrefix(InstructionPrefix.UNALIGNED);
                    break;
                case Code.Tail:
                    innerValue.InstructionPrefix(InstructionPrefix.UNALIGNED);
                    break;
                case Code.Ldftn:
                    innerValue.LoadFunctionPointer(false);
                    break;
                case Code.Ldvirtftn:
                    innerValue.LoadFunctionPointer(true);
                    break;
                case Code.Callvirt:
                    innerValue.Call(false, true);
                    break;
                case Code.Call:
                    innerValue.Call(false, false);
                    break;
                case Code.Calli:
                    innerValue.Call(true);
                    break;

                case Code.Refanyval:
                    innerValue.LoadAddressFromTypedReference((TypeReference)instruction.Operand);
                    break;
                case Code.Mkrefany:
                    innerValue.StoreTypedReference((TypeReference)instruction.Operand);
                    break;
                case Code.Refanytype:
                    innerValue.LoadTypeFromTypedReference();
                    break;
                case Code.Ldtoken:
                    innerValue.LoadMetadataTokenToRuntimeHandle(instruction.Operand);
                    break;
                case Code.Initobj:
                    innerValue.InitObject((TypeReference)instruction.Operand);
                    break;
                case Code.Constrained:
                    innerValue.InstructionPrefix
                        (InstructionPrefix.CONSTRAINED,
                         (TypeReference)instruction.Operand);
                    break;
                case Code.Readonly:
                    innerValue.InstructionPrefix(InstructionPrefix.READONLY);
                    break;

                case Code.Cpblk:
                    innerValue.CopyBlock();
                    break;
                case Code.Initblk:
                    innerValue.InitBlock();
                    break;
                case Code.Sizeof:
                    innerValue.SizeOf((TypeReference)instruction.Operand);
                    break;

                case Code.Ceq:
                    innerValue.ComputeComparison(Comparison.EQ, ComparisonOption.NONE);
                    break;
                case Code.Cgt:
                    innerValue.ComputeComparison(Comparison.GT, ComparisonOption.NONE);
                    break;
                case Code.Cgt_Un:
                    innerValue.ComputeComparison(Comparison.GT, ComparisonOption.UNSIGNED);
                    break;
                case Code.Clt:
                    innerValue.ComputeComparison(Comparison.LT, ComparisonOption.NONE);
                    break;
                case Code.Clt_Un:
                    innerValue.ComputeComparison(Comparison.LT, ComparisonOption.UNSIGNED);
                    break;



                // ***
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


                // ***

                // Control flow
                case Code.Switch:
                    break;
                case Code.Endfilter:
                    break;
                case Code.Jmp:
                    break;
                case Code.Ret:
                    break;
                case Code.Endfinally:
                    break;
                case Code.Leave:
                    break;
                case Code.Leave_S:
                    break;

                // unknown
                case Code.No:
                case Code.Arglist:
                    innerValue.UnknownCommand(instruction);
                    break;
                default:
                    break;
            }
        }
    }

}