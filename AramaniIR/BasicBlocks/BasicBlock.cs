using System;
using System.Collections.Generic;
using Aramani.IR.Commands;
using Aramani.Base;

namespace Aramani.IR.BasicBlocks
{
    public class  BasicBlock : IDescription
    {
        public BasicBlock(int id)
        {
            Code = new List<Command>();
            ID = id;
        }

        public ICollection<Command> Code { get; set; }
        public BasicBlock Next { get; set; }
        public int ID { get; set; }

        public string Description
        {
            get 
            {
                var result = "BASICBLOCK " + ID + "\n";
                foreach (var command in Code)
                {
                    result += "  " + command.Description + "\n";
                }
                if (Next != null)
                {
                    result += "END BASICBLOCK, GOTO " + Next.ID + "\n";
                }
                else
                {
                    result += "END BASICBLOCK\n";
                }
                return result;
            }
        }
    }
}
