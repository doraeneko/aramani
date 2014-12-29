using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetAnalyser.Domains
{
    interface IDomainElement<T> : ICloneable
    {

        #region /* Properties */

        bool IsTop { get; }

        bool IsBottom { get; }

        #endregion

        #region /* Methods */

        void UnionWith(T element);
        
        void JoinWith(T element);

        void Negate();

        void WidenWith(T element);

        bool IsSubsetOrEqual(T element);

        T CreateTopElement();
		
        #endregion
    }
}
