using System;
using System.Linq;
using System.Collections.Generic;
using Mono.Cecil;


namespace DotNetAnalyser.Analyser.Driver
{


    public class Analyser
    {
        public static void Main()
        {
            var inputFile = "Aramani.exe";
            var methodName = "Main";
            var typeName = "DotNetAnalyser.Analyser.Driver.Analyser";
            
            var myLibrary = AssemblyDefinition.ReadAssembly(inputFile);
            // try to locate the method...
            var type = myLibrary.MainModule.GetType(typeName);
            var s = new string('c', 20);



            if (type != null)
            {
                var method = type.Methods.First(methodDef => methodDef.Name == methodName);
                if (method != null)
                {
                    Console.WriteLine("FOUND: " + method);
                    AnalysisIntraAccumulating<Domains.TypeSet> analysis = new AnalysisIntraAccumulating<Domains.TypeSet>();
                    var result = analysis.Perform(method, new Domains.TypeSet(), new TypeAnalysis());
                    foreach (var el in result.Keys)
                    {
                        Console.WriteLine("Type: " + el);
                    }

                    if (method.Body.HasVariables)
                    {
                        foreach (var el in method.Body.Variables)
                        {
                            Console.WriteLine("RV: " + el + "," + method.Body.MaxStackSize);
                        }
                        
                        // VAR -> { ?, true, false, 0, !=0, ... }
                        // LOC: (VAR => Type) 
                        // LOC: (STACK[2] => TYPE)
                        // LOC: (ARG => TYPE)
                    }
                }
            }
            else
            {
                Console.WriteLine("Could not find anything.");
            }



        }

    }
}