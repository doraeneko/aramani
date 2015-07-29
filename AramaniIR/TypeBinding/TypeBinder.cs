using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;


namespace Aramani.IR.TypeBinder
{

    class TypeBinder
    {

        Dictionary<Mono.Cecil.GenericParameter, Mono.Cecil.TypeReference> bindings;


        void Bind(GenericParameter typeParameter, TypeReference typeRef)
        {
            bindings[typeParameter] = typeRef;
        }

        void BuildBindings(Mono.Cecil.TypeReference typeRef)
        {
            var asGenericInstance = typeRef as GenericInstanceType;
            if (asGenericInstance != null && asGenericInstance.HasGenericArguments)
            {
                bool matches = asGenericInstance.GenericArguments.Count == asGenericInstance.GenericParameters.Count;
                // add parameters

            }

        }
        public TypeBinder(Mono.Cecil.MethodReference groundInstance)
        {
            bindings = new Dictionary<Mono.Cecil.GenericParameter, Mono.Cecil.TypeReference>();

        }

    }
}