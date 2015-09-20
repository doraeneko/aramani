using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Aramani.ILTransformer;

namespace TestDriver
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputFile = @"c:\users\andreas\Documents\sandbox\test.dll";
            var methodName = "TestArguments";
            var typeName = "Test";
            var resolver = new DefaultAssemblyResolver();
            resolver.AddSearchDirectory(System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory());
            var myLibrary 
                = AssemblyDefinition.ReadAssembly(inputFile, new ReaderParameters { AssemblyResolver = resolver });


            // try to locate the method...
            var type = myLibrary.MainModule.GetType(typeName);
            MethodDefinition method = type.Methods.First(methodDef => methodDef.Name == methodName);
            method.Resolve();

            Console.WriteLine("Transforming.");

            ILToIRTransformer transformer = new ILToIRTransformer();
            transformer.TransformMethod(method);

        }
    }
}
