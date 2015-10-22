using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AramaniIR.Types
{
    public interface IBindTypes
    {
        Aramani.IR.Types.GroundType BindType(Mono.Cecil.TypeReference typeRef);

    }
}
