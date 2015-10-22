using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aramani.IR.Variables;

namespace Aramani.ILTransformer
{
    public class VariableFactory
    {
        Stack<StackVariable> variableStack;
        List<StackVariable> variablePool;
        Dictionary<int, ParameterVariable> parameters;
        Dictionary<int, LocalVariable> locals;
        Queue<StackVariable> stackvariablePool;

        int variableCounter = 0;

        public VariableFactory()
        {
            variablePool = new List<StackVariable>();
            variableStack = new Stack<StackVariable>();
            locals = new Dictionary<int, LocalVariable>();
            parameters = new Dictionary<int, ParameterVariable>();
            stackvariablePool = new Queue<StackVariable>();
            variableCounter = 0;
        }


        public LocalVariable GetLocalVariable(int index)
        {
            LocalVariable result;
            if (locals.TryGetValue(index, out result))
            {
                return result;
            }
            else
            {
                result = new LocalVariable();
                result.ID = index;
                locals.Add(index, result);
                return result;
            }
        }

        public ParameterVariable GetParameter(int index)
        {
            ParameterVariable result;
            if (parameters.TryGetValue(index, out result))
            {
                return result;
            }
            else
            {
                result = new ParameterVariable();
                result.ID = index;
                parameters.Add(index, result);
                return result;
            }
        }


        public StackVariable PushFreshVariable()
        {
            StackVariable variable;
            if (stackvariablePool.Any())
            {
                variable = stackvariablePool.Dequeue();
            }
            else
            {
                variable = new StackVariable();
                variable.ID = variableCounter;
                variableCounter++;
                variablePool.Add(variable);
            }

            variableStack.Push(variable);
            return variable;
        }

        public StackVariable PopVariable()
        {
            if (!variableStack.Any())
            {
                Console.WriteLine("ERROR: Nothing on the stack!");
                return null;
            }
            var result = variableStack.Pop();
            stackvariablePool.Enqueue(result);
            return result;
        }

        public StackVariable TopVariable()
        {
            if (!variableStack.Any())
            {
                Console.WriteLine("ERROR: Nothing on the stack!");
                return null;
            }
            return variableStack.Peek();
        }

    }
}
