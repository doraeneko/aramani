using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Aramani.IR.TypeBinder
{

    public static class TypeBinderHelpers
    {
    
            List<Mono.Cecil.GenericParameter> GetAllUninstantiatedParameters(this Mono.Cecil.TypeReference typeDef)
            {
                var result = new List<Mono.Cecil.GenericParameter>();
                if (typeDef.HasGenericParameters)
                {
                    result.AddRange(typeDef.GenericParameters);
                }
                var declaringType = typeDef.DeclaringType;
                if (declaringType != null)
                {
                    result.AddRange(GetAllUninstantiatedParameters(declaringType));
                }
                return result;
            }
            
    }

}