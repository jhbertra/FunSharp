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
            return option.Map(valueSelector);
        }


        //--------------------------------------------------
        /// <inheritdoc cref="Bind{T,TResult}"/>
        [NotNull]
        public static Option<T2> SelectMany<T1, TIntermediate, T2>(
            [NotNull] this Option<T1> option,
            [NotNull] Func<T1, Option<TIntermediate>> getNextOption,
            [NotNull] Func<T1, TIntermediate, T2> resultSelector)
        {
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
            return option.Filter(predicate);
        }

    }

}