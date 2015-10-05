using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aramani.IR.Commands;


namespace Aramani.ILTransformer
{
    public class ILToIRTransformer
    {

        class CommandPair
        {
            public Command irCommand;
            public Mono.Cecil.Cil.Instruction ilInstruction;
          
            public CommandPair(Command irCmd, Mono.Cecil.Cil.Instruction ilInstr)
            {
                irCommand = irCmd;
                ilInstruction = ilInstr;
            }

            public override string ToString()
            {
                var result = "";
                result += "$ " + ilInstruction.ToString() + "\n";
                result += irCommand.Description;
                return result;
            }
        }

        private List<CommandPair> commandList;
        private VariableFactory variables;
        private ILLocationsToIR ILToIr;
        private BasicBlocks basicBlocks;

        private void LoadArg(Mono.Cecil.Cil.Instruction instruction, int index)
        {
            var stackVar = variables.PushFreshVariable();
            var target = new VariableLocation(variables.GetParameter(0));
            var command = new ReceiveDirect(stackVar, target);
            CommandPair pair = new CommandPair(command, instruction);
            Console.Write(command.Description);
            ILToIr.Add(instruction, command);
        }

        private void LoadArgAddress(Mono.Cecil.Cil.Instruction instruction, int index)
        {
            var stackVar = variables.PushFreshVariable();
            var target = new VariableLocation(variables.GetParameter(0));
            var command = new ReceiveIndirect(stackVar, target);
            CommandPair pair = new CommandPair(command, instruction);
            Console.Write(command.Description);
            ILToIr.Add(instruction, command);
        }

        private void StoreToLocal(Mono.Cecil.Cil.Instruction instruction, int index)
        {
            var stackVar = variables.PopVariable();
            var target = new VariableLocation(variables.GetLocalVariable(index));
            var command = new SetDirect(stackVar, target);
            CommandPair pair = new CommandPair(command, instruction);
            Console.Write(command.Description);
            ILToIr.Add(instruction, command);
        }

        private void LoadLocal(Mono.Cecil.Cil.Instruction instruction, int index)
        {
            var stackVar = variables.PushFreshVariable();
            var target = new VariableLocation(variables.GetLocalVariable(index));
            var command = new ReceiveDirect(stackVar, target);
            CommandPair pair = new CommandPair(command, instruction);
            Console.Write(command.Description);
            ILToIr.Add(instruction, command);
        }

        private void LoadLocalAddress(Mono.Cecil.Cil.Instruction instruction, int index)
        {
            var stackVar = variables.PushFreshVariable();
            var target = new VariableLocation(variables.GetLocalVariable(0));
            var command = new ReceiveIndirect(stackVar, target);
            CommandPair pair = new CommandPair(command, instruction);
            Console.Write(command.Description);
            ILToIr.Add(instruction, command);
        }

        private void LoadConstant<T>(Mono.Cecil.Cil.Instruction instruction, T constant)
        {
            var stackVar = variables.PushFreshVariable();
            var constantLocation = new ConstantLocation<T>(constant);
            var command = new ReceiveDirect(stackVar, constantLocation);
            CommandPair pair = new CommandPair(command, instruction);
            Console.WriteLine(command.Description);
            ILToIr.Add(instruction, command);
        }

        private void StoreArgument(Mono.Cecil.Cil.Instruction instruction, int index)
        {
            var stackVar = variables.PopVariable();
            var target = new VariableLocation(variables.GetParameter(index));
            var command = new SetDirect(stackVar, target);
            CommandPair pair = new CommandPair(command, instruction);
            Console.Write(command.Description);
            ILToIr.Add(instruction, command);
        }

        private void Duplicate(Mono.Cecil.Cil.Instruction instruction)
        {
            var stackVar1 = variables.TopVariable();
            var stackVar2 = variables.PushFreshVariable();
            var target = new VariableLocation(stackVar2);
            var command = new SetDirect(stackVar1, target);
            CommandPair pair = new CommandPair(command, instruction);
            Console.Write(command.Description);
            ILToIr.Add(instruction, command);
        }

        private void BranchUnconditionally(Mono.Cecil.Cil.Instruction instruction)
        {
            var command = new BranchUnconditional();
            basicBlocks.AddBasicBlockEntry(instruction, command);   
            Console.Write(command.Description);
            ILToIr.Add(instruction, command);
        }

        public void TransformMethod(Mono.Cecil.MethodDefinition method)
        {
            variables = new VariableFactory();

            ILToIr = new ILLocationsToIR();
            basicBlocks = new BasicBlocks();

            if (method.Body == null || method.Body.Instructions == null)
            {
                Console.WriteLine("Warning: No method body in method {0}.", method);
                return;
            }
            foreach (var instruction in method.Body.Instructions)
            {
                switch (instruction.OpCode.Code)
                {

                    case Mono.Cecil.Cil.Code.Ldarg_0:
                        LoadArg(instruction, 0);
                        break;
                    case Mono.Cecil.Cil.Code.Ldarg_1:
                        LoadArg(instruction, 1);
                        break;
                    case Mono.Cecil.Cil.Code.Ldarg_2:
                        LoadArg(instruction, 2);
                        break;
                    case Mono.Cecil.Cil.Code.Ldarg_3:
                        LoadArg(instruction, 3);
                        break;
                    case Mono.Cecil.Cil.Code.Ldarg:
                    case Mono.Cecil.Cil.Code.Ldarg_S:
                        LoadArg(instruction,
                                ((Mono.Cecil.ParameterDefinition)instruction.Operand).Index);
                        break;

                    case Mono.Cecil.Cil.Code.Ldarga:
                    case Mono.Cecil.Cil.Code.Ldarga_S:
                        LoadArgAddress(instruction,
                                ((Mono.Cecil.ParameterDefinition)instruction.Operand).Index);
                       break;

                    case Mono.Cecil.Cil.Code.Stloc_0:
                        StoreToLocal(instruction, 0);
                        break;
                    case Mono.Cecil.Cil.Code.Stloc_1:
                        StoreToLocal(instruction, 1);
                        break;
                    case Mono.Cecil.Cil.Code.Stloc_2:
                        StoreToLocal(instruction, 2);
                        break;
                    case Mono.Cecil.Cil.Code.Stloc_3:
                        StoreToLocal(instruction, 3);
                        break;
                    case Mono.Cecil.Cil.Code.Stloc_S:
                    case Mono.Cecil.Cil.Code.Stloc:
                        StoreToLocal(instruction, ((Mono.Cecil.Cil.VariableDefinition)instruction.Operand).Index);
                        break;

                    case Mono.Cecil.Cil.Code.Ldloc_0:
                        LoadLocal(instruction, 0);
                        break;
                    case Mono.Cecil.Cil.Code.Ldloc_1:
                        LoadLocal(instruction, 1);
                        break;
                    case Mono.Cecil.Cil.Code.Ldloc_2:
                        LoadLocal(instruction, 2);
                        break;
                    case Mono.Cecil.Cil.Code.Ldloc_3:
                        LoadLocal(instruction, 3);
                        break;
                    case Mono.Cecil.Cil.Code.Ldloc_S:
                    case Mono.Cecil.Cil.Code.Ldloc:
                        LoadLocal(instruction,
                             ((Mono.Cecil.Cil.VariableDefinition)instruction.Operand).Index);
                        break;


                    case Mono.Cecil.Cil.Code.Ldloca_S:
                    case Mono.Cecil.Cil.Code.Ldloca:
                         LoadLocalAddress(instruction,
                             ((Mono.Cecil.Cil.VariableDefinition)instruction.Operand).Index);
                        break;

                    case Mono.Cecil.Cil.Code.Ldnull:
                        LoadConstant<object>(instruction, null);
                        break;

                    case Mono.Cecil.Cil.Code.Starg_S:
                    case Mono.Cecil.Cil.Code.Starg:
                        StoreArgument
                            (instruction,
                             ((Mono.Cecil.ParameterDefinition)instruction.Operand).Index);
                        break;

                    case Mono.Cecil.Cil.Code.Ldc_I4_M1:
                        LoadConstant<int>(instruction, -1);
                        break;
                    case Mono.Cecil.Cil.Code.Ldc_I4_0:
                        LoadConstant<int>(instruction, 0);
                        break;
                    case Mono.Cecil.Cil.Code.Ldc_I4_1:
                        LoadConstant<int>(instruction, 1);
                        break;
                    case Mono.Cecil.Cil.Code.Ldc_I4_2:
                        LoadConstant<int>(instruction, 2);
                        break;
                    case Mono.Cecil.Cil.Code.Ldc_I4_3:
                        LoadConstant<int>(instruction, 3);
                        break;
                    case Mono.Cecil.Cil.Code.Ldc_I4_4:
                        LoadConstant<int>(instruction, 4);
                        break;
                    case Mono.Cecil.Cil.Code.Ldc_I4_5:
                        LoadConstant<int>(instruction, 5);
                        break;
                    case Mono.Cecil.Cil.Code.Ldc_I4_6:
                        LoadConstant<int>(instruction, 6);
                        break;
                    case Mono.Cecil.Cil.Code.Ldc_I4_7:
                        LoadConstant<int>(instruction, 7);
                        break;
                    case Mono.Cecil.Cil.Code.Ldc_I4_8:
                        LoadConstant<int>(instruction, 8);
                        break;
                    case Mono.Cecil.Cil.Code.Ldc_I4_S:
                    case Mono.Cecil.Cil.Code.Ldc_I4:
                        // ok?
                        LoadConstant<int>(instruction, (int)(instruction.Operand));
                        break;
                    case Mono.Cecil.Cil.Code.Ldc_I8:
                        LoadConstant<long>(instruction, (long)(instruction.Operand));
                        break;

                    case Mono.Cecil.Cil.Code.Ldc_R4:
                        LoadConstant<float>(instruction, (float)(instruction.Operand));
                        break;

                    case Mono.Cecil.Cil.Code.Ldc_R8:
                        LoadConstant<double>(instruction, (double)(instruction.Operand));
                        break;
                    case Mono.Cecil.Cil.Code.Dup:
                        Duplicate(instruction);
                        break;

                    case Mono.Cecil.Cil.Code.Pop:
                        variables.PopVariable();
                        break;

                    case Mono.Cecil.Cil.Code.Nop:
                        // TODO: Required?
                        break;
                    case Mono.Cecil.Cil.Code.Break:
                        // IGNORE
                        break;

                    case Mono.Cecil.Cil.Code.Br:
                    case Mono.Cecil.Cil.Code.Br_S:
                        BranchUnconditionally(instruction);
                        break;

                    case Mono.Cecil.Cil.Code.Jmp:
                        break;
                    case Mono.Cecil.Cil.Code.Call:
                        break;
                    case Mono.Cecil.Cil.Code.Calli:
                        break;
                    case Mono.Cecil.Cil.Code.Ret:
                        break;
                    case Mono.Cecil.Cil.Code.Brfalse_S:
                        break;
                    case Mono.Cecil.Cil.Code.Brtrue_S:
                        break;
                    case Mono.Cecil.Cil.Code.Beq_S:
                        break;
                    case Mono.Cecil.Cil.Code.Bge_S:
                        break;
                    case Mono.Cecil.Cil.Code.Bgt_S:
                        break;
                    case Mono.Cecil.Cil.Code.Ble_S:
                        break;
                    case Mono.Cecil.Cil.Code.Blt_S:
                        break;
                    case Mono.Cecil.Cil.Code.Bne_Un_S:
                        break;
                    case Mono.Cecil.Cil.Code.Bge_Un_S:
                        break;
                    case Mono.Cecil.Cil.Code.Bgt_Un_S:
                        break;
                    case Mono.Cecil.Cil.Code.Ble_Un_S:
                        break;
                    case Mono.Cecil.Cil.Code.Blt_Un_S:
                        break;

                    case Mono.Cecil.Cil.Code.Brfalse:
                        break;
                    case Mono.Cecil.Cil.Code.Brtrue:
                        break;
                    case Mono.Cecil.Cil.Code.Beq:
                        break;
                    case Mono.Cecil.Cil.Code.Bge:
                        break;
                    case Mono.Cecil.Cil.Code.Bgt:
                        break;
                    case Mono.Cecil.Cil.Code.Ble:
                        break;
                    case Mono.Cecil.Cil.Code.Blt:
                        break;
                    case Mono.Cecil.Cil.Code.Bne_Un:
                        break;
                    case Mono.Cecil.Cil.Code.Bge_Un:
                        break;
                    case Mono.Cecil.Cil.Code.Bgt_Un:
                        break;
                    case Mono.Cecil.Cil.Code.Ble_Un:
                        break;
                    case Mono.Cecil.Cil.Code.Blt_Un:
                        break;
                    case Mono.Cecil.Cil.Code.Switch:
                        break;
                    case Mono.Cecil.Cil.Code.Ldind_I1:
                        break;
                    case Mono.Cecil.Cil.Code.Ldind_U1:
                        break;
                    case Mono.Cecil.Cil.Code.Ldind_I2:
                        break;
                    case Mono.Cecil.Cil.Code.Ldind_U2:
                        break;
                    case Mono.Cecil.Cil.Code.Ldind_I4:
                        break;
                    case Mono.Cecil.Cil.Code.Ldind_U4:
                        break;
                    case Mono.Cecil.Cil.Code.Ldind_I8:
                        break;
                    case Mono.Cecil.Cil.Code.Ldind_I:
                        break;
                    case Mono.Cecil.Cil.Code.Ldind_R4:
                        break;
                    case Mono.Cecil.Cil.Code.Ldind_R8:
                        break;
                    case Mono.Cecil.Cil.Code.Ldind_Ref:
                        break;
                    case Mono.Cecil.Cil.Code.Stind_Ref:
                        break;
                    case Mono.Cecil.Cil.Code.Stind_I1:
                        break;
                    case Mono.Cecil.Cil.Code.Stind_I2:
                        break;
                    case Mono.Cecil.Cil.Code.Stind_I4:
                        break;
                    case Mono.Cecil.Cil.Code.Stind_I8:
                        break;
                    case Mono.Cecil.Cil.Code.Stind_R4:
                        break;
                    case Mono.Cecil.Cil.Code.Stind_R8:
                        break;
                    case Mono.Cecil.Cil.Code.Add:
                        break;
                    case Mono.Cecil.Cil.Code.Sub:
                        break;
                    case Mono.Cecil.Cil.Code.Mul:
                        break;
                    case Mono.Cecil.Cil.Code.Div:
                        break;
                    case Mono.Cecil.Cil.Code.Div_Un:
                        break;
                    case Mono.Cecil.Cil.Code.Rem:
                        break;
                    case Mono.Cecil.Cil.Code.Rem_Un:
                        break;
                    case Mono.Cecil.Cil.Code.And:
                        break;
                    case Mono.Cecil.Cil.Code.Or:
                        break;
                    case Mono.Cecil.Cil.Code.Xor:
                        break;
                    case Mono.Cecil.Cil.Code.Shl:
                        break;
                    case Mono.Cecil.Cil.Code.Shr:
                        break;
                    case Mono.Cecil.Cil.Code.Shr_Un:
                        break;
                    case Mono.Cecil.Cil.Code.Neg:
                        break;
                    case Mono.Cecil.Cil.Code.Not:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_I1:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_I2:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_I4:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_I8:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_R4:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_R8:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_U4:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_U8:
                        break;
                    case Mono.Cecil.Cil.Code.Callvirt:
                        break;
                    case Mono.Cecil.Cil.Code.Cpobj:
                        break;
                    case Mono.Cecil.Cil.Code.Ldobj:
                        break;
                    case Mono.Cecil.Cil.Code.Ldstr:
                        break;
                    case Mono.Cecil.Cil.Code.Newobj:
                        break;
                    case Mono.Cecil.Cil.Code.Castclass:
                        break;
                    case Mono.Cecil.Cil.Code.Isinst:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_R_Un:
                        break;
                    case Mono.Cecil.Cil.Code.Unbox:
                        break;
                    case Mono.Cecil.Cil.Code.Throw:
                        break;
                    case Mono.Cecil.Cil.Code.Ldfld:
                        break;
                    case Mono.Cecil.Cil.Code.Ldflda:
                        break;
                    case Mono.Cecil.Cil.Code.Stfld:
                        break;
                    case Mono.Cecil.Cil.Code.Ldsfld:
                        break;
                    case Mono.Cecil.Cil.Code.Ldsflda:
                        break;
                    case Mono.Cecil.Cil.Code.Stsfld:
                        break;
                    case Mono.Cecil.Cil.Code.Stobj:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_Ovf_I1_Un:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_Ovf_I2_Un:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_Ovf_I4_Un:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_Ovf_I8_Un:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_Ovf_U1_Un:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_Ovf_U2_Un:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_Ovf_U4_Un:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_Ovf_U8_Un:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_Ovf_I_Un:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_Ovf_U_Un:
                        break;
                    case Mono.Cecil.Cil.Code.Box:
                        break;
                    case Mono.Cecil.Cil.Code.Newarr:
                        break;
                    case Mono.Cecil.Cil.Code.Ldlen:
                        break;
                    case Mono.Cecil.Cil.Code.Ldelema:
                        break;
                    case Mono.Cecil.Cil.Code.Ldelem_I1:
                        break;
                    case Mono.Cecil.Cil.Code.Ldelem_U1:
                        break;
                    case Mono.Cecil.Cil.Code.Ldelem_I2:
                        break;
                    case Mono.Cecil.Cil.Code.Ldelem_U2:
                        break;
                    case Mono.Cecil.Cil.Code.Ldelem_I4:
                        break;
                    case Mono.Cecil.Cil.Code.Ldelem_U4:
                        break;
                    case Mono.Cecil.Cil.Code.Ldelem_I8:
                        break;
                    case Mono.Cecil.Cil.Code.Ldelem_I:
                        break;
                    case Mono.Cecil.Cil.Code.Ldelem_R4:
                        break;
                    case Mono.Cecil.Cil.Code.Ldelem_R8:
                        break;
                    case Mono.Cecil.Cil.Code.Ldelem_Ref:
                        break;
                    case Mono.Cecil.Cil.Code.Stelem_I:
                        break;
                    case Mono.Cecil.Cil.Code.Stelem_I1:
                        break;
                    case Mono.Cecil.Cil.Code.Stelem_I2:
                        break;
                    case Mono.Cecil.Cil.Code.Stelem_I4:
                        break;
                    case Mono.Cecil.Cil.Code.Stelem_I8:
                        break;
                    case Mono.Cecil.Cil.Code.Stelem_R4:
                        break;
                    case Mono.Cecil.Cil.Code.Stelem_R8:
                        break;
                    case Mono.Cecil.Cil.Code.Stelem_Ref:
                        break;
                    case Mono.Cecil.Cil.Code.Ldelem_Any:
                        break;
                    case Mono.Cecil.Cil.Code.Stelem_Any:
                        break;
                    case Mono.Cecil.Cil.Code.Unbox_Any:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_Ovf_I1:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_Ovf_U1:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_Ovf_I2:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_Ovf_U2:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_Ovf_I4:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_Ovf_U4:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_Ovf_I8:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_Ovf_U8:
                        break;
                    case Mono.Cecil.Cil.Code.Refanyval:
                        break;
                    case Mono.Cecil.Cil.Code.Ckfinite:
                        break;
                    case Mono.Cecil.Cil.Code.Mkrefany:
                        break;
                    case Mono.Cecil.Cil.Code.Ldtoken:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_U2:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_U1:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_I:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_Ovf_I:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_Ovf_U:
                        break;
                    case Mono.Cecil.Cil.Code.Add_Ovf:
                        break;
                    case Mono.Cecil.Cil.Code.Add_Ovf_Un:
                        break;
                    case Mono.Cecil.Cil.Code.Mul_Ovf:
                        break;
                    case Mono.Cecil.Cil.Code.Mul_Ovf_Un:
                        break;
                    case Mono.Cecil.Cil.Code.Sub_Ovf:
                        break;
                    case Mono.Cecil.Cil.Code.Sub_Ovf_Un:
                        break;
                    case Mono.Cecil.Cil.Code.Endfinally:
                        break;
                    case Mono.Cecil.Cil.Code.Leave:
                        break;
                    case Mono.Cecil.Cil.Code.Leave_S:
                        break;
                    case Mono.Cecil.Cil.Code.Stind_I:
                        break;
                    case Mono.Cecil.Cil.Code.Conv_U:
                        break;
                    case Mono.Cecil.Cil.Code.Arglist:
                        break;
                    case Mono.Cecil.Cil.Code.Ceq:
                        break;
                    case Mono.Cecil.Cil.Code.Cgt:
                        break;
                    case Mono.Cecil.Cil.Code.Cgt_Un:
                        break;
                    case Mono.Cecil.Cil.Code.Clt:
                        break;
                    case Mono.Cecil.Cil.Code.Clt_Un:
                        break;
                    case Mono.Cecil.Cil.Code.Ldftn:
                        break;
                    case Mono.Cecil.Cil.Code.Ldvirtftn:
                        break;




                    case Mono.Cecil.Cil.Code.Localloc:
                        break;
                    case Mono.Cecil.Cil.Code.Endfilter:
                        break;
                    case Mono.Cecil.Cil.Code.Unaligned:
                        break;
                    case Mono.Cecil.Cil.Code.Volatile:
                        break;
                    case Mono.Cecil.Cil.Code.Tail:
                        break;
                    case Mono.Cecil.Cil.Code.Initobj:
                        break;
                    case Mono.Cecil.Cil.Code.Constrained:
                        break;
                    case Mono.Cecil.Cil.Code.Cpblk:
                        break;
                    case Mono.Cecil.Cil.Code.Initblk:
                        break;
                    case Mono.Cecil.Cil.Code.No:
                        break;
                    case Mono.Cecil.Cil.Code.Rethrow:
                        break;
                    case Mono.Cecil.Cil.Code.Sizeof:
                        break;
                    case Mono.Cecil.Cil.Code.Refanytype:
                        break;
                    case Mono.Cecil.Cil.Code.Readonly:
                        break;
                    default:
                        break;
                }
            }

        }



    }
}
