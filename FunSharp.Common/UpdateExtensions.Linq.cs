using System;
using JetBrains.Annotations;

namespace FunSharp.Common
{

    public static partial class UpdateExtensions
    {

        //--------------------------------------------------
        /// <inheritdoc cref="Map{T,TResult}"/>
        [NotNull]
        public static Update<T2> Select<T1, T2>(
            [NotNull] this Update<T1> option,
            [NotNull] Func<T1, T2> valueSelector)
        {
            return option.Map(valueSelector);
        }


        //--------------------------------------------------
        /// <inheritdoc cref="Bind{T,TResult}"/>
        [NotNull]
        public static Update<T2> SelectMany<T1, T2>(
            [NotNull] this Update<T1> option,
            [NotNull] Func<T1, Update<T2>> getNextUpdate)
        {
            return option.Bind(getNextUpdate);
        }


        //--------------------------------------------------
        /// <inheritdoc cref="Bind{T,TResult}"/>
        [NotNull]
        public static Update<T2> SelectMany<T1, TIntermediate, T2>(
            [NotNull] this Update<T1> option,
            [NotNull] Func<T1, Update<TIntermediate>> getNextUpdate,
            [NotNull] Func<T1, TIntermediate, T2> resultSelector)
        {
            if (getNextUpdate is null) throw new ArgumentNullException(nameof(getNextUpdate));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return option.Bind(x => getNextUpdate(x).Map(y => resultSelector(x, y)));
        }


        //--------------------------------------------------
        /// <inheritdoc cref="Filter{T}"/>
        [NotNull]
        public static Update<T> Where<T>(
            [NotNull] this Update<T> option,
            [NotNull] Func<T, bool> predicate)
        {
            return option.Filter(predicate);
        }

    }

}