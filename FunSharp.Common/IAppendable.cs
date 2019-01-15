using JetBrains.Annotations;

namespace FunSharp.Common
{

    //--------------------------------------------------
    /// <summary>
    /// Interface for a type that has an associative
    /// append operation.
    /// </summary>
    [PublicAPI]
    public interface IAppendable<T>
    {

        //--------------------------------------------------
        /// <summary>
        /// Produce a new <typeparamref name="T"/> instance
        /// by combining this instance with <paramref name="t" />.
        /// </summary>
        [NotNull]
        T Append([NotNull] T t);

    }

}