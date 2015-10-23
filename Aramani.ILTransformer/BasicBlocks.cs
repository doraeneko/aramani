using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aramani.IR.Commands;
using Mono.Cecil.Cil;

namespace Aramani.ILTransformer
{

    public class BasicBlocks
    {
        List<Instruction> jumpTargetsIL;
        List<Command> jumpTargetsIR;
        Dictionary<Branch, Instruction> commandTojumpTargets;
        Dictionary<Instruction, BasicBlocks> instructionToBasicBlocks;
     

        public BasicBlocks()
        {
            jumpTargetsIL = new List<Instruction>();
            jumpTargetsIR = new List<Command>();
            commandTojumpTargets = new Dictionary<Branch, Instruction>();
            instructionToBasicBlocks = new Dictionary<Instruction, BasicBlocks>();
        }

        public void AddBasicBlockEntry(Instruction instruction)
        {
            jumpTargetsIL.Add(instruction);
        }

        public void AddJumpTarget(Branch jumpCmd, Instruction target)
        {
            jumpTargetsIL.Add(target);
            commandTojumpTargets.Add(jumpCmd, target);
        }

        public void ComputeBasicBlocks(ILLocationsToIR transformer)
        {
            foreach (var entry in commandTojumpTargets)
            {
                Console.WriteLine("SET: " + entry.Value + ", " + transformer.Get(entry.Value));
                entry.Key.Target = transformer.Get(entry.Value);
            }

        }
    }
}