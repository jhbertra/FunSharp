using JetBrains.Annotations;

namespace FunSharp.Common
{

    //--------------------------------------------------
    /// <summary>
    /// Interface for a type that has a closed, single-
    /// level set of child types.
    /// </summary> 
    public interface IUnionType
    {

        //--------------------------------------------------
        /// <summary>
        /// Name that uniquely identifies the type of
        /// this instance within the union. 
        /// </summary>
        [NotNull]
        string Tag { get; }

    }

}