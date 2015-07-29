
using System;
using System.Linq;
using System.Collections.Generic;
using Mono.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Aramani.Base;
using CODE = Mono.Cecil.Cil.Code;

namespace Aramani.IntermediateForm
{

    /// <summary>
    /// A control flow graph of a bytecode method.
    /// </summary>
    public class MethodFlowGraph : IGraphDisplayable
    {

        Mono.Cecil.MethodDefinition bytecodeMethod;
        List<BasicBlock> basicBlocks;
        List<FlowEdge> edges;

        public List<BasicBlock> BasicBlocks { get { return basicBlocks; } }

        public BasicBlock StartBlock
        {
            get
            {
                if (basicBlocks != null
                    && basicBlocks.Count > 0)
                {
                    return basicBlocks[0];
                }
                return null;
            }
        }

        public MethodFlowGraph(Mono.Cecil.MethodDefinition method)
        {
            bytecodeMethod = method;
            edges = new List<FlowEdge>();
        }

        public string AsDot()
        {
            var result = "digraph {\n";

            foreach (var block in basicBlocks ?? (new List<BasicBlock>()))
            {
                result += block.AsDot();
            }

            result += "\n}\n";
            return result;
        }

        public string Description
        {
            get
            {
                var result = "";
                foreach (var block in basicBlocks ?? (new List<BasicBlock>()))
                {
                    result += block.Description;
                }
                return result;
            }
        }

        FlowEdge RegisterEdge
            (FlowEdgeKind kind,
             BasicBlock source, 
             BasicBlock target, 
             Mono.Cecil.Cil.Instruction branchInstruction = null)
        {
            var result = new FlowEdge();
            result.BranchInstruction = branchInstruction;
            result.Source = source;
            result.Target = target;
            result.Kind = kind;
            edges.Add(result);
            source.Successors.Add(result);
            target.Predecessors.Add(result);
            return result;
        }

        static bool IsBranchCommand
            (Mono.Cecil.Cil.Instruction instruction)
        {
            return 
                (instruction.OpCode.Code >= CODE.Br_S 
                 && instruction.OpCode.Code <= Code.Blt_Un)
                 || instruction.OpCode.Code == Code.Ret
                 || instruction.OpCode.Code == Code.Rethrow;
        }

        static bool IsCallCommand
            (Mono.Cecil.Cil.Instruction instruction)
        {
            return
                instruction.OpCode.Code == Code.Call
                || instruction.OpCode.Code == Code.Calli
                || instruction.OpCode.Code == Code.Callvirt;
        }

        public HashSet<Instruction> ComputeJumpTargets()
        {
            var result = new HashSet<Instruction>();
            foreach (var instruction
                     in bytecodeMethod.Body.Instructions)
            {
                if (IsBranchCommand(instruction))
                {
                    // add target to set
                    result.Add(instruction.Operand as Instruction);
                }
            }
            return result;
        }


        public void GenerateBasicBlocks()
        {
            basicBlocks = new List<BasicBlock>();

            // First step: Compute jump targets
            var jumpTargets = ComputeJumpTargets();
            var startPosition = 0;
            var instructions = bytecodeMethod.Body.Instructions;
            Dictionary<Instruction, BasicBlock> headerToBlock =
                new Dictionary<Instruction, BasicBlock>();

            for (int counter = 0; 
                 counter < instructions.Count;
                 counter++)
            {
                var currentInstruction = instructions[counter];
                // If the current instruction is the target of a jump,
                // unify preceeding instructions into a basic block.
                if (startPosition < counter 
                    && (jumpTargets.Contains(currentInstruction) 
                        || IsCallCommand(currentInstruction)))
                {
                    var block = new SingleSuccessorBlock
                        (bytecodeMethod, startPosition, counter - 1);
                    headerToBlock.Add(instructions[startPosition], block);
                    basicBlocks.Add(block);
                    startPosition = counter;
                }
                if (IsBranchCommand(currentInstruction) || IsCallCommand(currentInstruction))
                {
                    BasicBlock block = null;
                    if (currentInstruction.OpCode.Code == CODE.Ret
                        || currentInstruction.OpCode.Code == CODE.Rethrow)
                    {
                        block = new EndBlock(bytecodeMethod, startPosition, counter);
                    }
                    else if (currentInstruction.OpCode.Code == CODE.Br 
                        || currentInstruction.OpCode.Code == CODE.Br_S)
                    {
                        block = new SingleSuccessorBlock(bytecodeMethod, startPosition, counter);
                    }
                    else if (IsCallCommand(currentInstruction))
                    {
                        block = new CallBlock(bytecodeMethod, startPosition, counter);
                    }
                    else
                    {
                        block = new BranchingBlock(bytecodeMethod, startPosition, counter);
                    }
                    headerToBlock.Add(instructions[startPosition], block);
                    basicBlocks.Add(block);
                    startPosition = counter + 1;
                }
            }
            if (startPosition < instructions.Count)
            {
                var block = new EndBlock(bytecodeMethod, startPosition, instructions.Count - 1);
                headerToBlock.Add(instructions[startPosition], block);
                basicBlocks.Add(block);
            }

            // Generate the edges
            foreach (var block in basicBlocks)
            {
                var singleSuccBlock = block as SingleSuccessorBlock;
                if (singleSuccBlock != null)
                {
                    BasicBlock successor;
                    if (singleSuccBlock.LastInstruction.OpCode.Code == CODE.Br 
                        || singleSuccBlock.LastInstruction.OpCode.Code == CODE.Br_S)
                    {
                        successor = 
                            headerToBlock[singleSuccBlock.LastInstruction.Operand as Instruction];
                    }
                    else
                    {
                        successor = 
                            headerToBlock[singleSuccBlock.FollowingInstruction as Instruction];

                    }
                    singleSuccBlock.Successor = successor;
                    RegisterEdge(FlowEdgeKind.NORMAL, singleSuccBlock, successor, null);
                    continue;
                }
                var branchBlock = block as BranchingBlock;
                if (branchBlock != null)
                {
                    var ifSuccessorBlock 
                        = headerToBlock[branchBlock.LastInstruction.Operand as Instruction];
                    branchBlock.IfBranch
                        = ifSuccessorBlock;
                    RegisterEdge(FlowEdgeKind.COND_IF, branchBlock, ifSuccessorBlock, branchBlock.LastInstruction);
                    var elseSuccessorBlock 
                        = headerToBlock[branchBlock.FollowingInstruction];
                    branchBlock.ElseBranch
                        = elseSuccessorBlock;
                    RegisterEdge(FlowEdgeKind.COND_ELSE, branchBlock, elseSuccessorBlock, branchBlock.LastInstruction);
                }
            }
        }
    }
}

