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

        List<StackVariable> variablePool;
        Dictionary<int, ParameterVariable> parameters;
        Dictionary<int, LocalVariable> locals;
        StackVariable[] variableStack;

        int variableCounter = 0;
        int maxStackValue;

        public VariableFactory(int maxStackSize)
        {
            variablePool = new List<StackVariable>();


            locals = new Dictionary<int, LocalVariable>();
            parameters = new Dictionary<int, ParameterVariable>();

            variableStack = new StackVariable[maxStackSize];
            for (int i = 0; i < maxStackSize; ++i)
            {
                variableStack[i] = new StackVariable();
                variableStack[i].ID = i;
            }
            variableCounter = -1;
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

        public void SetStackPointer(int position)
        {
            variableCounter = position-1;
        }

        public StackVariable PushFreshVariable()
        {
            variableCounter++;
            return variableStack[variableCounter];

        }

        public StackVariable PopVariable()
        {
            variableCounter--;
            return variableStack[variableCounter+1];
        }

        public StackVariable TopVariable()
        {
            return variableStack[variableCounter];
        }

        public int GetCurrentStackValue()
        {
            return variableCounter;
        }

    }
}
