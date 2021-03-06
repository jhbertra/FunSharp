using System;
using JetBrains.Annotations;

namespace FunSharp.Common
{

    public static partial class EitherExtensions
    {

        //--------------------------------------------------
        /// <inheritdoc cref="MapRight{TLeft,TRight1,TRight2}"/>
        [NotNull]
        public static Either<TLeft, TRight2> Select<TLeft, TRight1, TRight2>(
            [NotNull] this Either<TLeft, TRight1> either,
            [NotNull] Func<TRight1, TRight2> valueSelector)
        {
            return either.MapRight(valueSelector);
        }


        //--------------------------------------------------
        /// <inheritdoc cref="BindRight{TLeft,TRight1,TRight2}"/>
        [NotNull]
        public static Either<TLeft, TRight2> SelectMany<TLeft, TRight1, TRightIntermediate, TRight2>(
            [NotNull] this Either<TLeft, TRight1> either,
            [NotNull] Func<TRight1, Either<TLeft, TRightIntermediate>> getNextOption,
            [NotNull] Func<TRight1, TRightIntermediate, TRight2> resultSelector)
        {
            if (getNextOption is null) throw new ArgumentNullException(nameof(getNextOption));
            if (resultSelector is null) throw new ArgumentNullException(nameof(resultSelector));

            return either.BindRight(x => getNextOption(x).MapRight(y => resultSelector(x, y)));
        }

    }

}