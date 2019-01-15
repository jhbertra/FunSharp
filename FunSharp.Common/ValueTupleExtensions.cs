using System;
using JetBrains.Annotations;

namespace FunSharp.Common
{

    //--------------------------------------------------
    /// <summary>
    /// Extensions for <see cref="ValueTuple{T1,T2}"/> instances.
    /// </summary>
    [PublicAPI]
    public static class ValueTupleExtensions
    {

        //--------------------------------------------------
        /// <summary>
        /// Use the left item to compute another
        /// <see cref="ValueTuple{T1,T2}"/>.
        /// </summary>
        public static (TLeft2, TRight) BindLeft<TLeft1, TLeft2, TRight>(
            this (TLeft1, TRight) tuple,
            [NotNull] Func<TLeft1, (TLeft2, TRight)> getNextPair)
        {
            if (getNextPair is null) throw new ArgumentNullException(nameof(getNextPair));

            return getNextPair(tuple.Item1);
        }


        //--------------------------------------------------
        /// <summary>
        /// Use the right item to compute another
        /// <see cref="ValueTuple{T1,T2}"/>.
        /// </summary>
        public static (TLeft, TRight2) BindRight<TLeft, TRight1, TRight2>(
            this (TLeft, TRight1) tuple,
            [NotNull] Func<TRight1, (TLeft, TRight2)> getNextPair)
        {
            if (getNextPair is null) throw new ArgumentNullException(nameof(getNextPair));

            return getNextPair(tuple.Item2);
        }


        //--------------------------------------------------
        /// <summary>
        /// Flips the type arguments of <paramref name="tuple" />
        /// </summary>
        public static (TRight, TLeft) Flip<TLeft, TRight>(this (TLeft, TRight) tuple)
        {
            var (left, right) = tuple;
            return (right, left);
        }


        //--------------------------------------------------
        /// <summary>
        /// Transform the left value contained in
        /// <paramref name="tuple" />.
        /// </summary>
        public static (TLeft2, TRight) MapLeft<TLeft1, TLeft2, TRight>(
            this (TLeft1, TRight) tuple,
            [NotNull] Func<TLeft1, TLeft2> valueSelector)
        {
            if (valueSelector is null) throw new ArgumentNullException(nameof(valueSelector));

            var (left, right) = tuple;
            return (valueSelector(left), right);
        }


        //--------------------------------------------------
        /// <summary>
        /// Transform the right value contained in
        /// <paramref name="tuple" />.
        /// </summary>
        public static (TLeft, TRight2) MapRight<TLeft, TRight1, TRight2>(
            this (TLeft, TRight1) tuple,
            [NotNull] Func<TRight1, TRight2> valueSelector)
        {
            if (valueSelector is null) throw new ArgumentNullException(nameof(valueSelector));

            var (left, right) = tuple;
            return (left, valueSelector(right));
        }


        //--------------------------------------------------
        /// <summary>
        /// Transform the right value contained in
        /// <paramref name="tuple" />.
        /// </summary>
        public static (TLeft2, TRight2) BiMap<TLeft1, TLeft2, TRight1, TRight2>(
            this (TLeft1, TRight1) tuple,
            [NotNull] Func<TLeft1, TLeft2> leftSelector,
            [NotNull] Func<TRight1, TRight2> rightSelector)
        {
            if (leftSelector is null) throw new ArgumentNullException(nameof(leftSelector));
            if (rightSelector is null) throw new ArgumentNullException(nameof(rightSelector));


            var (left, right) = tuple;
            return (leftSelector(left), rightSelector(right));
        }

    }

}