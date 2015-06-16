using System.Linq;
using System.Collections.Generic;
using Mono.Cecil;
using CODE = Mono.Cecil.Cil.Code;

namespace Aramani.Domains
{

    class AbstractTuple<C> : IDomainElement<AbstractTuple<C>>
        where C : class, IDomainElement<C>, new()
    {

        #region /* Private members */

        protected C[] theTuple;

        #endregion

        #region /* Constructors */

        public AbstractTuple()
            : this(0)
        {
        }

        public AbstractTuple(int size)
        {
            theTuple = new C[size];
            for (int i = 0; i < theTuple.Length; i++)
            {
                theTuple[i] = new C();
            }
        }

        #endregion

        public int Arity
        {
            get { return theTuple.Length; }
        }

        public bool IsTop
        {
            get 
            {  
                foreach (var entry in theTuple)
                {
                    if (entry == null || !entry.IsTop)
                        return false;
                }
                return true;
            }
        }

        public bool IsBottom
        {
            get
            {
                foreach (var entry in theTuple)
                {
                    if (entry == null)
                        return false;
                    else if (entry.IsBottom)
                    {
                        return true;
                    }
                }
                return false;
            }

        }

        public C this[int i]
        {
            set
            {
                theTuple[i] = value;
            }

            get
            {
                return theTuple[i];
            }
        }

        public void Normalize()
        {
            if (IsBottom)
            {
                for (int i = 0; i < theTuple.Length; i++)
                {
                    theTuple[i].ToBottomElement();
                }
            }
        }

        public void UnionWith(AbstractTuple<C> element)
        {
            Normalize();
            element.Normalize();
            for (int i = 0; i < theTuple.Length; i++)
            {
                theTuple[i].UnionWith(element.theTuple[i]);
            }
            Normalize();
        }

        public void JoinWith(AbstractTuple<C> element)
        {
            for (int i = 0; i < theTuple.Length; i++)
            {
                theTuple[i].JoinWith(element.theTuple[i]);
            }
            Normalize();
        }

        public void Negate()
        {
            for (int i = 0; i < theTuple.Length; i++)
            {
                theTuple[i].Negate();
            }
            Normalize();
        }

        public void WidenWith(AbstractTuple<C> element)
        {
            for (int i = 0; i < theTuple.Length; i++)
            {
                theTuple[i].WidenWith(element.theTuple[i]);
            }
            Normalize();
        }

        public bool IsSubsetOrEqual(AbstractTuple<C> element)
        {
            for (int i = 0; i < theTuple.Length; i++)
            {
                if (!theTuple[i].IsSubsetOrEqual(element.theTuple[i]))
                    return false;
            }
            return true;
        }

        public void SetAllComponentsToTop()
        {
            for (int i = 0; i < theTuple.Length; i++)
            {
                if (!theTuple[i].IsTop)
                {
                    theTuple[i].ToTopElement();
                }
            }
        }

        public virtual object Clone()
        {
            var result = new AbstractTuple<C>(theTuple.Length);
            for (int i = 0; i < theTuple.Length; i++)
            {
                result.theTuple[i] = theTuple[i].Clone() as C;
            }

            return result;
        }

        public override string ToString()
        {
            /*if (IsBottom)
                return "<BOTTOM>";
            else if (IsTop)
                return "<TOP>";
            */
            var result = "(\n";
            for (int i = 0; i < theTuple.Length; i++)
            {
                result += "[" + i + "] = " + theTuple[i].ToString() + "\n";
            }
            result += ")\n";
            return result;
        }


        public virtual void ToTopElement()
        {
            for (int i = 0; i < theTuple.Length; i++)
            {
                theTuple[i].ToTopElement();
            }
        }

        public virtual void ToBottomElement()
        {
            for (int i = 0; i < theTuple.Length; i++)
            {
                theTuple[i].ToBottomElement();
            }
        }


        public virtual string Description()
        {
            /*if (IsTop)
                return "TOP";
            else if (IsBottom)
                return "BOT";*/
            var result = "((";
            foreach (var element in theTuple)
            {
                result += element.Description() + " ";
            }
            result += "))";
            return result;
        }
    }
}
