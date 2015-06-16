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
    /// Domain that distinguishes between:
    /// - value 1 (int), 
    /// - all other ints,
    /// - null, 
    /// - all other values.
    /// </summary>
    public class OneNull : IDomainElement<OneNull>
    {

        public enum OneNullEnum
        {
            BOTTOM,
            TOP,
            NULL,
            INT_ONE,
            INT_NOTONE,
            NOT_NULL,
        }

        public OneNullEnum TheElement;

        public OneNull() :
            this(OneNullEnum.BOTTOM)
        {
 
        }

        public OneNull(OneNullEnum element)
        {
            TheElement = element;
        }

        public bool IsTop
        {
            get { return TheElement == OneNullEnum.TOP; }
        }

        public bool IsBottom
        {
            get { return TheElement == OneNullEnum.BOTTOM; }
        }

        public void UnionWith(OneNull element)
        {
            if (IsSubsetOrEqual(element))
            {
                TheElement = element.TheElement;
            }
            else if (element.IsSubsetOrEqual(this))
            {
                return;
            }
            else
            {
                TheElement = OneNullEnum.TOP;
            }
        }

        public void JoinWith(OneNull element)
        {
            if (TheElement == OneNullEnum.TOP)
            {
                TheElement = element.TheElement;
            }
            else if (TheElement == OneNullEnum.NULL && element.IsBottom)
            {
                TheElement = OneNullEnum.BOTTOM;
            }
        }

        public void Negate()
        {
            TheElement = OneNullEnum.TOP;
        }

        public void WidenWith(OneNull element)
        {
            // no widen necessary
        }

        public bool IsSubsetOrEqual(OneNull element)
        {
            if (element.IsTop)
                return true;
            if (element.TheElement == this.TheElement)
                return true;
            switch (TheElement)
            {
                case OneNullEnum.BOTTOM:
                    return true;
                case OneNullEnum.TOP:
                    return false;
                case OneNullEnum.NULL:
                    return element.TheElement == OneNullEnum.NULL;
                case OneNullEnum.INT_ONE:
                case OneNullEnum.INT_NOTONE:
                case OneNullEnum.NOT_NULL:
                    return element.TheElement == OneNullEnum.NOT_NULL;
                default:
                    return false;
            }
        }

        public void ToTopElement()
        {
            TheElement = OneNullEnum.TOP;
        }

        public object Clone()
        {
            Console.WriteLine("CLONE: " + TheElement);
            var result = new OneNull(TheElement);
            Console.WriteLine("CLONELI: " + result.Description());
            return result;
            
        }

        public override string ToString()
        {
            switch (TheElement)
            {
                case OneNullEnum.BOTTOM:
                    return "<BOTTOM>";
                case OneNullEnum.NULL:
                    return "NULL";
                case OneNullEnum.INT_NOTONE:
                    return "INT/{1}";
                case OneNullEnum.INT_ONE:
                    return "{1}";
                case OneNullEnum.TOP:
                    return "<TOP>";
                case OneNullEnum.NOT_NULL:
                    return "<NOTNULL>";
                default:
                    return "";
            }

        }


        public string Description()
        {
            return ToString();
        }


        public void ToBottomElement()
        {
            TheElement = OneNullEnum.BOTTOM;
        }
    }
}