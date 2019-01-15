using System;
using JetBrains.Annotations;

namespace FunSharp.Common
{

    //--------------------------------------------------
    /// <summary>
    /// Extensions for <see cref="List{T}"/> instances.
    /// </summary>
    [PublicAPI]
    public static partial class ListExtensions
    {

        //--------------------------------------------------
        /// <summary>
        /// Foreach value in <paramref name="list" />, obtain
        /// a sub-list and concatenate the results.
        /// </summary>
        [NotNull]
        public static List<TResult> Bind<T, TResult>(
            [NotNull] this List<T> list,
            [NotNull] Func<T, List<TResult>> getSubList)
        {
            if (list is null) throw new ArgumentNullException(nameof(list));
            if (getSubList is null) throw new ArgumentNullException(nameof(getSubList));

            var newList = List<TResult>.Empty;
            while (list is ListCons<T> cons)
            {
                newList = newList + getSubList(cons.Head);
                list = cons.Tail;
            }

            return newList;
        }


        //--------------------------------------------------
        /// <summary>
        /// Produce a <see cref="List{T}"/> with the values
        /// in <paramref name="list" /> which pass
        /// <paramref name="predicate" />.
        /// </summary>
        [NotNull]
        public static List<T> Filter<T>(
            [NotNull] this List<T> list,
            [NotNull] Func<T, bool> predicate)
        {
            if (list is null) throw new ArgumentNullException(nameof(list));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            var newList = List<T>.Empty;
            while (list is ListCons<T> cons)
            {
                if (predicate(cons.Head))
                {
                    newList = newList + cons.Head.ToList();
                }
                list = cons.Tail;
            }

            return newList;
        }


        //--------------------------------------------------
        /// <summary>
        /// Transform the values contained in <paramref name="list" />.
        /// </summary>
        [NotNull]
        public static List<TResult> Map<T, TResult>(
            [NotNull] this List<T> list,
            [NotNull] Func<T, TResult> valueSelector)
        {
            if (list is null) throw new ArgumentNullException(nameof(list));
            if (valueSelector is null) throw new ArgumentNullException(nameof(valueSelector));

            return list.Bind(x => valueSelector(x).ToList());
        }


        //--------------------------------------------------
        /// <summary>
        /// If <paramref name="list" /> has items, run
        /// <paramref name="consSelector" />, or else run
        /// <paramref name="emptySelector" />.
        /// </summary>
        [NotNull]
        public static TResult Match<T, TResult>(
            [NotNull] this List<T> list,
            [NotNull] Func<T, List<T>, TResult> consSelector,
            [NotNull] Func<TResult> emptySelector)
        {
            if (list is null) throw new ArgumentNullException(nameof(list));
            if (consSelector is null) throw new ArgumentNullException(nameof(consSelector));
            if (emptySelector is null) throw new ArgumentNullException(nameof(emptySelector));

            switch (list)
            {
                case ListCons<T> cons: return consSelector(cons.Head, cons.Tail);
                case ListEmpty<T> _: return emptySelector();
                default: throw new ArgumentOutOfRangeException(nameof(list));
            }
        }


        //--------------------------------------------------
        /// <summary>
        /// Return a new <see cref="List{TCast}"/> with the
        /// values in <paramref name="list" /> which are
        /// instances of <typeparamref name="TCast"/>. 
        /// </summary>
        [NotNull]
        public static List<TCast> OfType<T, TCast>(
            [NotNull] this List<T> list,
            TypeHint<TCast> tHint = default) where TCast : T
        {
            if (list is null) throw new ArgumentNullException(nameof(list));

            return list.Bind(x => x is TCast result ? result.ToList() : List<TCast>.Empty);
        }

    }

}