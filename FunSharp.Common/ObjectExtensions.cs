using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace FunSharp.Common
{

    [PublicAPI]
    public static class ObjectExtensions
    {

        [NotNull]
        public static T2 Pipe<T1, T2>([NotNull] this T1 t1, [NotNull] Func<T1, T2> f)
        {
            if (t1 == null) throw new ArgumentNullException(nameof(t1));
            if (f is null) { throw new ArgumentNullException(nameof(f)); }

            return f(t1);
        }


        [NotNull, ItemNotNull, Pure]
        public static IEnumerable<T> ToEnumerable<T>([NotNull] this T instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            yield return instance;
        }


        [NotNull, Pure]
        public static Either<TLeft, TRight> ToEitherLeft<TLeft, TRight>(
            [NotNull] this TLeft instance,
            [NotNull] Type<TRight> tRight)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (tRight is null) { throw new ArgumentNullException(nameof(tRight)); }

            return Either<TLeft, TRight>.Left(instance);
        }


        [NotNull, Pure]
        public static Either<TLeft, TRight> ToEitherRight<TLeft, TRight>(
            [NotNull] this TRight instance,
            [NotNull] Type<TLeft> tLeft)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (tLeft is null) { throw new ArgumentNullException(nameof(tLeft)); }

            return Either<TLeft, TRight>.Right(instance);
        }


        [NotNull, ItemNotNull, Pure]
        public static List<T> ToList<T>([NotNull] this T instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            return List.Cons(instance, List<T>.Empty);
        }


        [NotNull, ItemNotNull, Pure]
        public static Option<T> ToOption<T>([NotNull] this T instance)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));

            return Option.Some(instance);
        }

    }

}