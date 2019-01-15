using System;
using JetBrains.Annotations;

namespace FunSharp.Common
{

    public static partial class OptionExtensions
    {

        //--------------------------------------------------
        /// <inheritdoc cref="Map{T,TResult}"/>
        [NotNull]
        public static Option<T2> Select<T1, T2>(
            [NotNull] this Option<T1> option,
            [NotNull] Func<T1, T2> valueSelector)
        {
            if (option is null) throw new ArgumentNullException(nameof(option));
            if (valueSelector is null) throw new ArgumentNullException(nameof(valueSelector));

            return option.Map(valueSelector);
        }


        //--------------------------------------------------
        /// <inheritdoc cref="Bind{T,TResult}"/>
        [NotNull]
        public static Option<T2> SelectMany<T1, T2>(
            [NotNull] this Option<T1> option,
            [NotNull] Func<T1, Option<T2>> getNextOption)
        {
            if (option is null) throw new ArgumentNullException(nameof(option));
            if (getNextOption is null) throw new ArgumentNullException(nameof(getNextOption));

            return option.Bind(getNextOption);
        }


        //--------------------------------------------------
        /// <inheritdoc cref="Bind{T,TResult}"/>
        [NotNull]
        public static Option<T2> SelectMany<T1, TIntermediate, T2>(
            [NotNull] this Option<T1> option,
            [NotNull] Func<T1, Option<TIntermediate>> getNextOption,
            [NotNull] Func<T1, TIntermediate, T2> resultSelector)
        {
            if (option is null) throw new ArgumentNullException(nameof(option));
            if (getNextOption is null) throw new ArgumentNullException(nameof(getNextOption));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return option.Bind(x => getNextOption(x).Map(y => resultSelector(x, y)));
        }


        //--------------------------------------------------
        /// <inheritdoc cref="Filter{T}"/>
        [NotNull]
        public static Option<T> Where<T>(
            [NotNull] this Option<T> option,
            [NotNull] Func<T, bool> predicate)
        {
            if (option is null) throw new ArgumentNullException(nameof(option));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            return option.Filter(predicate);
        }

    }

}