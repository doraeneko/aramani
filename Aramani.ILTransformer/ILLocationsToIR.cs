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
    
        Dictionary<Mono.Cecil.Cil.Instruction, List<Command>> cilToIR;
        Dictionary<Aramani.IR.Commands.Command, Mono.Cecil.Cil.Instruction> IRtoCil;

        public ILLocationsToIR()
        {
            cilToIR = new Dictionary<Instruction, List<Command>>();
            IRtoCil = new Dictionary<Command, Instruction>();
        }

        public void AddCommand(Instruction instruction, Command command)
        {
            List<Command> commands;
            if (!cilToIR.TryGetValue(instruction, out commands))
            {
                commands.Add(command);
            }
            else
            {
                commands = new List<Command>();
                cilToIR.Add(instruction, commands);
            }
            commands.Add(command);

            IRtoCil.Add(command, instruction);
        }

        public Instruction Get(Command command)
        {
            Instruction instruction = null;
            IRtoCil.TryGetValue(command, out instruction);
            return instruction;
        }

        public List<Command> Get(Instruction instruction)
        {
            List<Command> command = null;
            cilToIR.TryGetValue(instruction, out command);
            return command;
        }
    
    }

}
