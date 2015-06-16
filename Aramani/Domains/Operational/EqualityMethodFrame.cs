using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Aramani.Domains
{
    class EqualityMethodFrame :
        IDomainElement<EqualityMethodFrame>,
        IEffectComputer<EqualityMethodFrame>
    {

        Dictionary<object, object> elementToClassRepresentative;
        Dictionary<object, List<object>> classRepresentativeToElements;

        bool isBottom;

        EqualityMethodFrame()
        {
            elementToClassRepresentative = new Dictionary<object, object>();
            classRepresentativeToElements = new Dictionary<object, List<object>>();
            isBottom = true;
        }

        public bool IsTop
        {
            get
            {
                if (isBottom)
                    return false;
                return !(from x in classRepresentativeToElements
                         where x.Value != null && x.Value.Count > 1
                         select x.Value).Any();
            }
        }

        public bool IsBottom
        {
            get { return isBottom; }
        }

        bool InSameClass(object e1, object e2)
        {
            object k1;
            var found1 = elementToClassRepresentative.TryGetValue(e1, out k1);
            object k2;
            var found2 = elementToClassRepresentative.TryGetValue(e2, out k2);
            if (found1 && found2)
                return k1 == k2;
            return false;
        }

        public void UnionWith(EqualityMethodFrame element)
        {
            foreach (var representative in element.classRepresentativeToElements)
            {

            }
        }

        public void JoinWith(EqualityMethodFrame element)
        {
            throw new NotImplementedException();
        }

        public void Negate()
        {
            throw new NotImplementedException();
        }

        public void WidenWith(EqualityMethodFrame element)
        {
            throw new NotImplementedException();
        }

        public bool IsSubsetOrEqual(EqualityMethodFrame element)
        {
            throw new NotImplementedException();
        }

        public void ToTopElement()
        {
            throw new NotImplementedException();
        }

        public void ToBottomElement()
        {
            throw new NotImplementedException();
        }

        public string Description()
        {
            throw new NotImplementedException();
        }

        public object Clone()
        {
            throw new NotImplementedException();
        }

        public void ComputeEffect(Instruction instruction, bool UseElseBranch = false)
        {
            throw new NotImplementedException();
        }
    }
}
