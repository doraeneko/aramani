using System.Linq;
using System.Collections.Generic;
using Mono.Cecil;
using CODE = Mono.Cecil.Cil.Code;

namespace DotNetAnalyser.Domains
{

    class ReducedProduct<T1, T2> : IDomainElement<ReducedProduct<T1,T2>>
        where T1 : class, IDomainElement<T1>, new()
        where T2 : class, IDomainElement<T2>, new()
    {

        T1 component1;
        T2 component2;

        public T1 Component1 { get { return component1; } }
        public T2 Component2 { get { return component2; } }

        public ReducedProduct()
        {
            component1 = new T1();
            component2 = new T2();
        }

        public ReducedProduct(T1 c1, T2 c2)
        {
            component1 = c1;
            component2 = c2;
        }

        public bool IsTop
        {
            get { return component1.IsTop && component2.IsTop; }
        }

        public bool IsBottom
        {
            get { return component1.IsBottom || component2.IsBottom; }
        }

        public void UnionWith(ReducedProduct<T1, T2> element)
        {
            component1.UnionWith(element.component1);
            component2.UnionWith(element.component2);
        }

        public void JoinWith(ReducedProduct<T1, T2> element)
        {
            component1.JoinWith(element.component1);
            component2.JoinWith(element.component2);
        }

        public void Negate()
        {
            component1.Negate();
            component2.Negate();
        }

        public void WidenWith(ReducedProduct<T1, T2> element)
        {
            component1.WidenWith(element.component1);
            component2.WidenWith(element.component2);
        }

        public bool IsSubsetOrEqual(ReducedProduct<T1, T2> element)
        {
            return component1.IsSubsetOrEqual(element.component1) 
                && component2.IsSubsetOrEqual(element.component2);
        }

        public void ToTopElement()
        {
            component1.ToTopElement();
            component2.ToTopElement();
        }

        public object Clone()
        {
            return new ReducedProduct<T1, T2>
                (component1.Clone() as T1,
                 component2.Clone() as T2);
        }


        public string Description()
        {
            if (IsBottom)
                return "BOT";
            if (IsTop)
                return "TOP";
            var result = "(\n";
            result += component1.Description();
            result += ",\n";
            result += component2.Description();
            result += ")\n";
            return result;
        }
    }


    class ReducedProduct<T1, T2, T3> : IDomainElement<ReducedProduct<T1, T2, T3>>
        where T1 : class, IDomainElement<T1>, new()
        where T2 : class, IDomainElement<T2>, new()
        where T3 : class, IDomainElement<T3>, new()

    {

        T1 component1;
        T2 component2;
        T3 component3;

        public T1 Component1 { get { return component1; } }
        public T2 Component2 { get { return component2; } }
        public T3 Component3 { get { return component3; } }

        public ReducedProduct()
        {
            component1 = new T1();
            component2 = new T2();
            component3 = new T3();
        
        }

        public ReducedProduct(T1 c1, T2 c2, T3 c3)
        {
            component1 = c1;
            component2 = c2;
            component3 = c3;
        }

        public bool IsTop
        {
            get
            {
                return component1.IsTop
                       && component2.IsTop
                       && component3.IsTop;
            }
        }

        public bool IsBottom
        {
            get
            {
                return component1.IsBottom
                       || component2.IsBottom
                       || component3.IsBottom;
            }
        }

        public void UnionWith(ReducedProduct<T1, T2, T3> element)
        {
            component1.UnionWith(element.component1);
            component2.UnionWith(element.component2);
            component3.UnionWith(element.component3);
        }

        public void JoinWith(ReducedProduct<T1, T2, T3> element)
        {
            component1.JoinWith(element.component1);
            component2.JoinWith(element.component2);
            component3.JoinWith(element.component3);
        }

        public void Negate()
        {
            component1.Negate();
            component2.Negate();
            component3.Negate();
        }

        public void WidenWith(ReducedProduct<T1, T2, T3> element)
        {
            component1.WidenWith(element.component1);
            component2.WidenWith(element.component2);
            component3.WidenWith(element.component3);
        }

        public bool IsSubsetOrEqual(ReducedProduct<T1, T2, T3> element)
        {
            return component1.IsSubsetOrEqual(element.component1)
                   && component2.IsSubsetOrEqual(element.component2)
                   && component3.IsSubsetOrEqual(element.component3);
        }

        public void ToTopElement()
        {
            component1.ToTopElement();
            component2.ToTopElement();
            component3.ToTopElement();
        }

        public object Clone()
        {
            return new ReducedProduct<T1, T2, T3>
                (component1.Clone() as T1,
                 component2.Clone() as T2,
                 component3.Clone() as T3);
        }

        public string Description()
        {
            if (IsTop)
                return "TOP";
            if (IsBottom)
                return "BOT";

            var result = "(\n";
            result += component1.Description();
            result += ",\n";
            result += component2.Description();
            result += ",\n";
            result += component3.Description();
            result += "\n)\n";
            return result;
        }
    }

}