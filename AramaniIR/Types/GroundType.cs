using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aramani.IR.Types
{
    public class GroundType : Aramani.Base.IDescription
    {
        Mono.Cecil.TypeReference DotnetType;

        public GroundType(Mono.Cecil.TypeReference dotnetType)
        {
            DotnetType = dotnetType;
        }

        public string Description
        {
            get
            {
                return DotnetType.FullName;
            }
        }
    }
}
