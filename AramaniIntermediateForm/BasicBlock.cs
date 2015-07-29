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
    /// Represents a basic block of a method.
    /// 
    /// </summary>
    public abstract class BasicBlock : IGraphDisplayable
    {
        MethodDefinition itsMethod;

        int entryPosition;
        int endPosition;

        public ICollection<FlowEdge> Predecessors 
        { 
            get; 
            set;
        }

        public ICollection<FlowEdge> Successors
        {
            get;
            set;
        }

        public Instruction LastInstruction 
        { 
            get 
            { 
                return itsMethod.Body.Instructions[endPosition]; 
            } 
        }

        public Instruction FollowingInstruction
        {
            get
            {
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


        public bool IsStartBlock
        {
            get
            {
                return entryPosition == 0;
            }
        }

        public virtual string AsDot()
        {
            var result
                = "\"n" + GetHashCode() + "\" [shape=\"Mrecord\";label=\"";
            result += this.GetHashCode() + "\\n";
            foreach (var instruction in Instructions)
            {
                result += "" + instruction.OpCode.Code + " " + instruction.Operand + " [" + instruction.Offset + "]\\n";
            }
            result += "\"];\n";
            return result;
        }

        public string Description
        {
            get
            {
                var result = "**********\n";
                foreach (var instruction in Instructions)
                {
                    result +=
                        String.Format("CODE: {0} OP: {1} OFFSET: {2}\n",
                                      instruction.OpCode.Code,
                                      instruction.Operand,
                                      instruction.Offset);
                }
                result += "**********";
                return result;
            }
        }

        public BasicBlock(MethodDefinition itsMethod,
                          int entryPosition,
                          int endPosition)
        {
            this.itsMethod = itsMethod;
            this.endPosition = endPosition;
            this.entryPosition = entryPosition;
            Predecessors = new List<FlowEdge>();
            Successors = new List<FlowEdge>();
        }

    }


    public class EndBlock : BasicBlock
    {

        public EndBlock(MethodDefinition itsMethod,
                  int entryPosition,
                  int endPosition)
            : base(itsMethod, entryPosition, endPosition)
        { }

    }

    public class SingleSuccessorBlock : BasicBlock
    {

        public BasicBlock Successor { get; set; }

        public SingleSuccessorBlock
            (MethodDefinition itsMethod,
             int entryPosition,
             int endPosition,
             BasicBlock successor = null) :
            base(itsMethod, entryPosition, endPosition)
        {
            Successor = successor;
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

    public class CallBlock : SingleSuccessorBlock
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

    public class BranchingBlock : BasicBlock
    {
        public BasicBlock IfBranch { get; set; }
        public BasicBlock ElseBranch { get; set; }

        public BranchingBlock
            (MethodDefinition itsMethod,
             int entryPosition,
             int endPosition,
             BasicBlock ifBlock = null,
             BasicBlock elseBlock = null) :
            base(itsMethod, entryPosition, endPosition)
        {
            IfBranch = ifBlock;
            ElseBranch = elseBlock;
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