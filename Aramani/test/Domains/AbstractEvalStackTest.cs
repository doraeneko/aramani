using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;


namespace DotNetAnalyser.Domains
{

    [TestFixture]
    public class AbstractEvalStackTest
    {
   
        [Test]
        public void PushAndPopTest()
        {
            Domains.AbstractEvalStack<VariableCharaterizationDomain> stack 
                = new AbstractEvalStack<VariableCharaterizationDomain>(4);
            stack.Push(new VariableCharaterizationDomain(VariableCharaterization.NULL));
            stack.Push(new VariableCharaterizationDomain(VariableCharaterization.TOP));
            stack.Push(new VariableCharaterizationDomain(VariableCharaterization.BOTTOM));
            stack.Push(new VariableCharaterizationDomain(VariableCharaterization.NULL));
            for (int i = 0; i < 4; i++ )
                stack.Pop();
            stack.Push(new VariableCharaterizationDomain(VariableCharaterization.NULL));
            
            var newStack = new AbstractEvalStack<VariableCharaterizationDomain>(4);
            newStack.Push(new VariableCharaterizationDomain(VariableCharaterization.TOP));
            newStack.Push(new VariableCharaterizationDomain(VariableCharaterization.TOP));
            newStack.Push(new VariableCharaterizationDomain(VariableCharaterization.TOP));
            newStack.Push(new VariableCharaterizationDomain(VariableCharaterization.NULL));
            Assert.IsTrue(stack.IsSubsetOrEqual(newStack) && newStack.IsSubsetOrEqual(stack));
        }
    }
}