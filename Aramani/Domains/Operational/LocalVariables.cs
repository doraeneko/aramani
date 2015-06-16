using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aramani.Domains
{
    class LocalVariables<T> : IDomainElement<LocalVariables<T>>
        where T : class, IDomainElement<T>, new()
    {

        #region /* Components */

        T[] store;

        Mono.Cecil.MethodDefinition theMethod;

        #endregion

        public LocalVariables(Mono.Cecil.MethodDefinition method)
        {
            theMethod = method;
            store = new T[method.Body.Variables.Count];
            for (int i = 0; i < store.Length; i++)
            {
                store[i] = new T();
            }
        }

        public LocalVariables
            (Mono.Cecil.MethodDefinition method, T bottomValue)
        {
            theMethod = method;
            store = new T[method.Body.Variables.Count];
            for (int i = 0; i < store.Length; i++)
            {
                store[i] = bottomValue.Clone() as T;
            }
        }

        public bool IsTop
        {
            get { return store.Any(x => !x.IsTop); }
        }

        public bool IsBottom
        {
            get { return store.Any(x => x.IsBottom); }
        }

        public void UnionWith(LocalVariables<T> element)
        {
            for (int i = 0; i < element.store.Length; i++)
            {
                store[i].UnionWith(element.store[i]);
            }
        }

        public void JoinWith(LocalVariables<T> element)
        {
            for (int i = 0; i < element.store.Length; i++)
            {
                store[i].JoinWith(element.store[i]);
            }
        }

        public void Negate()
        {
            for (int i = 0; i < store.Length; i++)
            {
                this.store[i].Negate();
            }
        }

        public void WidenWith(LocalVariables<T> element)
        {
            for (int i = 0; i < element.store.Length; i++)
            {
                store[i].WidenWith(element.store[i]);
            }
        }

        public bool IsSubsetOrEqual(LocalVariables<T> element)
        {
            for (int i = 0; i < element.store.Length; i++)
            {
                if (!store[i].IsSubsetOrEqual(element.store[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public void ToTopElement()
        {
            for (int i = 0; i < store.Length; i++)
            {
                store[i].ToTopElement();
            }
        }

        public void ToBottomElement()
        {
            for (int i = 0; i < store.Length; i++)
            {
                store[i].ToBottomElement();
            }
        }

        public string Description()
        {
            var result = "";
            var counter = 0;
            foreach (var component in store)
            {
                result += "" + counter + ":" + component.Description() + "\n";
                counter++;
            }
            return result;
        }

        public object Clone()
        {
            var result = new LocalVariables<T>(theMethod);
            for (int i = 0; i < store.Length; i++)
            {
                result.store[i] = store[i].Clone() as T;
            }
            return result;
        }



    }
}
