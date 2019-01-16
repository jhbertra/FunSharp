using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace FunSharp.Common
{

    //**************************************************
    //* Module
    //**************************************************

    //--------------------------------------------------
    /// <inheritdoc cref="IAppendable{T}"/>
    [PublicAPI]
    public static class Appendable
    {

        //--------------------------------------------------
        /// <summary>
        /// Concatenate a series of appendable items together
        /// to create a new one.
        /// </summary>
        public static T Concat<T>([NotNull] T empty, [NotNull] IEnumerable<T> items) where T : IAppendable<T>
        {
            if (empty == null) throw new ArgumentNullException(nameof(empty));
            if (items is null) throw new ArgumentNullException(nameof(items));

            return items.Aggregate(empty, (a, b) => a.Append(b));
        }


        //--------------------------------------------------
        /// <inheritdoc cref="Concat{T}(T,System.Collections.Generic.IEnumerable{T})" />
        public static T Concat<T>([NotNull] T empty, [NotNull] params T[] items) where T : IAppendable<T>
        {
            if (items is null) throw new ArgumentNullException(nameof(items));

            return Appendable.Concat(empty, items.AsEnumerable());
        }


        //--------------------------------------------------
        /// <inheritdoc cref="AppendableExtensions.Repeat{T}"/>
        public static Func<T, T> Repeat<T>([NotNull] T empty, int times) where T : IAppendable<T>
        {
            return t => t.Repeat(empty, times);
        }

    }


    //**************************************************
    //* Types
    //**************************************************

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