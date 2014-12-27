/*
 * 
 * 
 */
using System.Linq;
using System.Collections.Generic;
using Mono.Cecil;
using CODE = Mono.Cecil.Cil.Code;

namespace DotNetAnalyser.Domains
{

#if DONE

    abstract class ReferenceSet<T> : IDomainElement<ReferenceSet<T>>
        where T : MemberReference
    {

        #region /* Properties */

        bool isTop = false;
        public bool IsTop
        {
            get { return isTop; }
        }

        /// <summary>
        /// top != bottom
        /// </summary>
        public bool IsBottom
        {
            get { return !IsTop; }
        }

        #endregion

        #region /* Constructors */

        public ReferenceSet()
        { }

        #endregion

        #region /* Methods */

        public abstract void Add(T reference);

        public abstract void Remove(T reference);

        #endregion
    }
#endif
}