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
        Dictionary<Instruction, BasicBlocks> instructionToBasicBlocks;

        public BasicBlocks()
        {
            jumpTargetsIL = new List<Instruction>();
            jumpTargetsIR = new List<Command>();
            instructionToBasicBlocks = new Dictionary<Instruction, BasicBlocks>();
        }

        public void AddBasicBlockEntry(Instruction instruction, Command command)
        {
            jumpTargetsIL.Add(instruction);
            jumpTargetsIR.Add(command);
        }
    }
}