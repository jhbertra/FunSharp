using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace FunSharp.Common
{

    //--------------------------------------------------
    /// <summary>
    /// Extensions for <see cref="Either{TLeft,TRight}"/> instances.
    /// </summary>
    [PublicAPI]
    public static partial class EitherExtensions
    {

        //--------------------------------------------------
        /// <summary>
        /// Transform the right value contained in
        /// <paramref name="either" />.
        /// </summary>
        [NotNull]
        public static Either<TLeft2, TRight2> BiMap<TLeft1, TLeft2, TRight1, TRight2>(
            [NotNull] this Either<TLeft1, TRight1> either,
            [NotNull] Func<TLeft1, TLeft2> leftSelector,
            [NotNull] Func<TRight1, TRight2> rightSelector)
        {
            if (either is null) throw new ArgumentNullException(nameof(either));
            if (leftSelector is null) throw new ArgumentNullException(nameof(leftSelector));
            if (rightSelector is null) throw new ArgumentNullException(nameof(rightSelector));

            switch (either)
            {
                case EitherLeft<TLeft1, TRight1> left:
                    return Either.Left(leftSelector(left.Value), new TypeHint<TRight2>());
                case EitherRight<TLeft1, TRight1> right:
                    return Either.Right(rightSelector(right.Value), new TypeHint<TLeft2>());
                default:
                    throw new ArgumentOutOfRangeException(nameof(either));
            }
        }


        //--------------------------------------------------
        /// <summary>
        /// If <paramref name="either" /> is left, use
        /// it to compute another <see cref="Either{TLeft,TRight}"/>.
        /// </summary>
        [NotNull]
        public static Either<TLeft2, TRight> BindLeft<TLeft1, TLeft2, TRight>(
            [NotNull] this Either<TLeft1, TRight> either,
            [NotNull] Func<TLeft1, Either<TLeft2, TRight>> makeNextChoice)
        {
            if (either is null) throw new ArgumentNullException(nameof(either));
            if (makeNextChoice is null) throw new ArgumentNullException(nameof(makeNextChoice));

            switch (either)
            {
                case EitherLeft<TLeft1, TRight> left: return makeNextChoice(left.Value);
                case EitherRight<TLeft1, TRight> right: return Either.Right(right.Value, new TypeHint<TLeft2>());
                default: throw new ArgumentOutOfRangeException(nameof(either));
            }
        }


        //--------------------------------------------------
        /// <summary>
        /// If <paramref name="either" /> is right, use
        /// it to compute another <see cref="Either{TLeft,TRight}"/>.
        /// </summary>
        [NotNull]
        public static Either<TLeft, TRight2> BindRight<TLeft, TRight1, TRight2>(
            [NotNull] this Either<TLeft, TRight1> either,
            [NotNull] Func<TRight1, Either<TLeft, TRight2>> makeNextChoice)
        {
            if (either is null) throw new ArgumentNullException(nameof(either));
            if (makeNextChoice is null) throw new ArgumentNullException(nameof(makeNextChoice));

            switch (either)
            {
                case EitherLeft<TLeft, TRight1> left: return Either.Left(left.Value, new TypeHint<TRight2>());
                case EitherRight<TLeft, TRight1> right: return makeNextChoice(right.Value);
                default: throw new ArgumentOutOfRangeException(nameof(either));
            }
        }


        //--------------------------------------------------
        /// <summary>
        /// If <paramref name="either" /> is left, use
        /// it, or else use <paramref name="defaultValue" />.
        /// </summary>
        [NotNull]
        public static TLeft DefaultLeftWith<TLeft, TRight>(
            [NotNull] this Either<TLeft, TRight> either,
            [NotNull] TLeft defaultValue)
        {
            if (either is null) throw new ArgumentNullException(nameof(either));
            if (defaultValue == null) throw new ArgumentNullException(nameof(defaultValue));

            switch (either)
            {
                case EitherLeft<TLeft, TRight> left: return left.Value;
                case EitherRight<TLeft, TRight> _: return defaultValue;
                default: throw new ArgumentOutOfRangeException(nameof(either));
            }
        }


        //--------------------------------------------------
        /// <summary>
        /// If <paramref name="either" /> is right, use
        /// it, or else use <paramref name="defaultValue" />.
        /// </summary>
        [NotNull]
        public static TRight DefaultRightWith<TLeft, TRight>(
            [NotNull] this Either<TLeft, TRight> either,
            [NotNull] TRight defaultValue)
        {
            if (either is null) throw new ArgumentNullException(nameof(either));
            if (defaultValue == null) throw new ArgumentNullException(nameof(defaultValue));

            switch (either)
            {
                case EitherLeft<TLeft, TRight> _: return defaultValue;
                case EitherRight<TLeft, TRight> right: return right.Value;
                default: throw new ArgumentOutOfRangeException(nameof(either));
            }
        }


        //--------------------------------------------------
        /// <summary>
        /// Flips the type arguments of <paramref name="either" />
        /// </summary>
        public static Either<TRight, TLeft> Flip<TLeft, TRight>([NotNull] this Either<TLeft, TRight> either)
        {
            if (either is null) throw new ArgumentNullException(nameof(either));

            switch (either)
            {
                case EitherLeft<TLeft, TRight> left: return Either.Right<TRight, TLeft>(left.Value);
                case EitherRight<TLeft, TRight> right: return Either.Left<TRight, TLeft>(right.Value);
                default: throw new ArgumentOutOfRangeException(nameof(either));
            }
        }


        //--------------------------------------------------
        /// <summary>
        /// Transform the left value contained in
        /// <paramref name="either" />.
        /// </summary>
        [NotNull]
        public static Either<TLeft2, TRight> MapLeft<TLeft1, TLeft2, TRight>(
            [NotNull] this Either<TLeft1, TRight> either,
            [NotNull] Func<TLeft1, TLeft2> valueSelector)
        {
            if (either is null) throw new ArgumentNullException(nameof(either));
            if (valueSelector is null) throw new ArgumentNullException(nameof(valueSelector));

            switch (either)
            {
                case EitherLeft<TLeft1, TRight> left:
                    return Either.Left(valueSelector(left.Value), new TypeHint<TRight>());
                case EitherRight<TLeft1, TRight> right:
                    return Either.Right(right.Value, new TypeHint<TLeft2>());
                default:
                    throw new ArgumentOutOfRangeException(nameof(either));
            }
        }


        //--------------------------------------------------
        /// <summary>
        /// Transform the right value contained in
        /// <paramref name="either" />.
        /// </summary>
        [NotNull]
        public static Either<TLeft, TRight2> MapRight<TLeft, TRight1, TRight2>(
            [NotNull] this Either<TLeft, TRight1> either,
            [NotNull] Func<TRight1, TRight2> valueSelector)
        {
            if (either is null) throw new ArgumentNullException(nameof(either));
            if (valueSelector is null) throw new ArgumentNullException(nameof(valueSelector));

            switch (either)
            {
                case EitherLeft<TLeft, TRight1> left:
                    return Either.Left(left.Value, new TypeHint<TRight2>());
                case EitherRight<TLeft, TRight1> right:
                    return Either.Right(valueSelector(right.Value), new TypeHint<TLeft>());
                default:
                    throw new ArgumentOutOfRangeException(nameof(either));
            }
        }


        //--------------------------------------------------
        /// <summary>
        /// If <paramref name="either" /> is a left, run
        /// <paramref name="leftSelector" />, or else run
        /// <paramref name="rightSelector" />.
        /// </summary>
        [NotNull]
        public static TResult Match<TLeft, TRight, TResult>(
            [NotNull] this Either<TLeft, TRight> either,
            [NotNull] Func<TLeft, TResult> leftSelector,
            [NotNull] Func<TRight, TResult> rightSelector)
        {
            if (either is null) throw new ArgumentNullException(nameof(either));
            if (leftSelector is null) throw new ArgumentNullException(nameof(leftSelector));
            if (rightSelector is null) throw new ArgumentNullException(nameof(rightSelector));

            switch (either)
            {
                case EitherLeft<TLeft, TRight> left: return leftSelector(left.Value);
                case EitherRight<TLeft, TRight> right: return rightSelector(right.Value);
                default: throw new ArgumentOutOfRangeException(nameof(either));
            }
        }


        //--------------------------------------------------
        /// <summary>
        /// Discards the right value and converts <paramref name="either" />
        /// to an <see cref="Option{T}"/>.
        /// </summary>
        [NotNull]
        public static Option<TLeft> ToLeftOption<TLeft, TRight>([NotNull] this Either<TLeft, TRight> either)
        {
            if (either is null) throw new ArgumentNullException(nameof(either));

            return either.Match(Option.Some, _ => Option<TLeft>.None);
        }


        //--------------------------------------------------
        /// <summary>
        /// Discards the left value and converts <paramref name="either" />
        /// to an <see cref="Option{T}"/>.
        /// </summary>
        [NotNull]
        public static Option<TRight> ToRightOption<TLeft, TRight>([NotNull] this Either<TLeft, TRight> either)
        {
            if (either is null) throw new ArgumentNullException(nameof(either));

            return either.Match(_ => Option<TRight>.None, Option.Some);
        }


        //--------------------------------------------------
        /// <summary>
        /// Discards the right value and converts <paramref name="either" />
        /// to an <see cref="IEnumerable{T}"/>.
        /// </summary>
        [NotNull]
        public static IEnumerable<TLeft> ToLeftEnumerable<TLeft, TRight>([NotNull] this Either<TLeft, TRight> either)
        {
            if (either is null) throw new ArgumentNullException(nameof(either));

            switch (either)
            {
                case EitherLeft<TLeft, TRight> left:
                    yield return left.Value;
                    break;
                case EitherRight<TLeft, TRight> _:
                    yield break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(either));
            }
        }


        //--------------------------------------------------
        /// <summary>
        /// Discards the left value and converts <paramref name="either" />
        /// to an <see cref="IEnumerable{T}"/>.
        /// </summary>
        [NotNull]
        public static IEnumerable<TRight> ToRightEnumerable<TLeft, TRight>([NotNull] this Either<TLeft, TRight> either)
        {
            if (either is null) throw new ArgumentNullException(nameof(either));

            switch (either)
            {
                case EitherRight<TLeft, TRight> right:
                    yield return right.Value;
                    break;
                case EitherLeft<TLeft, TRight> _:
                    yield break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(either));
            }
        }

    }

}