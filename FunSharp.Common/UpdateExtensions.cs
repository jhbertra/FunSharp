using System;
using JetBrains.Annotations;

namespace FunSharp.Common
{

    //--------------------------------------------------
    /// <summary>
    /// Extensions for <see cref="Update{T}"/> instances.
    /// </summary>
    [PublicAPI]
    public static partial class UpdateExtensions
    {

        //--------------------------------------------------
        /// <summary>
        /// If <paramref name="update" /> has a value, use
        /// it to compute another <see cref="Update{T}"/>.
        /// </summary>
        [NotNull]
        public static Update<TResult> Bind<T, TResult>(
            [NotNull] this Update<T> update,
            [NotNull] Func<T, Update<TResult>> getNextUpdate)
        {
            if (update is null) throw new ArgumentNullException(nameof(update));
            if (getNextUpdate is null) throw new ArgumentNullException(nameof(getNextUpdate));

            switch (update)
            {
                case UpdateIgnore<T> _: return Update<TResult>.Ignore;
                case UpdateClear<T> _: return Update<TResult>.Clear;
                case UpdateSet<T> someUpdate: return getNextUpdate(someUpdate.Value);
                default: throw new ArgumentOutOfRangeException(nameof(update));
            }
        }


        //--------------------------------------------------
        /// <summary>
        /// If <paramref name="update" /> has a value, use
        /// it, or else use <paramref name="defaultValue" />.
        /// </summary>
        [NotNull]
        public static T DefaultWith<T>(
            [NotNull] this Update<T> update,
            [NotNull] T defaultValue)
        {
            if (update is null) throw new ArgumentNullException(nameof(update));
            if (defaultValue == null) throw new ArgumentNullException(nameof(defaultValue));

            switch (update)
            {
                case UpdateIgnore<T> _: return defaultValue;
                case UpdateClear<T> _: return defaultValue;
                case UpdateSet<T> someUpdate: return someUpdate.Value;
                default: throw new ArgumentOutOfRangeException(nameof(update));
            }
        }


        //--------------------------------------------------
        /// <summary>
        /// Produce an <see cref="Update{T}"/> with the value
        /// in <paramref name="update" />, only if it passes
        /// <paramref name="predicate" />.
        /// </summary>
        [NotNull]
        public static Update<T> Filter<T>(
            [NotNull] this Update<T> update,
            [NotNull] Func<T, bool> predicate)
        {
            if (update is null) throw new ArgumentNullException(nameof(update));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            switch (update)
            {
                case UpdateSet<T> someUpdate when predicate(someUpdate.Value):
                    return update;

                case UpdateSet<T> _:
                case UpdateIgnore<T> _:
                    return Update<T>.Ignore;
                case UpdateClear<T> _:
                    return Update<T>.Clear;

                default: throw new ArgumentOutOfRangeException(nameof(update));
            }
        }


        //--------------------------------------------------
        /// <summary>
        /// Transform the value contained in
        /// <paramref name="update" />.
        /// </summary>
        [NotNull]
        public static Update<TResult> Map<T, TResult>(
            [NotNull] this Update<T> update,
            [NotNull] Func<T, TResult> valueSelector)
        {
            if (update is null) throw new ArgumentNullException(nameof(update));
            if (valueSelector is null) throw new ArgumentNullException(nameof(valueSelector));

            switch (update)
            {
                case UpdateIgnore<T> _: return Update<TResult>.Ignore;
                case UpdateClear<T> _: return Update<TResult>.Clear;
                case UpdateSet<T> someUpdate: return Update.Set(valueSelector(someUpdate.Value));
                default: throw new ArgumentOutOfRangeException(nameof(update));
            }
        }


        //--------------------------------------------------
        /// <summary>
        /// Run the selector that matches the case of
        /// <paramref name="update" />.
        /// </summary>
        [NotNull]
        public static TResult Match<T, TResult>(
            [NotNull] this Update<T> update,
            [NotNull] Func<T, TResult> setSelector,
            [NotNull] Func<TResult> ignoreSelector,
            [NotNull] Func<TResult> clearSelector)
        {
            if (update is null) throw new ArgumentNullException(nameof(update));
            if (setSelector is null) throw new ArgumentNullException(nameof(setSelector));
            if (ignoreSelector is null) throw new ArgumentNullException(nameof(ignoreSelector));
            if (clearSelector is null) throw new ArgumentNullException(nameof(clearSelector));

            switch (update)
            {
                case UpdateIgnore<T> _: return ignoreSelector();
                case UpdateClear<T> _: return clearSelector();
                case UpdateSet<T> someUpdate: return setSelector(someUpdate.Value);
                default: throw new ArgumentOutOfRangeException(nameof(update));
            }
        }


        //--------------------------------------------------
        /// <summary>
        /// Return a new <see cref="Update{TCast}"/> if the
        /// value in <paramref name="update" /> is an instance
        /// of <typeparamref name="TCast"/>. 
        /// </summary>
        [NotNull]
        public static Update<TCast> OfType<T, TCast>(
            [NotNull] this Update<T> update,
            TypeHint<TCast> tHint = default) where TCast : T
        {
            if (update is null) throw new ArgumentNullException(nameof(update));

            return update.Bind(x => x is TCast result ? result.ToUpdate() : Update<TCast>.Ignore);
        }


        //--------------------------------------------------
        /// <summary>
        /// Resolve the value of updating an existing value.
        /// </summary>
        public static Option<T> Resolve<T>(
            [NotNull] this Update<T> update,
            [NotNull] Option<T> existingValue)
        {
            if (update is null) throw new ArgumentNullException(nameof(update));
            if (existingValue is null) throw new ArgumentNullException(nameof(existingValue));

            switch (update)
            {
                case UpdateClear<T> _: return Option<T>.None;
                case UpdateIgnore<T> _: return existingValue;
                case UpdateSet<T> updateSet: return updateSet.Value.ToOption();
                default: throw new ArgumentOutOfRangeException(nameof(update));
            }
        }


        //--------------------------------------------------
        /// <summary>
        /// Converts <paramref name="update" /> to an
        /// <see cref="Option{T}"/>.
        /// </summary>
        [NotNull]
        public static Option<T> ToOption<T>([NotNull] this Update<T> update)
        {
            if (update is null) throw new ArgumentNullException(nameof(update));

            return update.Match(
                Option.Some,
                () => Option<T>.None,
                () => Option<T>.None);
        }


        //--------------------------------------------------
        /// <summary>
        /// Converts <paramref name="update" /> to an
        /// <see cref="Either{TLeft,TRight}"/>, wrapping
        /// either the wrapped value or <paramref name="rightValue" />.
        /// </summary>
        public static Either<TLeft, TRight> ToEitherLeft<TLeft, TRight>(
            [NotNull] this Update<TLeft> update,
            [NotNull] TRight rightValue)
        {
            if (update is null) throw new ArgumentNullException(nameof(update));
            if (rightValue == null) throw new ArgumentNullException(nameof(rightValue));

            return update.ToOption().ToEitherLeft(rightValue);
        }


        //--------------------------------------------------
        /// <summary>
        /// Converts <paramref name="update" /> to an
        /// <see cref="Either{TLeft,TRight}"/>, wrapping
        /// either the wrapped value or <paramref name="leftValue" />.
        /// </summary>
        public static Either<TLeft, TRight> ToEitherRight<TLeft, TRight>(
            [NotNull] this Update<TRight> update,
            [NotNull] TLeft leftValue)
        {
            if (update is null) throw new ArgumentNullException(nameof(update));
            if (leftValue == null) throw new ArgumentNullException(nameof(leftValue));

            return update.ToOption().ToEitherRight(leftValue);
        }

    }

}