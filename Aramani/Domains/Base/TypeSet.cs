using System.Linq;
using System.Collections.Generic;
using Mono.Cecil;
using CODE = Mono.Cecil.Cil.Code;

namespace Aramani.Domains
{

    class TypeSet : Dictionary<Mono.Cecil.TypeReference, bool>, IDomainElement<TypeSet>
    {

        internal class MonoTypeComparer : IEqualityComparer<TypeReference>
        {

            public bool Equals(TypeReference x, TypeReference y)
            {
                var isXNull = x == null;
                var isYNull = y == null;
                if (isXNull != isYNull)
                    return false;
                if (isXNull)
                    return true;
                return x.FullName == y.FullName;
            }

            public int GetHashCode(TypeReference obj)
            {
                return (obj == null) ? 0 : obj.FullName.GetHashCode();
            }

        }

        public TypeSet()
            : base(new MonoTypeComparer())
        { }

        public void Add(Mono.Cecil.TypeReference typeRef)
        {
            if (!this.ContainsKey(typeRef))
            {
                this.Add(typeRef, true);
            }
        }

        public void UnionWith(TypeSet element)
        {
            if (element.IsBottom)
                return;
            else if (element.IsTop)
                IsTop = true;
            else if (IsTop)
                return;
            else
            {
                foreach (var key in element.Keys)
                {
                    bool found;
                    this.TryGetValue(key, out found);
                    if (!found)
                        this.Add(key, true);
                }
            }
        }

        public void JoinWith(TypeSet element)
        {
            if (element.IsBottom)
                IsBottom = true;
            else if (element.IsTop)
                return;
            else 
            {
                if (IsTop)
                {
                    // element is not top !
                    this.Clear();
                    foreach (var key in element.Keys)
                    {
                        this.Add(key, true);
                    }
                    this.IsTop = false;
                }
                else
                {
                    foreach (var key in Keys)
                    {
                        bool found;
                        element.TryGetValue(key, out found);
                        if (!found)
                            this.Remove(key);
                    }
                }
            }
        }

        public void Negate()
        {
            IsTop = true;
        }

        public void WidenWith(TypeSet element)
        {
            
        }

        bool isTop = false;
        public bool IsTop
        {
            get { return isTop; }
            set { this.Clear(); isTop = true; }
        }

        /// <summary>
        /// top != bottom
        /// </summary>
        public bool IsBottom
        {
            get { return !this.Any() && !IsTop; }
            set { this.Clear(); isTop = false; }
        }

        public override bool Equals(object obj)
        {
            var objAsTypeSet = obj as TypeSet;
            if (objAsTypeSet == null)
                return false;
            return base.Equals(objAsTypeSet);
        }

        public override int GetHashCode()
        {
            if (IsTop)
                return -1;
            if (IsBottom)
                return 0;
            return base.GetHashCode();
        }

        public object Clone()
        {
            throw new System.NotImplementedException();
        }

        public bool IsSubsetOrEqual(TypeSet element)
        {
            return false;
        }

        public void ToTopElement()
        {
            isTop = true;
            Clear();
        }


        public string Description()
        {
            if (isTop)
                return "_|_";
            else if (IsBottom)
                return "{}";
            var result = "{\n";
            foreach (var element in this.Keys)
            {
                result += element.FullName;
                result += "\n";
            }
            result += "}\n";
            return result;
        }


        public void ToBottomElement()
        {
            IsBottom = true;
        }
    }
}