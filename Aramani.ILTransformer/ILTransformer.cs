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

        private List<Command> commandList;
        private VariableFactory variables;
        private ILLocationsToIR ILToIr;
        private BasicBlocks basicBlocks;

        private Dictionary<Mono.Cecil.Cil.Instruction, int> stackHeights;

        private void ComputeStackHeights(Mono.Cecil.Cil.Instruction instruction, int currentHeight = 0)
        {
            int storedStackHeight;
            Console.WriteLine("VISITING: " + instruction);
            if (stackHeights.TryGetValue(instruction, out storedStackHeight))
            {
                if (currentHeight != storedStackHeight)
                    Console.WriteLine("Different stack heigths at " + instruction + " OLD: " + storedStackHeight + ", NEW: " + currentHeight);
            }
            else
            {

                stackHeights.Add(instruction, currentHeight);
                currentHeight += instruction.StackDelta;

                if (instruction.OpCode.OperandType == Mono.Cecil.Cil.OperandType.InlineSwitch)
                {
                    foreach (var target in (Mono.Cecil.Cil.Instruction[])instruction.Operand)
                    {
                        ComputeStackHeights(target, currentHeight);
                    }
                }
                else if (instruction.OpCode.OperandType == Mono.Cecil.Cil.OperandType.InlineBrTarget ||
                    instruction.OpCode.OperandType == Mono.Cecil.Cil.OperandType.ShortInlineBrTarget)
                {
                    ComputeStackHeights(instruction.Operand as Mono.Cecil.Cil.Instruction, currentHeight);
                }


                switch (instruction.OpCode.Code)
                {

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
                        break;
                    default:
                        ComputeStackHeights(instruction.Next, currentHeight);
                        break;
                }
            }

        }

        private Command LoadArg(Mono.Cecil.Cil.Instruction instruction, int index)
        {
            var stackVar = variables.PushFreshVariable();
            var target = new VariableLocation(variables.GetParameter(index));
            var command = new Receive(stackVar, target);
            CommandPair pair = new CommandPair(command, instruction);
            Console.Write(command.Description);
            ILToIr.Add(instruction, command);
            commandList.Add(command);
            return command;
        }

        private Command LoadArgAddress(Mono.Cecil.Cil.Instruction instruction, int index)
        {
            var stackVar = variables.PushFreshVariable();
            var target = new AddressOfLocation(new VariableLocation(variables.GetParameter(index)));
            var command = new Receive(stackVar, target);
            CommandPair pair = new CommandPair(command, instruction);
            Console.Write(command.Description);
            ILToIr.Add(instruction, command);
            commandList.Add(command);
            return command;
        }

        private Command StoreToLocal(Mono.Cecil.Cil.Instruction instruction, int index)
        {
            var stackVar = variables.PopVariable();
            var target = new VariableLocation(variables.GetLocalVariable(index));
            var command = new Set(stackVar, target);
            CommandPair pair = new CommandPair(command, instruction);
            Console.Write(command.Description);
            ILToIr.Add(instruction, command);
            commandList.Add(command);
            return command;
        }

        private Command LoadLocal(Mono.Cecil.Cil.Instruction instruction, int index)
        {
            var stackVar = variables.PushFreshVariable();
            var target = new VariableLocation(variables.GetLocalVariable(index));
            var command = new Receive(stackVar, target);
            CommandPair pair = new CommandPair(command, instruction);
            Console.Write(command.Description);
            ILToIr.Add(instruction, command);
            commandList.Add(command);
            return command;
        }

        private Command LoadLocalAddress(Mono.Cecil.Cil.Instruction instruction, int index)
        {
            var stackVar = variables.PushFreshVariable();
            var target = new AddressOfLocation(new VariableLocation(variables.GetLocalVariable(index)));
            var command = new Receive(stackVar, target);
            CommandPair pair = new CommandPair(command, instruction);
            Console.Write(command.Description);
            ILToIr.Add(instruction, command);
            commandList.Add(command);
            return command;
        }

        private Command LoadConstant<T>(Mono.Cecil.Cil.Instruction instruction, T constant)
        {
            var stackVar = variables.PushFreshVariable();
            var constantLocation = new ConstantLocation<T>(constant);
            var command = new Receive(stackVar, constantLocation);
            CommandPair pair = new CommandPair(command, instruction);
            Console.WriteLine(command.Description);
            ILToIr.Add(instruction, command);
            commandList.Add(command);
            return command;
        }

        private Command StoreArgument(Mono.Cecil.Cil.Instruction instruction, int index)
        {
            var stackVar = variables.PopVariable();
            var target = new VariableLocation(variables.GetParameter(index));
            var command = new Set(stackVar, target);
            CommandPair pair = new CommandPair(command, instruction);
            Console.Write(command.Description);
            ILToIr.Add(instruction, command);
            commandList.Add(command);
            return command;
        }

        private Command Duplicate(Mono.Cecil.Cil.Instruction instruction)
        {
            var stackVar1 = variables.TopVariable();
            var stackVar2 = variables.PushFreshVariable();
            var target = new VariableLocation(stackVar2);
            var command = new Set(stackVar1, target);
            CommandPair pair = new CommandPair(command, instruction);
            Console.Write(command.Description);
            ILToIr.Add(instruction, command);
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
            basicBlocks.AddJumpTarget(command, target);   
            Console.Write(command.Description);
            ILToIr.Add(instruction, command);
            commandList.Add(command);
            return command;
        }

        private Command BranchBinary(Mono.Cecil.Cil.Instruction instruction, BinaryOperation.BinaryOp binaryOp)
        {
            var stackVar1 = variables.PopVariable();
            var stackVar2 = variables.PopVariable();
            var resultVar = variables.PushFreshVariable();
            variables.PopVariable(); // clean, since we do not need the intermediate variable
            var command1 = new BinaryOperation(resultVar, stackVar1, binaryOp, stackVar2);
            ILToIr.Add(instruction, command1);
            commandList.Add(command1);
            Console.Write(command1.Description);

            var target = instruction.Operand as Mono.Cecil.Cil.Instruction;
            if (target == null)
            {
                throw new Exception("No target given for branch instruction.");
            }

            var command2 = new BranchConditional(resultVar);
            basicBlocks.AddJumpTarget(command2, target);
            Console.Write(command2.Description);
            commandList.Add(command2);
            return command1;
        }

        private Command ComputeBinary(Mono.Cecil.Cil.Instruction instruction, BinaryOperation.BinaryOp binaryOp)
        {
            var stackVar1 = variables.PopVariable();
            var stackVar2 = variables.PopVariable();
            var resultVar = variables.PushFreshVariable();
            var command1 = new BinaryOperation(resultVar, stackVar1, binaryOp, stackVar2);
            ILToIr.Add(instruction, command1);
            commandList.Add(command1);
            Console.Write(command1.Description);
            return command1;
        }


        private Command BranchUnary(Mono.Cecil.Cil.Instruction instruction, UnaryOperation.UnaryOp unaryOp)
        {
            var stackVar1 = variables.PopVariable();
            var resultVar = variables.PushFreshVariable();
            variables.PopVariable(); // clean afterwards...
            var command1 = new UntypedUnaryOperation(resultVar, unaryOp, stackVar1);
            ILToIr.Add(instruction, command1);
            commandList.Add(command1);
            Console.Write(command1.Description);

            var target = instruction.Operand as Mono.Cecil.Cil.Instruction;
            if (target == null)
            {
                throw new Exception("No target given for branch instruction.");
            }
            var command2 = new BranchConditional(resultVar);
            basicBlocks.AddJumpTarget(command2, target);
            Console.Write(command2.Description);
            commandList.Add(command2);
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
            ILToIr.Add(instruction, command1);
            commandList.Add(command1);
            Console.Write(command1.Description);
            return command1;
        }

        private Command LoadIndirect(Mono.Cecil.Cil.Instruction instruction, object type)
        {
            var stackVar1 = variables.PopVariable();
            var stackVar2 = variables.PushFreshVariable();
            var target = new DerefLocation(new VariableLocation(stackVar1));
            var command = new Receive(stackVar2, target);
            CommandPair pair = new CommandPair(command, instruction);
            Console.Write(command.Description);
            ILToIr.Add(instruction, command);
            commandList.Add(command);
            return command;
        }

        private Command StoreIndirect(Mono.Cecil.Cil.Instruction instruction, object type)
        {
            var valueVar = variables.PopVariable();
            var addressVar = variables.PopVariable();
            var target = new DerefLocation(new VariableLocation(addressVar));
            var command = new Set(valueVar, target);
            CommandPair pair = new CommandPair(command, instruction);
            Console.Write(command.Description);
            ILToIr.Add(instruction, command);
            commandList.Add(command);
            return command;
        }

        private Command Return(Mono.Cecil.Cil.Instruction instruction)
        {
            var command = new Return();
            CommandPair pair = new CommandPair(command, instruction);
            Console.Write(command.Description);
            ILToIr.Add(instruction, command);
            commandList.Add(command);
            return command;
        }

        private Command PopVariable(Mono.Cecil.Cil.Instruction instruction)
        {
            variables.PopVariable();
            var command = new Nop();
            CommandPair pair = new CommandPair(command, instruction);
            Console.Write(command.Description);
            ILToIr.Add(instruction, command);
            commandList.Add(command);
            return command;
        }

        private Command Nop(Mono.Cecil.Cil.Instruction instruction)
        {
            var command = new Nop();
            CommandPair pair = new CommandPair(command, instruction);
            Console.Write(command.Description);
            ILToIr.Add(instruction, command);
            commandList.Add(command);
            return command;
        }


        private Command Convert(Mono.Cecil.Cil.Instruction instruction, 
                                Aramani.IR.Types.GroundType type,
                                bool withOverflowCheck)
        {
            var stackVar1 = variables.PopVariable();
            var stackVar2 = variables.PushFreshVariable();
            var command = new Aramani.IR.Commands.Convert(stackVar1, stackVar2, type, withOverflowCheck);
            CommandPair pair = new CommandPair(command, instruction);
            Console.Write(command.Description);
            ILToIr.Add(instruction, command);
            commandList.Add(command);
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
            CommandPair pair = new CommandPair(command, instruction);
            Console.Write(command.Description);
            ILToIr.Add(instruction, command);
            commandList.Add(command);
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
            CommandPair pair = new CommandPair(command, instruction);
            Console.Write(command.Description);
            ILToIr.Add(instruction, command);
            commandList.Add(command);
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
            CommandPair pair = new CommandPair(command, instruction);
            Console.Write(command.Description);
            ILToIr.Add(instruction, command);
            commandList.Add(command);
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

            CommandPair pair = new CommandPair(command, instruction);
            Console.Write(command.Description);
            ILToIr.Add(instruction, command);
            commandList.Add(command);
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
            CommandPair pair = new CommandPair(command, instruction);
            Console.Write(command.Description);
            ILToIr.Add(instruction, command);
            commandList.Add(command);
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
            CommandPair pair = new CommandPair(command, instruction);
            Console.Write(command.Description);
            ILToIr.Add(instruction, command);
            commandList.Add(command);
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
            CommandPair pair = new CommandPair(command, instruction);
            Console.Write(command.Description);
            ILToIr.Add(instruction, command);
            commandList.Add(command);
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
            CommandPair pair = new CommandPair(command, instruction);
            Console.Write(command.Description);
            ILToIr.Add(instruction, command);
            commandList.Add(command);
            return command;
        }

        private Command StoreStaticField(Mono.Cecil.Cil.Instruction instruction)
        {
            var valueVar = variables.PopVariable();
            var fieldRef = instruction.Operand as Mono.Cecil.FieldReference;
            var fieldVar = new Aramani.IR.Variables.FieldVariable(fieldRef);
            Location targetLocation = new VariableLocation(fieldVar);
            var command = new Set(valueVar, targetLocation);
            CommandPair pair = new CommandPair(command, instruction);
            Console.Write(command.Description);
            ILToIr.Add(instruction, command);
            commandList.Add(command);
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
            if (method.Body.Instructions.Count > 0)
            {
                Console.WriteLine("Computing method body stack heigths...");
                ComputeStackHeights(method.Body.Instructions[0], 0);
            }



            var isSplitPoint = true;

            foreach (var instruction in method.Body.Instructions)
            {

                Command command = null;
                isSplitPoint = isSplitPoint
                    || (instruction.OpCode.Code >= Mono.Cecil.Cil.Code.Call
                        && instruction.OpCode.Code >= Mono.Cecil.Cil.Code.Switch)
                    || (instruction.OpCode.Code == Mono.Cecil.Cil.Code.Newobj)
                    || (instruction.OpCode.Code == Mono.Cecil.Cil.Code.Throw)
                    || (instruction.OpCode.Code == Mono.Cecil.Cil.Code.Call)
                    || (instruction.OpCode.Code == Mono.Cecil.Cil.Code.Calli)
                    || (instruction.OpCode.Code == Mono.Cecil.Cil.Code.Throw);
                // TODO: complete it.

                Console.WriteLine("VALUES: " + variables.GetCurrentStackValue() + "," + stackHeights[instruction]);
                variables.SetStackPointer(stackHeights[instruction]);

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
                        Nop(instruction);
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
                        Convert(instruction, null, true);
                        break;

                    case Mono.Cecil.Cil.Code.Ldelema:
                        LoadIndirectArrayElement(instruction, null);
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
                        LoadArrayElement(instruction, null);
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
                        StoreArrayElement(instruction, null);
                        break;
                    case Mono.Cecil.Cil.Code.Call:
                        Call(instruction, false);
                        break;
                    case Mono.Cecil.Cil.Code.Callvirt:
                        Call(instruction, true);
                        break;
                    case Mono.Cecil.Cil.Code.Ret:
                        command = Return(instruction);
                        break;
                    case Mono.Cecil.Cil.Code.Newobj:
                        NewObject(instruction);
                        break;

                        /////////


                    case Mono.Cecil.Cil.Code.Ldvirtftn:
                        break;

                    case Mono.Cecil.Cil.Code.Ldftn:
                        break;

                    case Mono.Cecil.Cil.Code.Ldfld:
                        LoadInstanceField(instruction, false);
                        break;
                    case Mono.Cecil.Cil.Code.Ldflda:
                        LoadInstanceField(instruction, true);
                        break;
                    case Mono.Cecil.Cil.Code.Stfld:
                        StoreInstanceField(instruction);
                        break;
                    case Mono.Cecil.Cil.Code.Ldsfld:
                        LoadStaticField(instruction, false);
                        break;
                    case Mono.Cecil.Cil.Code.Ldsflda:
                        LoadStaticField(instruction, true);
                        break;
                    case Mono.Cecil.Cil.Code.Stsfld:
                        StoreStaticField(instruction);
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

                if (isSplitPoint)
                {
                    basicBlocks.AddBasicBlockEntry(instruction);
                }
                isSplitPoint = false;

            }

            basicBlocks.ComputeBasicBlocks(ILToIr);
            foreach (var command in commandList)
            {
                Console.WriteLine(command.GetHashCode() + ": " + command.Description);
            }
            foreach (var instruction in method.Body.Instructions)
            {
                int stackValue;
                if (stackHeights.TryGetValue(instruction, out stackValue))
                {
                    Console.WriteLine(instruction + " STACK HEIGHT: " + stackValue);
                }
                else
                {
                    Console.WriteLine(instruction + " !!! NO STACK HEIGHT COMPUTED.");
                }

            }
        }



    }
}
