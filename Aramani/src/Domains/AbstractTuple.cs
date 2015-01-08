using System.Linq;
using System.Collections.Generic;
using Mono.Cecil;
using CODE = Mono.Cecil.Cil.Code;

namespace DotNetAnalyser.Domains
{

    class AbstractTuple<C> : IDomainElement<AbstractTuple<C>>
        where C : class, IDomainElement<C>, new()
    {

        #region /* Private members */

        C[] theTuple;

        #endregion

        #region /* Constructors */

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
                    if (entry != null && !entry.IsBottom)
                        return false;
                }
                return true;
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

        public void UnionWith(AbstractTuple<C> element)
        {
            for (int i = 0; i < theTuple.Length; i++)
            {
                theTuple[i].UnionWith(element.theTuple[i]);
            }
        }

        public void JoinWith(AbstractTuple<C> element)
        {
            for (int i = 0; i < theTuple.Length; i++)
            {
                theTuple[i].JoinWith(element.theTuple[i]);
            }
        }

        public void Negate()
        {
            for (int i = 0; i < theTuple.Length; i++)
            {
                theTuple[i].Negate();
            }
        }

        public void WidenWith(AbstractTuple<C> element)
        {
            for (int i = 0; i < theTuple.Length; i++)
            {
                theTuple[i].WidenWith(element.theTuple[i]);
            }
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
                    theTuple[i] = theTuple[i].CreateTopElement();
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


        public virtual AbstractTuple<C> CreateTopElement()
        {
            var result = new AbstractTuple<C>(theTuple.Length);
            for (int i = 0; i < theTuple.Length; i++)
            {
                result.theTuple[i] = theTuple[0].CreateTopElement();
            }
            return result;
        }
    }

}
