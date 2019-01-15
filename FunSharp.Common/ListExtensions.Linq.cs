using System;
using JetBrains.Annotations;

namespace FunSharp.Common
{

    public static partial class OptionExtensions
    {

        //--------------------------------------------------
        /// <inheritdoc cref="Map{T,TResult}"/>
        [NotNull]
        public static List<T2> Select<T1, T2>(
            [NotNull] this List<T1> list,
            [NotNull] Func<T1, T2> valueSelector)
        {
            if (list is null) throw new ArgumentNullException(nameof(list));
            if (valueSelector is null) throw new ArgumentNullException(nameof(valueSelector));

            return list.Map(valueSelector);
        }


        //--------------------------------------------------
        /// <inheritdoc cref="Bind{T,TResult}"/>
        [NotNull]
        public static List<T2> SelectMany<T1, T2>(
            [NotNull] this List<T1> list,
            [NotNull] Func<T1, List<T2>> getNextOption)
        {
            if (list is null) throw new ArgumentNullException(nameof(list));
            if (getNextOption is null) throw new ArgumentNullException(nameof(getNextOption));

            return list.Bind(getNextOption);
        }


        //--------------------------------------------------
        /// <inheritdoc cref="Bind{T,TResult}"/>
        [NotNull]
        public static List<T2> SelectMany<T1, TIntermediate, T2>(
            [NotNull] this List<T1> list,
            [NotNull] Func<T1, List<TIntermediate>> getNextOption,
            [NotNull] Func<T1, TIntermediate, T2> resultSelector)
        {
            if (list is null) throw new ArgumentNullException(nameof(list));
            if (getNextOption is null) throw new ArgumentNullException(nameof(getNextOption));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return list.Bind(x => getNextOption(x).Map(y => resultSelector(x, y)));
        }


        //--------------------------------------------------
        /// <inheritdoc cref="Filter{T}"/>
        [NotNull]
        public static List<T> Where<T>(
            [NotNull] this List<T> list,
            [NotNull] Func<T, bool> predicate)
        {
            if (list is null) throw new ArgumentNullException(nameof(list));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            return list.Filter(predicate);
        }

    }

}