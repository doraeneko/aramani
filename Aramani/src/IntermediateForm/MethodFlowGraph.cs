using System.Linq;
using System.Collections.Generic;
using Mono.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;

using CODE = Mono.Cecil.Cil.Code;

namespace DotNetAnalyser.IntermediateForm
{


    public class MethodFlowGraph
    {

        class Block
        {
            public MethodDefinition itsMethod;

            int entryPosition;
            int endPosition;

            bool IsBlockEndInstruction(Instruction instruction)
            {
                // TODO
                return
                    (instruction.OpCode.Code >= CODE.Beq && instruction.OpCode.Code <= CODE.Callvirt);
            }

            public IEnumerable<Instruction> Instructions
            {
                get
                {
                    var instructions = itsMethod.Body.Instructions;
                    var count = instructions.Count;
                    var foundBlockEnd = false;
                    for (int i = entryPosition; i <= endPosition; i++)
                    {
                        if (foundBlockEnd)
                            break;
                        var result = instructions[i];
                        yield return result;
                        foundBlockEnd = IsBlockEndInstruction(result);
                    }
                }
            }

            public Block(MethodDefinition itsMethod, int entryPosition)
            {
                this.itsMethod = itsMethod;
                this.entryPosition = entryPosition;
            }

        }

        

        class Edge
        {


        }

    }

}

