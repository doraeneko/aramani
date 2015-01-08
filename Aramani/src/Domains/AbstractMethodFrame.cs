using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;


namespace DotNetAnalyser.Domains
{

    /// <summary>
    /// Models a method frame.
    /// </summary>
    /// <typeparam name="C"></typeparam>
    class AbstractMethodFrame<C> : IDomainElement<AbstractMethodFrame<C>>
        where C: class, IDomainElement<C>, new()
    {

        AbstractTuple<C> theLocalVariables;
        public AbstractTuple<C> LocalVariables
        {
            get { return theLocalVariables; }
        }
        AbstractEvalStack<C> theStack;
        public AbstractEvalStack<C> Stack
        {
            get { return theStack; }
        }

        public AbstractMethodFrame(MethodDefinition methodDef)
        {
            var stackSize = methodDef.Body.MaxStackSize;
            theStack = new AbstractEvalStack<C>(stackSize);
            int variablesCount = 0;
            if (methodDef.Body.HasVariables)
                variablesCount = methodDef.Body.Variables.Count;
            theLocalVariables = new AbstractTuple<C>(variablesCount);
        }

        public AbstractMethodFrame(int stackSize, int variablesCount)
        {
            theStack = new AbstractEvalStack<C>(stackSize);
            theLocalVariables = new AbstractTuple<C>(variablesCount);
        }


   
        #region IDomainElement implementation

        public void UnionWith(AbstractMethodFrame<C> element)
        {
            theStack.UnionWith(element.theStack);
            theLocalVariables.UnionWith(element.theLocalVariables);
        }

        public void JoinWith(AbstractMethodFrame<C> element)
        {
            theStack.JoinWith(element.theStack);
            theLocalVariables.JoinWith(element.theLocalVariables);

        }

        public void Negate()
        {
            theStack.Negate();
            theLocalVariables.Negate();
        }

        public void WidenWith(AbstractMethodFrame<C> element)
        {
            theStack.WidenWith(element.theStack);
            theLocalVariables.WidenWith(element.theLocalVariables);        
        }


        public bool IsSubsetOrEqual(AbstractMethodFrame<C> element)
        {
            return (theStack.IsSubsetOrEqual(element.theStack)
                && theLocalVariables.IsSubsetOrEqual(element.theLocalVariables));
        }

        public AbstractMethodFrame<C> CreateTopElement()
        {
            var result = new AbstractMethodFrame<C>(theStack.Arity, theLocalVariables.Arity);
            result.theStack = 
                result.theStack.CreateTopElement() as AbstractEvalStack<C>;
            result.theLocalVariables
                = result.theLocalVariables.CreateTopElement();
            return result;
        }

        public bool IsTop
        {
            get
            {
                return theStack.IsTop && theLocalVariables.IsTop;
            }
        }

        public bool IsBottom
        {
            get
            {
                return theStack.IsBottom && theLocalVariables.IsBottom;
            }
        }

        #endregion
        #region ICloneable implementation
        public object Clone()
        {
            var result = new AbstractMethodFrame<C>(theStack.Arity, theLocalVariables.Arity);
            result.theStack = (AbstractEvalStack<C>)theStack.Clone();
            result.theLocalVariables = (AbstractTuple<C>)theLocalVariables.Clone();
            return result;
        }
        #endregion

        public override string ToString()
        {
            var result = "FRAME \n(\n";
            result += "Stack: \n";
            result += theStack.ToString();
            result += "Local variables: \n";
            result += theLocalVariables.ToString();
            return result;
        }
    }
}
