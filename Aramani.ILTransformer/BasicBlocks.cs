using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aramani.IR.Commands;
using Mono.Cecil.Cil;
using Aramani.IR.BasicBlocks;

namespace Aramani.ILTransformer
{

    public class BasicBlocks
    {
        public Dictionary<Instruction, bool> JumpTargetsIL;
        public List<Command> JumpTargetsIR;
        public List<Aramani.IR.BasicBlocks.BasicBlock> Blocks;

        Dictionary<Branch, Instruction> CommandTojumpTargets;
        Dictionary<Instruction, BasicBlocks> InstructionToBasicBlocks;
        Dictionary<Command, Aramani.IR.BasicBlocks.BasicBlock> CommandsToBasicBlocks;
        int blockCounter = 0;

        public BasicBlocks()
        {
            JumpTargetsIL = new Dictionary<Instruction, bool>();
            JumpTargetsIR = new List<Command>();
            CommandTojumpTargets = new Dictionary<Branch, Instruction>();
            InstructionToBasicBlocks = new Dictionary<Instruction, BasicBlocks>();
            Blocks = new List<IR.BasicBlocks.BasicBlock>();
            CommandsToBasicBlocks = new Dictionary<Command, IR.BasicBlocks.BasicBlock>();
        }

        public IR.BasicBlocks.BasicBlock CurrentBasicBlock { get; set; }

        public void PushNewBasicBlock()
        {
            if (CurrentBasicBlock != null 
                && CurrentBasicBlock.Code.Count == 0)
            {
                return;
            }
            if (CurrentBasicBlock != null)
            {
                Blocks.Add(CurrentBasicBlock);
            }
            CurrentBasicBlock = new IR.BasicBlocks.BasicBlock(blockCounter++);
        }

        public void AddInstructionAsJumpTarget(Instruction instruction)
        {
            JumpTargetsIL.Add(instruction, true);
        }

        public bool IsJumpTarget(Instruction instruction)
        {
            return JumpTargetsIL.ContainsKey(instruction);
        }

        public BasicBlock GetBlock(Command command)
        {
            BasicBlock result = null;
            CommandsToBasicBlocks.TryGetValue(command, out result);
            return result;
        }

        public void AddCommandToCurrentBasicBlock(Command command, Mono.Cecil.Cil.Instruction lastInstruction)
        {
            CommandsToBasicBlocks.Add(command, CurrentBasicBlock);
            CurrentBasicBlock.Code.Add(command);
        }

        public void RegisterBranchJumpTargets(ICollection<Instruction> instructions)
        {
            // Insert jump targets 
            foreach (var instruction in instructions)
            {
                if (instruction.OpCode.OperandType == Mono.Cecil.Cil.OperandType.InlineSwitch)
                {
                    foreach (var target in (Mono.Cecil.Cil.Instruction[])instruction.Operand)
                    {
                        AddInstructionAsJumpTarget(target);
                    }
                }
                else if (instruction.OpCode.OperandType == Mono.Cecil.Cil.OperandType.InlineBrTarget ||
                    instruction.OpCode.OperandType == Mono.Cecil.Cil.OperandType.ShortInlineBrTarget)
                {
                    AddInstructionAsJumpTarget(instruction.Operand as Mono.Cecil.Cil.Instruction);
                }
            }
        }

        public void BackpatchJumpTargets(ILLocationsToIR ILToIR)
        {
            // phase 3
            foreach (var basicBlock in Blocks)
            {
                foreach (var command in basicBlock.Code)
                {
                    var asBranch = command as Branch;
                    if (asBranch == null)
                        continue;
                    var instruction = ILToIR.Get(command);
                    if (instruction == null)
                        continue;

                    if (instruction.OpCode.OperandType == Mono.Cecil.Cil.OperandType.InlineBrTarget ||
                        instruction.OpCode.OperandType == Mono.Cecil.Cil.OperandType.ShortInlineBrTarget)
                    {
                        var target = instruction.Operand as Mono.Cecil.Cil.Instruction;
                        var targetCommand = ILToIR.Get(target);
                        if (targetCommand != null)
                        {
                            asBranch.Target = GetBlock(targetCommand[0]);
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
        }

    }
}