/*
 * 
 * 
 */
using System.Linq;
using System.Collections.Generic;
using Mono.Cecil;
using CODE = Mono.Cecil.Cil.Code;

namespace Aramani.Domains
{


    class ReferenceSet<T> : IDomainElement<ReferenceSet<T>>
        where T : MemberReference
    {

        #region /* Internal data structures */

        HashSet<T> theSet;

        #endregion

        #region /* Comparer */

        class Comparer : IEqualityComparer<T>
        {

            public bool Equals(T x, T y)
            {
                return x.FullName == y.FullName;
            }

            public int GetHashCode(T obj)
            {
                if (obj == null)
                    return 0;
                return obj.FullName.GetHashCode();
            }

        }

        #endregion

        #region /* Constructors */

        public ReferenceSet()
        {
            theSet = new HashSet<T>(new Comparer());
        }

        #endregion

        #region /* Properties */

        bool isTop = false;
        public bool IsTop
        {
            get { return isTop; }
        }

        public bool IsBottom
        {
            get { return !theSet.Any() && !IsTop; }
        }

        #endregion
        
        #region /* Methods */

        public IEnumerable<T> GetEntries()
        {
            foreach (var entry in theSet)
            {
                yield return entry;
            }
        }

        public void Add(T reference)
        {
            theSet.Add(reference);
        }

        public void Remove(T reference)
        {
            theSet.Remove(reference);
        }

        public void UnionWith(ReferenceSet<T> element)
        {
            if (element.IsTop || isTop)
                isTop = true;
            else
                theSet.UnionWith(element.theSet);
        }

        public void JoinWith(ReferenceSet<T> element)
        {
            if (element.IsTop)
                return;
            else if (isTop)
            {
                isTop = false;
                theSet = new HashSet<T>();
                theSet.UnionWith(element.theSet);
            }
            else
                theSet.IntersectWith(element.theSet);
        }

        public void Negate()
        {
            if (isTop)
            {
                isTop = false;
                theSet.Clear();
            }
            else if (IsBottom) // bottom
            {
                isTop = true;
            }
            else
            {
                isTop = true;
            }
        }

        public void WidenWith(ReferenceSet<T> element)
        {
            isTop = true;
            theSet.Clear();
        }

        public bool IsSubsetOrEqual(ReferenceSet<T> element)
        {
            if (element.IsTop)
                return true;
            if (element.IsBottom)
                return this.IsBottom;
            return theSet.IsSubsetOf(element.theSet);
        }

        public object Clone()
        {
            var result = new ReferenceSet<T>();
            result.isTop = isTop;
            result.theSet.UnionWith(theSet);
            return result;
        }

        public int Cardinality
        {
            get 
            { 
                if (isTop) return -1;
                if (IsBottom) return 0;
                return theSet.Count();
            }

        }
        public override string ToString()
        {
            var result = "";
            if (isTop)
                result += "<TOP>\n";
            else if (IsBottom)
                result += "<BOTTOM>\n";
            else
            {
                result += "{\n";
                foreach (var el in theSet)
                    result += el.FullName + "\n";
                result += "}\n";
            }
            return result;
        }

        public void ToTopElement()
        {
            isTop = true;
            theSet.Clear();
        }


        public string Description()
        {
            if (isTop)
                return "TOP";
            else if (IsBottom)
                return "BOT";
            var result = "{\n";
            foreach (var element in theSet)
            {
                result += element.ToString();
                result += "\n";
            }
            result += "}\n";
            return result;
        }

        #endregion



        public void ToBottomElement()
        {
            isTop = false;
            theSet.Clear();
        }
    }

}