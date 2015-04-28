using System;
using System.Linq;
using System.Collections.Generic;
using Mono.Cecil;
using DotNetAnalyser.Analyses;
using Mono.Cecil.Cil;

namespace DotNetAnalyser.App
{
    
    public class Analyser
    {
        public static void Main()
        {
            var inputFile = @"c:\users\andreas\Documents\sandbox\test1.exe";
            var methodName = "Foo";
            var typeName = "A";
            var resolver = new DefaultAssemblyResolver();
            resolver.AddSearchDirectory(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory());
            var myLibrary 
                = AssemblyDefinition.ReadAssembly(inputFile, new ReaderParameters { AssemblyResolver = resolver });


            // try to locate the method...
            var type = myLibrary.MainModule.GetType(typeName);
            MethodDefinition method = type.Methods.First(methodDef => methodDef.Name == methodName);
            method.Resolve();

            Console.WriteLine("Performing a few simple analyses.");

            DotNetAnalyser.IntermediateForm.MethodFlowGraph g 
                = new DotNetAnalyser.IntermediateForm.MethodFlowGraph(method);

            g.PrintDescription();
            g.ComputeJumpTargets();
            g.GenerateBasicBlocks();
            g.PrintDescription();
            System.IO.File.WriteAllText(@"C:\Users\andreas\Documents\sandbox\graph.dot", g.AsDot());


            Console.WriteLine(">>>\n" + g.AsDot());
            var input = new DotNetAnalyser.Domains.TypeAnalysisMethodFrame(method);

           
            input.Negate();
            var bb1 = g.BasicBlocks[0];


            input.ComputeEffect(bb1);
            Console.WriteLine("AFTER FIRST BLOCK:" + input);
            Console.Read();
            return;
            
            var bb2 = g.BasicBlocks[1];
            var bb3 = g.BasicBlocks[2];
            var old = input.Clone() as DotNetAnalyser.Domains.TypeAnalysisMethodFrame;
            Console.WriteLine("AFTER FIRST BLOCK:" + input);
            var out1 = input.Clone() as DotNetAnalyser.Domains.TypeAnalysisMethodFrame;
            

            out1.ComputeEffect(bb2);

            Console.WriteLine("O1:" + input.Description());
            Console.WriteLine("O2:" + out1.Description());
            input.UnionWith(out1);

            Console.WriteLine("UNION:" + input.Description());
            Console.WriteLine("OLD:" + old.Description());
            Console.Read();
            return;
#if BLUBB
            // Type analysis
            Console.WriteLine("Performing type analysis.");
            AnalysisIntraAccumulating<Domains.TypeSet> typeAnalysis = new AnalysisIntraAccumulating<Domains.TypeSet>();
            var typeAnalysisResult = typeAnalysis.Perform(method, new Domains.TypeSet(), new TypeAnalysis());
            foreach (var el in typeAnalysisResult.Keys)
            {
                Console.WriteLine("Type: " + el);
            }

            // Local Call analysis
            Console.WriteLine("Performing local call analysis.");
            AnalysisIntraAccumulating<Domains.ReferenceSet<MethodDefinition>> methodAnalysis
                = new AnalysisIntraAccumulating<Domains.ReferenceSet<MethodDefinition>>();
            var methodAnalysisResult = methodAnalysis.Perform(method, new Domains.ReferenceSet<MethodDefinition>(), new LocalCallAnalysis());
            Console.WriteLine("Result: \n" + methodAnalysisResult.ToString());

            // Transitive Call analysis
            Console.WriteLine("Performing transitive call analysis.");
            TransitiveCallAnalysis transitiveCallAnalysis = new TransitiveCallAnalysis(method);
            var transitiveCallResult = transitiveCallAnalysis.Perform();
            Console.WriteLine("Final result: \n" + transitiveCallResult.ToString());
            Console.WriteLine("# reachable methods: " + transitiveCallAnalysis.GetAllMethodDefinitions().Cardinality);

#endif
            Domains.AbstractEvalStack<Domains.VariableCharaterizationDomain> stack = new Domains.AbstractEvalStack<Domains.VariableCharaterizationDomain>(4);
            var safe = (Domains.AbstractEvalStack<Domains.VariableCharaterizationDomain>)stack.Clone();
            stack.Push(new Domains.VariableCharaterizationDomain(Domains.VariableCharaterization.NULL));
            stack.Push(new Domains.VariableCharaterizationDomain(Domains.VariableCharaterization.TOP));
            stack.Push(new Domains.VariableCharaterizationDomain(Domains.VariableCharaterization.BOTTOM));
            stack.Push(new Domains.VariableCharaterizationDomain(Domains.VariableCharaterization.NULL));
            Console.WriteLine("STACK: \n" + stack.ToString());
            stack.Pop();
            Console.WriteLine("STACK: \n" + stack.ToString());
            stack.Pop();
            Console.WriteLine("STACK: \n" + stack.ToString());
            stack.Pop();
            Console.WriteLine("STACK: \n" + stack.ToString());
            stack.Pop();
            Console.WriteLine("STACK: \n" + stack.ToString());
            stack.Push(new Domains.VariableCharaterizationDomain(Domains.VariableCharaterization.NULL));

            Console.WriteLine("STACK: \n" + stack.ToString());

            var newStack = new Domains.AbstractEvalStack<Domains.VariableCharaterizationDomain>(4);
            newStack.Push(new Domains.VariableCharaterizationDomain(Domains.VariableCharaterization.TOP));
            newStack.Push(new Domains.VariableCharaterizationDomain(Domains.VariableCharaterization.TOP));
            newStack.Push(new Domains.VariableCharaterizationDomain(Domains.VariableCharaterization.TOP));
            newStack.Push(new Domains.VariableCharaterizationDomain(Domains.VariableCharaterization.NULL));
            Console.WriteLine("NEW STACK: \n" + newStack.ToString());

            Console.WriteLine("SUBSETEQ: " + stack.IsSubsetOrEqual(newStack) + "," + newStack.IsSubsetOrEqual(stack));
            //var frame = new Domains.AbstractMethodFrame<Domains.ReferenceSet<TypeDefinition>>(method);
            //Console.WriteLine(frame.ToString());
            Console.Read();   

        }

    }
}