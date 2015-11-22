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

        private List<Command> commandList;
        private VariableFactory variables;
        private ILLocationsToIR ILToIr;
        private BasicBlocks basicBlocks;
        private Dictionary<Mono.Cecil.Cil.Instruction, int> stackHeights;
   


        private Command LoadArg(Mono.Cecil.Cil.Instruction instruction, int index)
        {
            var stackVar = variables.PushFreshVariable();
            var target = new VariableLocation(variables.GetParameter(index));
            var command = new Receive(stackVar, target);
            ILToIr.AddCommand(instruction, command);
            commandList.Add(command);
            basicBlocks.AddCommandToCurrentBasicBlock(command, instruction);
            return command;
        }

        private Command LoadArgAddress(Mono.Cecil.Cil.Instruction instruction, int index)
        {
            var stackVar = variables.PushFreshVariable();
            var target = new AddressOfLocation(new VariableLocation(variables.GetParameter(index)));
            var command = new Receive(stackVar, target);
            ILToIr.AddCommand(instruction, command);
            commandList.Add(command);
            basicBlocks.AddCommandToCurrentBasicBlock(command, instruction);
            return command;
        }

        private Command StoreToLocal(Mono.Cecil.Cil.Instruction instruction, int index)
        {
            var stackVar = variables.PopVariable();
            var target = new VariableLocation(variables.GetLocalVariable(index));
            var command = new Set(stackVar, target);
            ILToIr.AddCommand(instruction, command);
            commandList.Add(command);
            basicBlocks.AddCommandToCurrentBasicBlock(command, instruction);
            return command;
        }

        private Command LoadLocal(Mono.Cecil.Cil.Instruction instruction, int index)
        {
            var stackVar = variables.PushFreshVariable();
            var target = new VariableLocation(variables.GetLocalVariable(index));
            var command = new Receive(stackVar, target);
            ILToIr.AddCommand(instruction, command);
            commandList.Add(command);
            basicBlocks.AddCommandToCurrentBasicBlock(command, instruction);
            return command;
        }

        private Command LoadLocalAddress(Mono.Cecil.Cil.Instruction instruction, int index)
        {
            var stackVar = variables.PushFreshVariable();
            var target = new AddressOfLocation(new VariableLocation(variables.GetLocalVariable(index)));
            var command = new Receive(stackVar, target);
            ILToIr.AddCommand(instruction, command);
            commandList.Add(command);
            basicBlocks.AddCommandToCurrentBasicBlock(command, instruction);
            return command;
        }

        private Command LoadConstant<T>(Mono.Cecil.Cil.Instruction instruction, T constant)
        {
            var stackVar = variables.PushFreshVariable();
            var constantLocation = new ConstantLocation<T>(constant);
            var command = new Receive(stackVar, constantLocation);
            ILToIr.AddCommand(instruction, command);
            commandList.Add(command);
            basicBlocks.AddCommandToCurrentBasicBlock(command, instruction);
            return command;
        }

        private Command StoreArgument(Mono.Cecil.Cil.Instruction instruction, int index)
        {
            var stackVar = variables.PopVariable();
            var target = new VariableLocation(variables.GetParameter(index));
            var command = new Set(stackVar, target);
            ILToIr.AddCommand(instruction, command);
            commandList.Add(command);
            basicBlocks.AddCommandToCurrentBasicBlock(command, instruction);
            return command;
        }

        private Command Duplicate(Mono.Cecil.Cil.Instruction instruction)
        {
            var stackVar1 = variables.TopVariable();
            var stackVar2 = variables.PushFreshVariable();
            var target = new VariableLocation(stackVar2);
            var command = new Set(stackVar1, target);
            ILToIr.AddCommand(instruction, command);
            commandList.Add(command);
            basicBlocks.AddCommandToCurrentBasicBlock(command, instruction);
            return command;
        }

        private Command BranchUnconditionally(Mono.Cecil.Cil.Instruction instruction)
        {
            var command = new BranchUnconditional();
            var target = instruction.Operand as Mono.Cecil.Cil.Instruction;
            if (target == null)
            {
                throw new Exception("No target given for branch instruction.");
            }
            // basicBlocks.AddJumpTarget(command, target);   
            ILToIr.AddCommand(instruction, command);
            commandList.Add(command);
            basicBlocks.AddCommandToCurrentBasicBlock(command, instruction);
            return command;
        }

        private Command BranchBinary(Mono.Cecil.Cil.Instruction instruction, BinaryOperation.BinaryOp binaryOp)
        {
            var stackVar1 = variables.PopVariable();
            var stackVar2 = variables.PopVariable();
            var resultVar = variables.PushFreshVariable();
            variables.PopVariable(); // clean, since we do not need the intermediate variable
            var command1 = new BinaryOperation(resultVar, stackVar1, binaryOp, stackVar2);

            commandList.Add(command1);
            basicBlocks.AddCommandToCurrentBasicBlock(command1, instruction);

            var target = instruction.Operand as Mono.Cecil.Cil.Instruction;
            if (target == null)
            {
                throw new Exception("No target given for branch instruction.");
            }

            var command2 = new BranchConditional(resultVar);
            // basicBlocks.AddJumpTarget(command2, target);
            commandList.Add(command2);
            ILToIr.AddCommand(instruction, command2);
            basicBlocks.AddCommandToCurrentBasicBlock(command2, instruction); // TODO: not elegant...
            return command1;
        }

        private Command ComputeBinary(Mono.Cecil.Cil.Instruction instruction, BinaryOperation.BinaryOp binaryOp)
        {
            var stackVar1 = variables.PopVariable();
            var stackVar2 = variables.PopVariable();
            var resultVar = variables.PushFreshVariable();
            var command1 = new BinaryOperation(resultVar, stackVar1, binaryOp, stackVar2);
            ILToIr.AddCommand(instruction, command1);
            commandList.Add(command1);
            basicBlocks.AddCommandToCurrentBasicBlock(command1, instruction);
            return command1;
        }


        private Command BranchUnary(Mono.Cecil.Cil.Instruction instruction, UnaryOperation.UnaryOp unaryOp)
        {
            var stackVar1 = variables.PopVariable();
            var resultVar = variables.PushFreshVariable();
            variables.PopVariable(); // clean afterwards...
            var command1 = new UntypedUnaryOperation(resultVar, unaryOp, stackVar1);
            
            commandList.Add(command1);
            basicBlocks.AddCommandToCurrentBasicBlock(command1, instruction);
            var target = instruction.Operand as Mono.Cecil.Cil.Instruction;
            if (target == null)
            {
                throw new Exception("No target given for branch instruction.");
            }
            var command2 = new BranchConditional(resultVar);
            // basicBlocks.AddJumpTarget(command2, target);
            commandList.Add(command2);
            ILToIr.AddCommand(instruction, command2);
            basicBlocks.AddCommandToCurrentBasicBlock(command2, instruction);
            return command1;
        }

        private Command ComputeUnary
            (Mono.Cecil.Cil.Instruction instruction, 
             UnaryOperation.UnaryOp unaryOp)
        {
            var stackVar1 = variables.PopVariable();
            var resultVar = variables.PushFreshVariable();
            var operandAsType = instruction.Operand as Mono.Cecil.TypeReference;
            UnaryOperation command1;
            if (operandAsType == null)
            {
                command1 = new UntypedUnaryOperation(resultVar, unaryOp, stackVar1);
            }
            else
            {
                var groundType = new Aramani.IR.Types.GroundType(operandAsType);
                command1 = new TypedUnaryOperation(resultVar, unaryOp, stackVar1, groundType);
            }
            ILToIr.AddCommand(instruction, command1);
            commandList.Add(command1);
            basicBlocks.AddCommandToCurrentBasicBlock(command1, instruction);
            return command1;
        }

        private Command LoadIndirect(Mono.Cecil.Cil.Instruction instruction, object type)
        {
            var stackVar1 = variables.PopVariable();
            var stackVar2 = variables.PushFreshVariable();
            var target = new DerefLocation(new VariableLocation(stackVar1));
            var command = new Receive(stackVar2, target);
            ILToIr.AddCommand(instruction, command);
            commandList.Add(command);
            basicBlocks.AddCommandToCurrentBasicBlock(command, instruction);
            return command;
        }

        private Command StoreIndirect(Mono.Cecil.Cil.Instruction instruction, object type)
        {
            var valueVar = variables.PopVariable();
            var addressVar = variables.PopVariable();
            var target = new DerefLocation(new VariableLocation(addressVar));
            var command = new Set(valueVar, target);
            ILToIr.AddCommand(instruction, command);
            commandList.Add(command);
            basicBlocks.AddCommandToCurrentBasicBlock(command, instruction);
            return command;
        }

        private Command Return(Mono.Cecil.Cil.Instruction instruction)
        {
            var command = new Return();
            ILToIr.AddCommand(instruction, command);
            commandList.Add(command);
            basicBlocks.AddCommandToCurrentBasicBlock(command, instruction);
            return command;
        }

        private Command PopVariable(Mono.Cecil.Cil.Instruction instruction)
        {
            variables.PopVariable();
            var command = new Nop();
            ILToIr.AddCommand(instruction, command);
            commandList.Add(command);
            basicBlocks.AddCommandToCurrentBasicBlock(command, instruction);
            return command;
        }

        private Command Nop(Mono.Cecil.Cil.Instruction instruction)
        {
            var command = new Nop();
            ILToIr.AddCommand(instruction, command);
            commandList.Add(command);
            basicBlocks.AddCommandToCurrentBasicBlock(command, instruction);
            return command;
        }

        private Command Convert(Mono.Cecil.Cil.Instruction instruction, 
                                Aramani.IR.Types.GroundType type,
                                bool withOverflowCheck)
        {
            var stackVar1 = variables.PopVariable();
            var stackVar2 = variables.PushFreshVariable();
            var command = new Aramani.IR.Commands.Convert(stackVar1, stackVar2, type, withOverflowCheck);
            ILToIr.AddCommand(instruction, command);
            commandList.Add(command);
            basicBlocks.AddCommandToCurrentBasicBlock(command, instruction);
            return command;
        }

        private Command LoadArrayElement(Mono.Cecil.Cil.Instruction instruction,
                                         Aramani.IR.Types.GroundType type)
        {
            var indexVar = variables.PopVariable();
            var arayRefVar = variables.PopVariable();
            var resultVar = variables.PushFreshVariable();
            var elementLocation = new ArrayElementLocation(arayRefVar, indexVar);
            var command = new Aramani.IR.Commands.Receive(resultVar, elementLocation);
            ILToIr.AddCommand(instruction, command);
            commandList.Add(command);
            basicBlocks.AddCommandToCurrentBasicBlock(command, instruction);
            return command;
        }

        private Command LoadIndirectArrayElement(Mono.Cecil.Cil.Instruction instruction,
                                 Aramani.IR.Types.GroundType type)
        {
            var indexVar = variables.PopVariable();
            var arayRefVar = variables.PopVariable();
            var resultVar = variables.PushFreshVariable();
            var elementLocation = new AddressOfLocation(new ArrayElementLocation(arayRefVar, indexVar));
            var command = new Aramani.IR.Commands.Receive(resultVar, elementLocation);
            ILToIr.AddCommand(instruction, command);
            commandList.Add(command);
            basicBlocks.AddCommandToCurrentBasicBlock(command, instruction);
            return command;
        }

        private Command StoreArrayElement(Mono.Cecil.Cil.Instruction instruction,
                                 Aramani.IR.Types.GroundType type)
        {
            var valueVar = variables.PopVariable();
            var indexVar = variables.PopVariable();
            var arayRefVar = variables.PopVariable();
            var elementLocation = new ArrayElementLocation(arayRefVar, indexVar);
            var command = new Aramani.IR.Commands.Set(valueVar, elementLocation);
            ILToIr.AddCommand(instruction, command);
            commandList.Add(command);
            basicBlocks.AddCommandToCurrentBasicBlock(command, instruction);
            return command;
        }

        private Command Call(Mono.Cecil.Cil.Instruction instruction, bool isVirtual)
        {
            var staticCallee = instruction.Operand as Mono.Cecil.MethodReference;
            if (staticCallee == null)
            {
                throw new Exception("Call instruction without valid static callee.");
            }
            var methodDefinition = staticCallee.Resolve();
            var arguments = new List<Aramani.IR.Variables.StackVariable>();
            if (staticCallee.HasParameters)
            {
                for (int i = 0; i < staticCallee.Parameters.Count; ++i)
                {
                    arguments.Add(variables.PopVariable());
                }
            }
            if (staticCallee.HasThis)
            {
                arguments.Add(variables.PopVariable());
            }
            arguments.Reverse();
            // TODO: binder
            Aramani.IR.Variables.StackVariable resultVar = null;
            if (methodDefinition.ReturnType.FullName != "System.Void")
            {
                resultVar = variables.PushFreshVariable();
            }
            var command = new Aramani.IR.Commands.Call(new IR.Routines.Routine(methodDefinition), arguments, resultVar, isVirtual);
            ILToIr.AddCommand(instruction, command);
            commandList.Add(command);
            basicBlocks.AddCommandToCurrentBasicBlock(command, instruction);
            return command;
        }

        private Command NewObject(Mono.Cecil.Cil.Instruction instruction)
        {
            var staticCallee = instruction.Operand as Mono.Cecil.MethodReference;
            if (staticCallee == null)
            {
                throw new Exception("Call instruction without valid static callee.");
            }
            var methodDefinition = staticCallee.Resolve();
            var arguments = new List<Aramani.IR.Variables.StackVariable>();
            if (staticCallee.HasParameters)
            {
                for (int i = 0; i < staticCallee.Parameters.Count; ++i)
                {
                    arguments.Add(variables.PopVariable());
                }
            }
            arguments.Reverse();
            // TODO: binder
            Aramani.IR.Variables.StackVariable resultVar = null;
            resultVar = variables.PushFreshVariable();
            var command = new Aramani.IR.Commands.NewObject(new IR.Routines.Routine(methodDefinition), arguments, resultVar);
            ILToIr.AddCommand(instruction, command);
            commandList.Add(command);
            basicBlocks.AddCommandToCurrentBasicBlock(command, instruction);
            return command;
        }

        private Command LoadInstanceField(Mono.Cecil.Cil.Instruction instruction, bool loadAddress)
        {
            var stackVar1 = variables.PopVariable();
            var stackVar2 = variables.PushFreshVariable();
            var fieldRef = instruction.Operand as Mono.Cecil.FieldReference;
            var fieldVar = new Aramani.IR.Variables.FieldVariable(fieldRef);
            Location sourceLocation = new InstanceFieldLocation(stackVar1, fieldVar);
            if (loadAddress)
                sourceLocation = new AddressOfLocation(sourceLocation);
            var command = new Receive(stackVar2, sourceLocation);
            ILToIr.AddCommand(instruction, command);
            commandList.Add(command);
            basicBlocks.AddCommandToCurrentBasicBlock(command, instruction);
            return command;
        }

        private Command StoreInstanceField(Mono.Cecil.Cil.Instruction instruction)
        {
            var valueVar = variables.PopVariable();
            var objVar = variables.PopVariable();
            var fieldRef = instruction.Operand as Mono.Cecil.FieldReference;
            var fieldVar = new Aramani.IR.Variables.FieldVariable(fieldRef);
            Location targetLocation = new InstanceFieldLocation(objVar, fieldVar);
            var command = new Set(valueVar, targetLocation);
            ILToIr.AddCommand(instruction, command);
            commandList.Add(command);
            basicBlocks.AddCommandToCurrentBasicBlock(command, instruction);
            return command;
        }

        private Command LoadStaticField(Mono.Cecil.Cil.Instruction instruction, bool loadAddress)
        {
            var stackVar = variables.PushFreshVariable();
            var fieldRef = instruction.Operand as Mono.Cecil.FieldReference;
            var fieldVar = new Aramani.IR.Variables.FieldVariable(fieldRef);
            Location sourceLocation = new VariableLocation(fieldVar);
            if (loadAddress)
                sourceLocation = new AddressOfLocation(sourceLocation);
            var command = new Receive(stackVar, sourceLocation);
            ILToIr.AddCommand(instruction, command);
            commandList.Add(command);
            basicBlocks.AddCommandToCurrentBasicBlock(command, instruction);
            return command;
        }

        private Command StoreStaticField(Mono.Cecil.Cil.Instruction instruction)
        {
            var valueVar = variables.PopVariable();
            var fieldRef = instruction.Operand as Mono.Cecil.FieldReference;
            var fieldVar = new Aramani.IR.Variables.FieldVariable(fieldRef);
            Location targetLocation = new VariableLocation(fieldVar);
            var command = new Set(valueVar, targetLocation);
            ILToIr.AddCommand(instruction, command);
            commandList.Add(command);
            basicBlocks.AddCommandToCurrentBasicBlock(command, instruction);
            return command;
        }



        Command CreateCommand(Mono.Cecil.Cil.Instruction instruction)
        {
            Command command = null;

            switch (instruction.OpCode.Code)
            {
                case Mono.Cecil.Cil.Code.Ldarg_0:
                    command = LoadArg(instruction, 0);
                    break;
                case Mono.Cecil.Cil.Code.Ldarg_1:
                    command = LoadArg(instruction, 1);
                    break;
                case Mono.Cecil.Cil.Code.Ldarg_2:
                    command = LoadArg(instruction, 2);
                    break;
                case Mono.Cecil.Cil.Code.Ldarg_3:
                    command = LoadArg(instruction, 3);
                    break;
                case Mono.Cecil.Cil.Code.Ldarg:
                case Mono.Cecil.Cil.Code.Ldarg_S:
                    command = LoadArg(instruction,
                            ((Mono.Cecil.ParameterDefinition)instruction.Operand).Index);
                    break;
                case Mono.Cecil.Cil.Code.Ldarga:
                case Mono.Cecil.Cil.Code.Ldarga_S:
                    command = LoadArgAddress(instruction,
                            ((Mono.Cecil.ParameterDefinition)instruction.Operand).Index);
                    break;
                case Mono.Cecil.Cil.Code.Stloc_0:
                    command = StoreToLocal(instruction, 0);
                    break;
                case Mono.Cecil.Cil.Code.Stloc_1:
                    command = StoreToLocal(instruction, 1);
                    break;
                case Mono.Cecil.Cil.Code.Stloc_2:
                    command = StoreToLocal(instruction, 2);
                    break;
                case Mono.Cecil.Cil.Code.Stloc_3:
                    command = StoreToLocal(instruction, 3);
                    break;
                case Mono.Cecil.Cil.Code.Stloc_S:
                case Mono.Cecil.Cil.Code.Stloc:
                    command = StoreToLocal(instruction,
                        ((Mono.Cecil.Cil.VariableDefinition)instruction.Operand).Index);
                    break;
                case Mono.Cecil.Cil.Code.Ldloc_0:
                    command = LoadLocal(instruction, 0);
                    break;
                case Mono.Cecil.Cil.Code.Ldloc_1:
                    command = LoadLocal(instruction, 1);
                    break;
                case Mono.Cecil.Cil.Code.Ldloc_2:
                    command = LoadLocal(instruction, 2);
                    break;
                case Mono.Cecil.Cil.Code.Ldloc_3:
                    command = LoadLocal(instruction, 3);
                    break;
                case Mono.Cecil.Cil.Code.Ldloc_S:
                case Mono.Cecil.Cil.Code.Ldloc:
                    command = LoadLocal(instruction,
                         ((Mono.Cecil.Cil.VariableDefinition)instruction.Operand).Index);
                    break;
                case Mono.Cecil.Cil.Code.Ldloca_S:
                case Mono.Cecil.Cil.Code.Ldloca:
                    command = LoadLocalAddress(instruction,
                        ((Mono.Cecil.Cil.VariableDefinition)instruction.Operand).Index);
                    break;
                case Mono.Cecil.Cil.Code.Ldnull:
                    command = LoadConstant<object>(instruction, null);
                    break;
                case Mono.Cecil.Cil.Code.Starg_S:
                case Mono.Cecil.Cil.Code.Starg:
                    command = StoreArgument
                        (instruction,
                         ((Mono.Cecil.ParameterDefinition)instruction.Operand).Index);
                    break;
                case Mono.Cecil.Cil.Code.Ldc_I4_M1:
                    LoadConstant<int>(instruction, -1);
                    break;
                case Mono.Cecil.Cil.Code.Ldc_I4_0:
                    command = LoadConstant<int>(instruction, 0);
                    break;
                case Mono.Cecil.Cil.Code.Ldc_I4_1:
                    command = LoadConstant<int>(instruction, 1);
                    break;
                case Mono.Cecil.Cil.Code.Ldc_I4_2:
                    command = LoadConstant<int>(instruction, 2);
                    break;
                case Mono.Cecil.Cil.Code.Ldc_I4_3:
                    command = LoadConstant<int>(instruction, 3);
                    break;
                case Mono.Cecil.Cil.Code.Ldc_I4_4:
                    command = LoadConstant<int>(instruction, 4);
                    break;
                case Mono.Cecil.Cil.Code.Ldc_I4_5:
                    command = LoadConstant<int>(instruction, 5);
                    break;
                case Mono.Cecil.Cil.Code.Ldc_I4_6:
                    command = LoadConstant<int>(instruction, 6);
                    break;
                case Mono.Cecil.Cil.Code.Ldc_I4_7:
                    command = LoadConstant<int>(instruction, 7);
                    break;
                case Mono.Cecil.Cil.Code.Ldc_I4_8:
                    command = LoadConstant<int>(instruction, 8);
                    break;
                case Mono.Cecil.Cil.Code.Ldstr:
                    command = LoadConstant<string>(instruction, (string)(instruction.Operand));
                    break;
                case Mono.Cecil.Cil.Code.Ldc_I4_S:
                    command = LoadConstant<int>(instruction, (sbyte)(instruction.Operand) & 0xff);
                    break;
                case Mono.Cecil.Cil.Code.Ldc_I4:
                    command = LoadConstant<int>(instruction, (int)(instruction.Operand));
                    break;
                case Mono.Cecil.Cil.Code.Ldc_I8:
                    command = LoadConstant<long>(instruction, (long)(instruction.Operand));
                    break;
                case Mono.Cecil.Cil.Code.Ldc_R4:
                    command = LoadConstant<float>(instruction, (float)(instruction.Operand));
                    break;
                case Mono.Cecil.Cil.Code.Ldc_R8:
                    command = LoadConstant<double>(instruction, (double)(instruction.Operand));
                    break;
                case Mono.Cecil.Cil.Code.Dup:
                    command = Duplicate(instruction);
                    break;
                case Mono.Cecil.Cil.Code.Pop:
                    command = PopVariable(instruction);

                    break;
                case Mono.Cecil.Cil.Code.Nop:
                    command = Nop(instruction);
                    break;
                case Mono.Cecil.Cil.Code.Break:
                    // IGNORE
                    break;
                case Mono.Cecil.Cil.Code.Br:
                case Mono.Cecil.Cil.Code.Br_S:
                case Mono.Cecil.Cil.Code.Leave:
                case Mono.Cecil.Cil.Code.Leave_S:
                    command = BranchUnconditionally(instruction);
                    break;

                case Mono.Cecil.Cil.Code.Stind_Ref:
                case Mono.Cecil.Cil.Code.Stind_I1:
                case Mono.Cecil.Cil.Code.Stind_I2:
                case Mono.Cecil.Cil.Code.Stind_I4:
                case Mono.Cecil.Cil.Code.Stind_I8:
                case Mono.Cecil.Cil.Code.Stind_R4:
                case Mono.Cecil.Cil.Code.Stind_R8:
                case Mono.Cecil.Cil.Code.Stind_I:
                    command = StoreIndirect(instruction, null);
                    break;

                case Mono.Cecil.Cil.Code.Stobj:
                    break;

                case Mono.Cecil.Cil.Code.Ldind_I1:
                case Mono.Cecil.Cil.Code.Ldind_U1:
                case Mono.Cecil.Cil.Code.Ldind_I2:
                case Mono.Cecil.Cil.Code.Ldind_U2:
                case Mono.Cecil.Cil.Code.Ldind_I4:
                case Mono.Cecil.Cil.Code.Ldind_U4:
                case Mono.Cecil.Cil.Code.Ldind_I8:
                case Mono.Cecil.Cil.Code.Ldind_I:
                case Mono.Cecil.Cil.Code.Ldind_R4:
                case Mono.Cecil.Cil.Code.Ldind_R8:
                case Mono.Cecil.Cil.Code.Ldind_Ref:
                    command = LoadIndirect(instruction, null);
                    break;


                case Mono.Cecil.Cil.Code.Brfalse_S:
                case Mono.Cecil.Cil.Code.Brfalse:
                    command = BranchUnary(instruction, UnaryOperation.UnaryOp.NEG);
                    break;
                case Mono.Cecil.Cil.Code.Brtrue_S:
                case Mono.Cecil.Cil.Code.Brtrue:
                    command = BranchUnary(instruction, UnaryOperation.UnaryOp.ID);
                    break;
                case Mono.Cecil.Cil.Code.Beq_S:
                case Mono.Cecil.Cil.Code.Beq:
                    command = BranchBinary(instruction, BinaryOperation.BinaryOp.EQ);
                    break;

                // TODO: UNSIGNED VERSIONS !!!

                case Mono.Cecil.Cil.Code.Bge_S:
                case Mono.Cecil.Cil.Code.Bge:
                case Mono.Cecil.Cil.Code.Bge_Un_S:
                case Mono.Cecil.Cil.Code.Bge_Un:
                    command = BranchBinary(instruction, BinaryOperation.BinaryOp.GEQ);
                    break;

                case Mono.Cecil.Cil.Code.Bgt_S:
                case Mono.Cecil.Cil.Code.Bgt:
                case Mono.Cecil.Cil.Code.Bgt_Un_S:
                case Mono.Cecil.Cil.Code.Bgt_Un:
                    command = BranchBinary(instruction, BinaryOperation.BinaryOp.GE);
                    break;

                case Mono.Cecil.Cil.Code.Ble_S:
                case Mono.Cecil.Cil.Code.Ble:
                case Mono.Cecil.Cil.Code.Ble_Un_S:
                case Mono.Cecil.Cil.Code.Ble_Un:
                    command = BranchBinary(instruction, BinaryOperation.BinaryOp.LEQ);
                    break;

                case Mono.Cecil.Cil.Code.Blt_S:
                case Mono.Cecil.Cil.Code.Blt:
                case Mono.Cecil.Cil.Code.Blt_Un_S:
                case Mono.Cecil.Cil.Code.Blt_Un:
                    command = BranchBinary(instruction, BinaryOperation.BinaryOp.LE);
                    break;

                case Mono.Cecil.Cil.Code.Bne_Un_S:
                case Mono.Cecil.Cil.Code.Bne_Un:
                    command = BranchBinary(instruction, BinaryOperation.BinaryOp.NEQ);
                    break;

                case Mono.Cecil.Cil.Code.Add:
                case Mono.Cecil.Cil.Code.Sub:
                case Mono.Cecil.Cil.Code.Mul:
                case Mono.Cecil.Cil.Code.Div:
                case Mono.Cecil.Cil.Code.Div_Un:
                case Mono.Cecil.Cil.Code.Rem:
                case Mono.Cecil.Cil.Code.Rem_Un:
                case Mono.Cecil.Cil.Code.And:
                case Mono.Cecil.Cil.Code.Or:
                case Mono.Cecil.Cil.Code.Xor:
                case Mono.Cecil.Cil.Code.Shl:
                case Mono.Cecil.Cil.Code.Shr:
                case Mono.Cecil.Cil.Code.Shr_Un:
                case Mono.Cecil.Cil.Code.Add_Ovf:
                case Mono.Cecil.Cil.Code.Add_Ovf_Un:
                case Mono.Cecil.Cil.Code.Mul_Ovf:
                case Mono.Cecil.Cil.Code.Mul_Ovf_Un:
                case Mono.Cecil.Cil.Code.Sub_Ovf:
                case Mono.Cecil.Cil.Code.Sub_Ovf_Un:
                case Mono.Cecil.Cil.Code.Clt:
                case Mono.Cecil.Cil.Code.Clt_Un:
                    command = ComputeBinary(instruction, BinaryOperation.BinaryOp.UNKNOWN);
                    break;
                case Mono.Cecil.Cil.Code.Ceq:
                    command = ComputeBinary(instruction, BinaryOperation.BinaryOp.EQ);
                    break;
                case Mono.Cecil.Cil.Code.Cgt:
                case Mono.Cecil.Cil.Code.Cgt_Un:
                    command = ComputeBinary(instruction, BinaryOperation.BinaryOp.GE);
                    break;
                case Mono.Cecil.Cil.Code.Neg:
                case Mono.Cecil.Cil.Code.Not:
                    command = ComputeUnary(instruction, UnaryOperation.UnaryOp.UNKNOWN);
                    break;

                case Mono.Cecil.Cil.Code.Conv_I1:
                case Mono.Cecil.Cil.Code.Conv_I2:
                case Mono.Cecil.Cil.Code.Conv_I4:
                case Mono.Cecil.Cil.Code.Conv_I8:
                case Mono.Cecil.Cil.Code.Conv_R4:
                case Mono.Cecil.Cil.Code.Conv_R8:
                case Mono.Cecil.Cil.Code.Conv_U4:
                case Mono.Cecil.Cil.Code.Conv_U8:
                case Mono.Cecil.Cil.Code.Conv_R_Un:
                case Mono.Cecil.Cil.Code.Conv_U:
                case Mono.Cecil.Cil.Code.Conv_U2:
                case Mono.Cecil.Cil.Code.Conv_U1:
                case Mono.Cecil.Cil.Code.Conv_I:
                    Convert(instruction, null, false);
                    break;
                case Mono.Cecil.Cil.Code.Conv_Ovf_I1_Un:
                case Mono.Cecil.Cil.Code.Conv_Ovf_I2_Un:
                case Mono.Cecil.Cil.Code.Conv_Ovf_I4_Un:
                case Mono.Cecil.Cil.Code.Conv_Ovf_I8_Un:
                case Mono.Cecil.Cil.Code.Conv_Ovf_U1_Un:
                case Mono.Cecil.Cil.Code.Conv_Ovf_U2_Un:
                case Mono.Cecil.Cil.Code.Conv_Ovf_U4_Un:
                case Mono.Cecil.Cil.Code.Conv_Ovf_U8_Un:
                case Mono.Cecil.Cil.Code.Conv_Ovf_I_Un:
                case Mono.Cecil.Cil.Code.Conv_Ovf_U_Un:
                case Mono.Cecil.Cil.Code.Conv_Ovf_I:
                case Mono.Cecil.Cil.Code.Conv_Ovf_U:
                case Mono.Cecil.Cil.Code.Conv_Ovf_I1:
                case Mono.Cecil.Cil.Code.Conv_Ovf_U1:
                case Mono.Cecil.Cil.Code.Conv_Ovf_I2:
                case Mono.Cecil.Cil.Code.Conv_Ovf_U2:
                case Mono.Cecil.Cil.Code.Conv_Ovf_I4:
                case Mono.Cecil.Cil.Code.Conv_Ovf_U4:
                case Mono.Cecil.Cil.Code.Conv_Ovf_I8:
                case Mono.Cecil.Cil.Code.Conv_Ovf_U8:
                    command = Convert(instruction, null, true);
                    break;

                case Mono.Cecil.Cil.Code.Ldelema:
                    command = LoadIndirectArrayElement(instruction, null);
                    break;

                case Mono.Cecil.Cil.Code.Ldelem_I1:
                case Mono.Cecil.Cil.Code.Ldelem_U1:
                case Mono.Cecil.Cil.Code.Ldelem_I2:
                case Mono.Cecil.Cil.Code.Ldelem_U2:
                case Mono.Cecil.Cil.Code.Ldelem_I4:
                case Mono.Cecil.Cil.Code.Ldelem_U4:
                case Mono.Cecil.Cil.Code.Ldelem_I8:
                case Mono.Cecil.Cil.Code.Ldelem_I:
                case Mono.Cecil.Cil.Code.Ldelem_R4:
                case Mono.Cecil.Cil.Code.Ldelem_R8:
                case Mono.Cecil.Cil.Code.Ldelem_Ref:
                case Mono.Cecil.Cil.Code.Ldelem_Any:
                    command = LoadArrayElement(instruction, null);
                    break;

                case Mono.Cecil.Cil.Code.Stelem_I:
                case Mono.Cecil.Cil.Code.Stelem_I1:
                case Mono.Cecil.Cil.Code.Stelem_I2:
                case Mono.Cecil.Cil.Code.Stelem_I4:
                case Mono.Cecil.Cil.Code.Stelem_I8:
                case Mono.Cecil.Cil.Code.Stelem_R4:
                case Mono.Cecil.Cil.Code.Stelem_R8:
                case Mono.Cecil.Cil.Code.Stelem_Ref:
                case Mono.Cecil.Cil.Code.Stelem_Any:
                    command = StoreArrayElement(instruction, null);
                    break;
                case Mono.Cecil.Cil.Code.Call:
                    command = Call(instruction, false);
                    break;
                case Mono.Cecil.Cil.Code.Callvirt:
                    command = Call(instruction, true);
                    break;
                case Mono.Cecil.Cil.Code.Ret:
                    command = Return(instruction);
                    break;
                case Mono.Cecil.Cil.Code.Newobj:
                    command = NewObject(instruction);
                    break;
                case Mono.Cecil.Cil.Code.Ldfld:
                    command = LoadInstanceField(instruction, false);
                    break;
                case Mono.Cecil.Cil.Code.Ldflda:
                    command = LoadInstanceField(instruction, true);
                    break;
                case Mono.Cecil.Cil.Code.Stfld:
                    command = StoreInstanceField(instruction);
                    break;
                case Mono.Cecil.Cil.Code.Ldsfld:
                    command = LoadStaticField(instruction, false);
                    break;
                case Mono.Cecil.Cil.Code.Ldsflda:
                    command = LoadStaticField(instruction, true);
                    break;
                case Mono.Cecil.Cil.Code.Stsfld:
                    command = StoreStaticField(instruction);
                    break;

                case Mono.Cecil.Cil.Code.Readonly:
                case Mono.Cecil.Cil.Code.Unaligned:
                case Mono.Cecil.Cil.Code.Volatile:
                    // NO EFFECT
                    break;
                case Mono.Cecil.Cil.Code.Constrained:
                    Console.WriteLine("CONSTRAINED is not supported yet.");
                    break;
                case Mono.Cecil.Cil.Code.Jmp:
                    Console.WriteLine("JMP is not supported yet.");
                    break;
                case Mono.Cecil.Cil.Code.Mkrefany:
                    Console.WriteLine("MKREFANY is not supported yet.");
                    break;
                case Mono.Cecil.Cil.Code.Refanyval:
                    Console.WriteLine("REFANYVAL is not supported yet.");
                    break;
                case Mono.Cecil.Cil.Code.Refanytype:
                    Console.WriteLine("REFANYVAL is not supported yet.");
                    break;
                case Mono.Cecil.Cil.Code.Ldtoken:
                    Console.WriteLine("LDTOKEN is not supported yet.");
                    break;
                case Mono.Cecil.Cil.Code.Endfilter:
                    Console.WriteLine("ENDFILTER is not supported yet.");
                    break;
                case Mono.Cecil.Cil.Code.Switch:
                    Console.WriteLine("SWITCH is not supported yet.");
                    break;
                case Mono.Cecil.Cil.Code.Endfinally:
                    Console.WriteLine("ENDFINALLY is not supported yet.");
                    break;
                case Mono.Cecil.Cil.Code.Initblk:
                    Console.WriteLine("INITBLK is not supported yet.");
                    break;
                case Mono.Cecil.Cil.Code.Tail:
                    Console.WriteLine("TAIL is not supported yet.");
                    break;
                case Mono.Cecil.Cil.Code.Arglist:
                    Console.WriteLine("ARGLIST is not supported yet.");
                    break;
                case Mono.Cecil.Cil.Code.No:
                    Console.WriteLine("NO is not supported yet.");
                    break;
                case Mono.Cecil.Cil.Code.Calli:
                    Console.WriteLine("CALLI is not supported yet.");
                    break;
                case Mono.Cecil.Cil.Code.Cpblk:
                    Console.WriteLine("CPBLK is not supported yet.");
                    break;
                case Mono.Cecil.Cil.Code.Cpobj:
                    Console.WriteLine("CPOBJ is not supported yet.");
                    break;
                case Mono.Cecil.Cil.Code.Ldobj:
                    Console.WriteLine("LDOBJ is not supported yet.");
                    break;
                case Mono.Cecil.Cil.Code.Rethrow:
                    Console.WriteLine("RETHROW is not supported yet.");
                    break;
                case Mono.Cecil.Cil.Code.Ldvirtftn:
                    Console.WriteLine("LDVIRTFTN is not supported yet.");
                    break;
                case Mono.Cecil.Cil.Code.Ldftn:
                    Console.WriteLine("LDFTN is not supported yet.");
                    break;

                // Unary op without type info
                case Mono.Cecil.Cil.Code.Ldlen:
                    command = ComputeUnary(instruction, UnaryOperation.UnaryOp.LOADLEN);
                    break;
                case Mono.Cecil.Cil.Code.Sizeof:
                    command = ComputeUnary(instruction, UnaryOperation.UnaryOp.SIZEOF);
                    break;
                case Mono.Cecil.Cil.Code.Throw:
                    command = ComputeUnary(instruction, UnaryOperation.UnaryOp.THROW);
                    break;
                case Mono.Cecil.Cil.Code.Localloc:
                    command = ComputeUnary(instruction, UnaryOperation.UnaryOp.LOCALLOC);
                    break;
                case Mono.Cecil.Cil.Code.Ckfinite:
                    command = ComputeUnary(instruction, UnaryOperation.UnaryOp.CKFINITE);
                    break;
                // Unary Op with type info
                case Mono.Cecil.Cil.Code.Initobj:
                    command = ComputeUnary(instruction, UnaryOperation.UnaryOp.INITOBJ);
                    break;
                case Mono.Cecil.Cil.Code.Box:
                    command = ComputeUnary(instruction, UnaryOperation.UnaryOp.BOX);
                    break;
                case Mono.Cecil.Cil.Code.Unbox:
                    command = ComputeUnary(instruction, UnaryOperation.UnaryOp.UNBOX);
                    break;
                case Mono.Cecil.Cil.Code.Castclass:
                    command = ComputeUnary(instruction, UnaryOperation.UnaryOp.CASTCLASS);
                    break;
                case Mono.Cecil.Cil.Code.Isinst:
                    command = ComputeUnary(instruction, UnaryOperation.UnaryOp.ISINST);
                    break;
                case Mono.Cecil.Cil.Code.Newarr:
                    command = ComputeUnary(instruction, UnaryOperation.UnaryOp.NEWARR);
                    break;
                case Mono.Cecil.Cil.Code.Unbox_Any:
                    command = ComputeUnary(instruction, UnaryOperation.UnaryOp.UNBOX_ANY);
                    break;

                default:
                    break;
            }
            return command;
        }

        public void TransformMethod(Mono.Cecil.MethodDefinition method)
        {
            stackHeights = new Dictionary<Mono.Cecil.Cil.Instruction, int>();
            variables = new VariableFactory(method.Body.MaxStackSize);
            ILToIr = new ILLocationsToIR();
            basicBlocks = new BasicBlocks();
            commandList = new List<Command>();

            if (method.Body == null || method.Body.Instructions == null)
            {
                Console.WriteLine("Warning: No method body in method {0}.", method);
                return;
            }

            // phase 1
            foreach (var instruction in method.Body.Instructions)
            {
                if (instruction.OpCode.OperandType == Mono.Cecil.Cil.OperandType.InlineSwitch)
                {
                    foreach (var target in (Mono.Cecil.Cil.Instruction[])instruction.Operand)
                    {
                        basicBlocks.AddBasicBlockEntry(target);
                    }
                }
                else if (instruction.OpCode.OperandType == Mono.Cecil.Cil.OperandType.InlineBrTarget ||
                    instruction.OpCode.OperandType == Mono.Cecil.Cil.OperandType.ShortInlineBrTarget)
                {
                    basicBlocks.AddBasicBlockEntry(instruction.Operand as Mono.Cecil.Cil.Instruction);
                }
            }

            // phase 2
            basicBlocks.PushNewBasicBlock();
            ComputeCommands(method.Body.Instructions[0]);

            // phase 3
            foreach (var basicBlock in basicBlocks.Blocks)
            {
                foreach (var command in basicBlock.Code)
                {
                    var asBranch = command as Branch;
                    if (asBranch == null)
                        continue;
                    var instruction = ILToIr.Get(command);
                    if (instruction == null)
                    {
                        Console.WriteLine("%%%" + command + "," + instruction);
                        continue;
                    }
                    if (instruction.OpCode.OperandType == Mono.Cecil.Cil.OperandType.InlineBrTarget ||
                        instruction.OpCode.OperandType == Mono.Cecil.Cil.OperandType.ShortInlineBrTarget)
                    {
                        var target = instruction.Operand as Mono.Cecil.Cil.Instruction;
                        var targetCommand = ILToIr.Get(target);
                        if (targetCommand != null)
                        {
                            asBranch.Target = basicBlocks.GetBlock(targetCommand);
                        }
                        else
                        {
                            Console.WriteLine("NO TARGET " + instruction.Operand);
                        }
                    }
                    else if (instruction.OpCode.OperandType == Mono.Cecil.Cil.OperandType.InlineSwitch)
                    {
                        // TODO
                    }
                }
            }

            // output 
            foreach (var basicBlock in basicBlocks.Blocks)
            {
                Console.WriteLine(basicBlock.Description);
            }
        }


        private void ComputeCommands(Mono.Cecil.Cil.Instruction instruction, 
                                     int currentStackHeight = 0)
        {
            Console.WriteLine("PROCESSING: " + instruction);
            int storedStackHeight;

            Aramani.IR.BasicBlocks.BasicBlock previousBlock = null;

            if (stackHeights.TryGetValue(instruction, out storedStackHeight))
            {
                if (currentStackHeight != storedStackHeight)
                    Console.WriteLine("Different stack heigths at " 
                                      + instruction + " OLD: " 
                                      + storedStackHeight 
                                      + ", NEW: " + currentStackHeight);
            }
            else
            {

                if (basicBlocks.IsJumpTarget(instruction))
                {
                    var buffer = basicBlocks.CurrentBasicBlock;
                    basicBlocks.PushNewBasicBlock();
                    buffer.Next = basicBlocks.CurrentBasicBlock;

                }

                switch (instruction.OpCode.Code)
                {
                    case Mono.Cecil.Cil.Code.Call:
                    case Mono.Cecil.Cil.Code.Calli:
                    case Mono.Cecil.Cil.Code.Callvirt:
                    case Mono.Cecil.Cil.Code.Newobj:
                        basicBlocks.PushNewBasicBlock();
                        break;
                    default:
                        break;
                }

                var command = CreateCommand(instruction);


                stackHeights.Add(instruction, currentStackHeight);
                currentStackHeight += instruction.StackDelta;

                if (instruction.OpCode.OperandType == Mono.Cecil.Cil.OperandType.InlineSwitch)
                {
                    basicBlocks.PushNewBasicBlock();
                    foreach (var target in (Mono.Cecil.Cil.Instruction[])instruction.Operand)
                    {
                        ComputeCommands(target, currentStackHeight);
                    }
                    // TODO: continuation? basic block connection.
                }
                else if (instruction.OpCode.OperandType == Mono.Cecil.Cil.OperandType.InlineBrTarget ||
                         instruction.OpCode.OperandType == Mono.Cecil.Cil.OperandType.ShortInlineBrTarget)
                {
                    previousBlock = basicBlocks.CurrentBasicBlock;
                    basicBlocks.PushNewBasicBlock();
                    ComputeCommands(instruction.Operand as Mono.Cecil.Cil.Instruction, currentStackHeight);
                }

                switch (instruction.OpCode.Code)
                {
                    // unconditional jump
                    case Mono.Cecil.Cil.Code.Jmp:
                    case Mono.Cecil.Cil.Code.Ret:
                    case Mono.Cecil.Cil.Code.Br_S:
                    case Mono.Cecil.Cil.Code.Br:
                    case Mono.Cecil.Cil.Code.Throw:
                    case Mono.Cecil.Cil.Code.Endfinally:
                    case Mono.Cecil.Cil.Code.Leave:
                    case Mono.Cecil.Cil.Code.Leave_S:
                    case Mono.Cecil.Cil.Code.Endfilter:
                    case Mono.Cecil.Cil.Code.Rethrow:
                        basicBlocks.PushNewBasicBlock();
                        break;
                    case Mono.Cecil.Cil.Code.Call:
                    case Mono.Cecil.Cil.Code.Calli:
                    case Mono.Cecil.Cil.Code.Callvirt:
                    case Mono.Cecil.Cil.Code.Newobj:
                        var buffer = basicBlocks.CurrentBasicBlock;
                        basicBlocks.PushNewBasicBlock();
                        
                        ComputeCommands(instruction.Next, currentStackHeight);
                        break;
                    default:
                        ComputeCommands(instruction.Next, currentStackHeight);
                        break;
                }
            }
        }
    }
}
