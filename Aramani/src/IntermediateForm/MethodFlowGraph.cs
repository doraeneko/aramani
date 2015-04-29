
using System;
using System.Linq;
using System.Collections.Generic;
using Mono.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

using CODE = Mono.Cecil.Cil.Code;

namespace DotNetAnalyser.IntermediateForm
{

    /// <summary>
    /// Contains a control flow graph of a bytecode method.
    /// </summary>
    public class MethodFlowGraph
    {

        Mono.Cecil.MethodDefinition bytecodeMethod;
        List<BasicBlock> basicBlocks;

        public List<BasicBlock> BasicBlocks { get { return basicBlocks; } }

        public MethodFlowGraph(Mono.Cecil.MethodDefinition method)
        {
            bytecodeMethod = method;
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
        public void PrintDescription()
        {
            foreach (var instruction
                     in bytecodeMethod.Body.Instructions)
            {

                System.Console.WriteLine(
                    "CODE: {0} OP: {1} OFF: {2}", instruction.OpCode.Code, instruction.Operand, instruction.Offset);
            }

            
            foreach (var block in basicBlocks??(new List<BasicBlock>()))
            {
                block.Description();
            }
        }

        public static bool IsBranchCommand
            (Mono.Cecil.Cil.Instruction instruction)
        {
            return 
                (instruction.OpCode.Code >= CODE.Br_S 
                 && instruction.OpCode.Code <= Code.Blt_Un)
                || instruction.OpCode.Code == Code.Ret
                || instruction.OpCode.Code == Code.Rethrow;
        }

        public static bool IsCallCommand
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
                    && (jumpTargets.Contains(currentInstruction) || IsCallCommand(currentInstruction)))
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

            // Now generate the edges
            foreach (var block in basicBlocks)
            {
                var singleSuccBlock = block as SingleSuccessorBlock;
                if (singleSuccBlock != null)
                {
                    if (singleSuccBlock.LastInstruction.OpCode.Code == CODE.Br 
                        || singleSuccBlock.LastInstruction.OpCode.Code == CODE.Br_S)
                    {
                        // actual jump
                        singleSuccBlock.Successor 
                            = headerToBlock[singleSuccBlock.LastInstruction.Operand as Instruction];
                    }
                    else
                    {
                        singleSuccBlock.Successor
                            = headerToBlock[singleSuccBlock.NextInstruction as Instruction];
                    }
                    continue;
                }
                var branchBlock = block as BranchingBlock;
                if (branchBlock != null)
                {
                    branchBlock.IfBranch
                        = headerToBlock[branchBlock.LastInstruction.Operand as Instruction];
                    branchBlock.ElseBranch
                        = headerToBlock[branchBlock.NextInstruction];
                }
            }
        }

        public abstract class BasicBlock
        {
            public MethodDefinition itsMethod;

            int entryPosition;
            int endPosition;

            public Instruction LastInstruction { get { return itsMethod.Body.Instructions[endPosition];  } }
            
            public Instruction NextInstruction { 
                get {
                    if (itsMethod.Body.Instructions.Count > endPosition + 1)
                        return itsMethod.Body.Instructions[endPosition + 1];
                    else
                        return null;
                }
            }

            public IEnumerable<Instruction> Instructions
            {
                get
                {
                    var instructions = itsMethod.Body.Instructions;
                    for (int i = entryPosition; i <= endPosition; i++)
                    {
                        yield return instructions[i];
                    }
                }
            }

            public virtual string AsDot()
            {
                var result
                    = "\"n" + GetHashCode() + "\" [shape=\"Mrecord\";label=\"";
                foreach (var instruction in Instructions)
                {
                    result += "" + instruction.OpCode.Code + " " + instruction.Operand + " [" + instruction.Offset + "]\\n";
                }
                result += "\"];\n";
                return result;
            }

            public void Description()
            {
                Console.WriteLine("**********");
                foreach (var instruction in Instructions)
                {
                    Console.WriteLine
                        ("CODE: {0} OP: {1} OFF: {2}",
                         instruction.OpCode.Code, 
                         instruction.Operand, 
                         instruction.Offset);
                }
                Console.WriteLine("**********");

            }

            public BasicBlock(MethodDefinition itsMethod, 
                              int entryPosition, 
                              int endPosition)
            {
                this.itsMethod = itsMethod;
                this.endPosition = endPosition;
                this.entryPosition = entryPosition;
            }

            public abstract IEnumerable<BasicBlock> Successors();

        }


        class EndBlock : BasicBlock
        {

            public EndBlock(MethodDefinition itsMethod,
                      int entryPosition,
                      int endPosition)
                : base(itsMethod, entryPosition, endPosition)
            { }

            public override IEnumerable<BasicBlock> Successors()
            {
                return new BasicBlock[] { };
            }

        }

        class SingleSuccessorBlock: BasicBlock
        {

            public BasicBlock Successor { get; set; }
            
            public SingleSuccessorBlock
                (MethodDefinition itsMethod, 
                 int entryPosition, 
                 int endPosition,
                 BasicBlock successor = null):
                base (itsMethod, entryPosition, endPosition)
            {
                Successor = successor;
            }

            public override IEnumerable<BasicBlock> Successors()
            {
                return new BasicBlock[] { Successor };
            }

            public override string AsDot()
            {
                var result = base.AsDot();
                result = result + "n" 
                    + this.GetHashCode() + "-> n" 
                    + Successor.GetHashCode() + ";\n";
                return result;
            }

        }

        class CallBlock : SingleSuccessorBlock
        {



            public CallBlock
                (MethodDefinition itsMethod,
                 int entryPosition,
                 int endPosition,
                 BasicBlock successor = null) :
                base(itsMethod, entryPosition, endPosition, successor)
            {
            }


        }

        class BranchingBlock: BasicBlock
        {
           public BasicBlock IfBranch { get; set; }
           public BasicBlock ElseBranch { get; set; }

            public BranchingBlock
                (MethodDefinition itsMethod, 
                 int entryPosition, 
                 int endPosition,
                 BasicBlock ifBlock = null,
                 BasicBlock elseBlock = null):
                base (itsMethod, entryPosition, endPosition)
            {
                IfBranch = ifBlock;
                ElseBranch = elseBlock;
            }

            public override IEnumerable<BasicBlock> Successors()
            {
                return new BasicBlock[] { IfBranch, ElseBranch };
            }


            public override string AsDot()
            {
                var result = base.AsDot();
                result = result + "n"
                    + this.GetHashCode() + "-> n"
                    + IfBranch.GetHashCode() + " [label=\"IF\"];\n";
                result = result + "n"
                    + this.GetHashCode() + "-> n"
                    + ElseBranch.GetHashCode() + " [label=\"ELSE\"];\n";
                return result;
            }
        }

        

    }

}

