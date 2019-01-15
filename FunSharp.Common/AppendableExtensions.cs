using System;
using System.Linq;
using JetBrains.Annotations;

namespace FunSharp.Common
{

    //--------------------------------------------------
    /// <summary>
    /// Extensions for <see cref="IAppendable{T}"/> instances.
    /// </summary>
    [PublicAPI]
    public static class AppendableExtensions
    {

        //--------------------------------------------------
        /// <summary>
        /// Repeats <paramref name="item" /> <paramref name="times" />
        /// times, and concatenates the result.
        /// </summary>
        public static T Repeat<T>(this T item, [NotNull] T empty, int times) where T : IAppendable<T>
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            if (empty == null) throw new ArgumentNullException(nameof(empty));
            if (times <= 0) throw new ArgumentOutOfRangeException(nameof(times));

            return Appendable.Concat(empty, Enumerable.Repeat(empty, times));
        }

    }

}