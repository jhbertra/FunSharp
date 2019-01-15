using JetBrains.Annotations;

namespace FunSharp.Common
{

    //--------------------------------------------------
    /// <summary>
    /// Interface for a type that has a defined empty
    /// value.
    /// </summary>
    [PublicAPI]
    public interface IEmpty
    {

        //--------------------------------------------------
        /// <summary>
        /// Whether this instance is empty.
        /// </summary>
        bool IsEmpty { get; }

    }

}