
namespace Aramani.Base
{
    /// <summary>
    /// Implemented by entities containing to a graph structure, 
    /// for displaying them as simple text or in dot format.
    /// </summary>
    public interface IGraphDisplayable : IDescription
    {
       
        /// <summary>
        /// Return code fragment in dot format
        /// describing the object.
        /// </summary>
        string AsDot();

    }
}
