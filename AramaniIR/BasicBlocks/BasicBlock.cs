using System;
using System.Collections.Generic;
using Aramani.IR.Commands;
using Aramani.Base;

namespace Aramani.IR.BasicBlocks
{
    public class  BasicBlock : IDescription
    {
        public BasicBlock()
        {
            Code = new List<Command>();
        }

        public ICollection<Command> Code { get; set; }

        public string Description
        {
            get 
            {
                var result = "";
                foreach (var command in Code)
                {
                    result += command.Description;
                }
                return result;
            }
        }
    }
}
