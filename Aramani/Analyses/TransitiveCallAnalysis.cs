using System;
using System.Collections.Generic;
using Mono.Cecil;
using Mono.Cecil.Cil;
using CODE = Mono.Cecil.Cil.Code;
using Aramani.Domains;

namespace Aramani.Analyses
{
    class TransitiveCallAnalysis 
    {

        AnalysisIntraAccumulating<ReferenceSet<MethodDefinition>> localAnalysis;
        LocalCallAnalysis localCallAnalysis;
        MethodDefinition initialMethod;
        ReferenceSet<MethodDefinition> allDefinitions;


        public TransitiveCallAnalysis(MethodDefinition initialMethodDef)
        {
            localAnalysis = new AnalysisIntraAccumulating<ReferenceSet<MethodDefinition>>();
            localCallAnalysis = new LocalCallAnalysis();
            initialMethod = initialMethodDef;
            allDefinitions = new ReferenceSet<MethodDefinition>();
            allDefinitions.Add(initialMethodDef);
        }

        /// <summary>
        /// Returns true if something has changed
        /// </summary>
        public void ComputeEffect(MethodSummary<ReferenceSet<MethodDefinition>> input)
        {
            ReferenceSet<MethodDefinition> buffer = new ReferenceSet<MethodDefinition>();
            foreach (var definition in allDefinitions.GetEntries())
            {
                if (!input.Contains(definition))
                {
                    var callSet = input[definition];
                    localAnalysis.Perform(definition, callSet, localCallAnalysis);
                    var resultCopy = callSet.Clone() as ReferenceSet<MethodDefinition>;
                    foreach (var method in resultCopy.GetEntries())
                    {
                        // callSet.UnionWith(input[method]); // transitive step.
                        buffer.Add(method);
                    }
                }
            }
            allDefinitions.UnionWith(buffer);
        }

        public ReferenceSet<MethodDefinition> GetAllMethodDefinitions()
        {
            return allDefinitions;
        }

        public MethodSummary<ReferenceSet<MethodDefinition>> Perform()
        {
            MethodSummary<ReferenceSet<MethodDefinition>> tempValue;
            MethodSummary<ReferenceSet<MethodDefinition>> value = new MethodSummary<ReferenceSet<MethodDefinition>>();

            do
            {
                tempValue = value.Clone() as MethodSummary<ReferenceSet<MethodDefinition>>;
                ComputeEffect(value);
                //Console.WriteLine("New Result:");
                //Console.WriteLine(value.ToString());

                Console.WriteLine("############################################################");
                if (value.IsSubsetOrEqual(tempValue))
                    break;

            }
            while (true);
            return value;
        }
    }
}
