using System.Linq;
using System.Collections.Generic;
using Mono.Cecil;
using CODE = Mono.Cecil.Cil.Code;

namespace DotNetAnalyser.Domains
{

    class MethodSummary<T> : IDomainElement<MethodSummary<T>>
        where T : class, IDomainElement<T>, new()
    {

        #region /* Private members */

        Dictionary<MethodDefinition, T> methodEntries;

        #endregion

        #region /* Comparer */

        class MethodComparer : IEqualityComparer<MethodDefinition>
        {

            public bool Equals(MethodDefinition x, MethodDefinition y)
            {
                if (x == null)
                    return y == null;
                else
                    return x.FullName == y.FullName;
            }

            public int GetHashCode(MethodDefinition obj)
            {
                if (obj == null)
                    return 0;
                else
                    return obj.FullName.GetHashCode();
            }
        }

        #endregion

        #region /* Constructors */

        public MethodSummary()
        {
            methodEntries = new Dictionary<MethodDefinition, T>(new MethodComparer());
            isTop = false;
        }

        #endregion

        bool isTop;
        public bool IsTop
        {
            get { return isTop; }
        }

        public bool IsBottom
        {
            get { return !methodEntries.Keys.Any() && !isTop; }
        }

        public T this[MethodDefinition reference]
        {
            set
            {
                methodEntries.Remove(reference);

                if (!value.IsBottom)
                {
                    methodEntries.Add(reference, value);
                }
            }

            get
            {
                return GetOrCreateComponent(reference);
            }
        }

        public IEnumerable<MethodDefinition> Components
        {
            get
            {
                foreach (var element in methodEntries.Keys)
                {
                    yield return element;
                }
            }
        }

        T GetOrCreateComponent(MethodDefinition reference)
        {
            T entry;
            methodEntries.TryGetValue(reference, out entry);
            if (entry == null)
            {
                entry = new T();
                methodEntries.Add(reference, entry);
            }
            return entry;
        }

        T GetComponentOrNull(MethodDefinition reference)
        {
            T entry;
            methodEntries.TryGetValue(reference, out entry);
            return entry;
        }

        public bool Contains(MethodDefinition reference)
        {
            return methodEntries.ContainsKey(reference);
        }

        public void UnionWith(MethodSummary<T> element)
        {
            if (element.isTop || isTop)
                isTop = true;
            else if (element.IsBottom)
                return;
            else if (element == this)
                return;
            else
            {
                foreach (var entry in element.methodEntries.Keys)
                {
                    var otherValue = element.methodEntries[entry];
                    if (!otherValue.IsBottom)
                    {
                        var thisComponent = GetOrCreateComponent(entry);
                        thisComponent.UnionWith(otherValue);
                    }
                }
            }
        }

        public void JoinWith(MethodSummary<T> element)
        {
            if (IsBottom)
                isTop = true;
            else if (element.IsBottom)
            {
                methodEntries.Clear();
            }
            else if (element.IsTop)
                return;
            else if (element == this)
                return;
            else
            {
                foreach (var entry in methodEntries.Keys)
                {
                    var elementValue = element.GetComponentOrNull(entry);
                    if (elementValue == null)
                    {
                        methodEntries.Remove(entry);
                    }
                    else
                    {
                        var thisComponent = methodEntries[entry];
                        thisComponent.JoinWith(elementValue);
                        if (thisComponent.IsBottom)
                        {
                            methodEntries.Remove(entry);
                        }
                    }
                }
            }
        }

        public void Negate()
        {
            isTop = true;
            methodEntries.Clear();
        }

        public void WidenWith(MethodSummary<T> element)
        {
            isTop = true;
            methodEntries.Clear();
        }

        public bool IsSubsetOrEqual(MethodSummary<T> element)
        {
            if (element.IsTop)
                return true;
            if (element.IsBottom)
                return IsBottom;
            if (isTop)
                return element.IsTop;
            if (IsBottom)
                return true;


            foreach (var entry in methodEntries.Keys)
            {
                var elementValue = element.GetComponentOrNull(entry);
                var thisValue = methodEntries[entry];
                if (thisValue == null)
                    System.Console.WriteLine("LOST!");
                if (elementValue == null)
                {
                    if (!thisValue.IsBottom)
                        return false;
                }
                else if (!thisValue.IsSubsetOrEqual(elementValue))
                {
                    return false;
                }
            }

            return true;

        }

        public object Clone()
        {
            var result = new MethodSummary<T>();
            if (IsBottom)
                return result;
            if (IsTop)
            {
                result.isTop = true;
                return result;
            }

            foreach (var entry in methodEntries.Keys)
            {
                var value = methodEntries[entry];
                result.methodEntries.Add(entry, (T)value.Clone());
            }

            return result;
        }

        public override string ToString()
        {
            if (IsBottom)
                return "<BOTTOM>";
            else if (IsTop)
                return "<TOP>";

            var result = "";
            foreach (var entry in methodEntries.Keys)
            {
                result += "\n[" + entry + "] := \n" + methodEntries[entry].ToString() + "\n"; 
            }

            return result;
        }

        public void ToTopElement()
        {
            isTop = true;
            methodEntries.Clear();
        }

        public string Description()
        {
            if (IsTop)
                return "TOP";
            else if (IsBottom)
                return "BOT";
            var result = "{\n";
            foreach (var element in methodEntries.Keys)
            {
                result += element.FullName;
                result += "\n";
            }
            result += "}\n";
            return result;
        }

    }

}
