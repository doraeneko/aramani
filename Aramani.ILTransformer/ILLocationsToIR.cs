using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aramani.IR.Commands;
using Mono.Cecil.Cil;

namespace Aramani.ILTransformer
{

    public class ILLocationsToIR
    {
    
        Dictionary<Mono.Cecil.Cil.Instruction, Aramani.IR.Commands.Command> cilToIR;
        Dictionary<Aramani.IR.Commands.Command, Mono.Cecil.Cil.Instruction> IRtoCil;

        public ILLocationsToIR()
        {
            cilToIR = new Dictionary<Instruction, Command>();
            IRtoCil = new Dictionary<Command, Instruction>();
        }

        public void Add(Instruction instruction, Command command)
        {
            cilToIR.Add(instruction, command);
            IRtoCil.Add(command, instruction);
        }

        public Instruction Get(Command command)
        {
            Instruction instruction = null;
            IRtoCil.TryGetValue(command, out instruction);
            return instruction;
        }

        public Command Get(Instruction instruction)
        {
            Command command = null;
            cilToIR.TryGetValue(instruction, out command);
            return command;
        }
    
    }

}
