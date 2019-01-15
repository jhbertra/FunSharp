using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace FunSharp.Common
{

    //--------------------------------------------------
    /// <summary>
    /// Extensions for <see cref="object"/> instances
    /// (including generics).
    /// </summary>
    [PublicAPI]
    public static class ObjectExtensions
    {

        //--------------------------------------------------
        /// <summary>
        /// Call <see cref="f"/> with <paramref name="t1" />.
        /// </summary>
        [NotNull]
        public static T2 Pipe<T1, T2>([NotNull] this T1 t1, [NotNull] Func<T1, T2> f)
        {
            if (t1 == null) throw new ArgumentNullException(nameof(t1));
            if (f is null) throw new ArgumentNullException(nameof(f));

            return f(t1);
        }


        //--------------------------------------------------
        /// <summary>
        /// Wrap <paramref name="instance" /> in an
        /// <see cref="IEnumerable{T}"/>.
        /// </summary>
        [NotNull, ItemNotNull, Pure]
        public static IEnumerable<T> ToEnumerable<T>([NotNull] this T instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            yield return instance;
        }


        //--------------------------------------------------
        /// <summary>
        /// Wrap <paramref name="instance" /> in an
        /// <see cref="Either{TLeft,TRight}"/>.
        /// </summary>
        [NotNull, Pure]
        public static Either<TLeft, TRight> ToEitherLeft<TLeft, TRight>(
            [NotNull] this TLeft instance,
            TypeHint<TRight> tRightHint = default)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            return Either<TLeft, TRight>.Left(instance);
        }


        //--------------------------------------------------
        /// <summary>
        /// Wrap <paramref name="instance" /> in an
        /// <see cref="Either{TLeft,TRight}"/>.
        /// </summary>
        [NotNull, Pure]
        public static Either<TLeft, TRight> ToEitherRight<TLeft, TRight>(
            [NotNull] this TRight instance,
            TypeHint<TLeft> tLeftHint = default)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            return Either<TLeft, TRight>.Right(instance);
        }


        //--------------------------------------------------
        /// <summary>
        /// Wrap <paramref name="instance" /> in a
        /// <see cref="List{T}"/>.
        /// </summary>
        [NotNull, ItemNotNull, Pure]
        public static List<T> ToList<T>([NotNull] this T instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            return List.Cons(instance, List<T>.Empty);
        }


        //--------------------------------------------------
        /// <summary>
        /// Wrap <paramref name="instance" /> in an
        /// <see cref="Option{T}"/>.
        /// </summary>
        [NotNull, ItemNotNull, Pure]
        public static Option<T> ToOption<T>([NotNull] this T instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            return Option.Some(instance);
        }

    }

}