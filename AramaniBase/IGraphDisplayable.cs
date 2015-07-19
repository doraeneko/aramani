using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Aramani
{
    /// <summary>
    /// Implemented by entities containing to a graph structure, 
    /// for displaying them as simple text or in dot format.
    /// </summary>
    public interface IGraphDisplayable
    {
       
        /// <summary>
        /// Return code fragment in dot format
        /// describing the object.
        /// </summary>
        /// <returns></returns>
        string AsDot();

        /// <summary>
        /// Return a simple textual description of
        /// the object.
        /// </summary>
        /// <returns></returns>
        string Description { get; }
    }
}
