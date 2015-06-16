using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;


namespace Aramani.Domains
{

    /// <summary>
    /// Models an eval stack of a method frame.
    /// The top of the stack is always at index 0; this complicates push and pop,
    /// but facilitates the union and join operations (which are the same as 
    /// for normal elements of the tuple domain).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class VariableCharaterizationDomain : IDomainElement<VariableCharaterizationDomain>
    {

        VariableCharaterization theElement;

        public VariableCharaterizationDomain() :
            this (VariableCharaterization.BOTTOM)
        { }

        public VariableCharaterizationDomain(VariableCharaterization element)
        {
            theElement = element;
        }

        public bool IsTop
        {
            get { return theElement == VariableCharaterization.TOP; }
        }

        public bool IsBottom
        {
            get { return theElement == VariableCharaterization.BOTTOM; }
        }

        public void UnionWith(VariableCharaterizationDomain element)
        {
            if (theElement == VariableCharaterization.BOTTOM)
            {
                theElement = element.theElement;
            }
            else if (element.IsTop)
            {
                theElement = VariableCharaterization.TOP;
            }
        }

        public void JoinWith(VariableCharaterizationDomain element)
        {
            if (theElement == VariableCharaterization.TOP)
            {
                theElement = element.theElement;
            }
            else if (theElement == VariableCharaterization.NULL && element.IsBottom)
            {
                theElement = VariableCharaterization.BOTTOM;
            }
        }

        public void Negate()
        {
            theElement = VariableCharaterization.TOP;
        }

        public void WidenWith(VariableCharaterizationDomain element)
        {
            theElement = VariableCharaterization.TOP;
        }

        public bool IsSubsetOrEqual(VariableCharaterizationDomain element)
        {
            if (IsBottom)
                return true;
            if (IsTop)
                return element.IsTop;
            return !element.IsBottom;

        }

        public void ToTopElement()
        {
            theElement = VariableCharaterization.TOP;
        }

        public object Clone()
        {
            return new VariableCharaterizationDomain(theElement);
        }

        public override string ToString()
        {
            switch (theElement)
            {
                case VariableCharaterization.BOTTOM:
                    return "<BOTTOM>";
                case VariableCharaterization.NULL:
                    return "NULL";
                case VariableCharaterization.TOP:
                    return "<TOP>";
            }
            return "";
        }


        public string Description()
        {
            return ToString();
        }


        public void ToBottomElement()
        {
            theElement = VariableCharaterization.BOTTOM;
        }
    }
}