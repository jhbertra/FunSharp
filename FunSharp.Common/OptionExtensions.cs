using System;
using JetBrains.Annotations;

namespace FunSharp.Common
{

    //--------------------------------------------------
    /// <summary>
    /// Extensions for <see cref="Option{T}"/> instances.
    /// </summary>
    [PublicAPI]
    public static partial class OptionExtensions
    {

        //--------------------------------------------------
        /// <summary>
        /// If <paramref name="option" /> has a value, use
        /// it to compute another <see cref="Option{T}"/>.
        /// </summary>
        [NotNull]
        public static Option<TResult> Bind<T, TResult>(
            [NotNull] this Option<T> option,
            [NotNull] Func<T, Option<TResult>> getNextOption)
        {
            if (option is null) throw new ArgumentNullException(nameof(option));
            if (getNextOption is null) throw new ArgumentNullException(nameof(getNextOption));

            switch (option)
            {
                case OptionNone<T> _: return Option<TResult>.None;
                case OptionSome<T> someOption: return getNextOption(someOption.Value);
                default: throw new ArgumentOutOfRangeException(nameof(option));
            }
        }


        //--------------------------------------------------
        /// <summary>
        /// If <paramref name="option" /> has a value, use
        /// it, or else use <paramref name="defaultValue" />.
        /// </summary>
        [NotNull]
        public static T DefaultWith<T>(
            [NotNull] this Option<T> option,
            [NotNull] T defaultValue)
        {
            if (option is null) throw new ArgumentNullException(nameof(option));
            if (defaultValue == null) throw new ArgumentNullException(nameof(defaultValue));

            switch (option)
            {
                case OptionNone<T> _: return defaultValue;
                case OptionSome<T> someOption: return someOption.Value;
                default: throw new ArgumentOutOfRangeException(nameof(option));
            }
        }


        //--------------------------------------------------
        /// <summary>
        /// Produce an <see cref="Option{T}"/> with the value
        /// in <paramref name="option" />, only if it passes
        /// <paramref name="predicate" />.
        /// </summary>
        [NotNull]
        public static Option<T> Filter<T>(
            [NotNull] this Option<T> option,
            [NotNull] Func<T, bool> predicate)
        {
            if (option is null) throw new ArgumentNullException(nameof(option));
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            switch (option)
            {
                case OptionSome<T> someOption when predicate(someOption.Value):
                    return option;

                case OptionSome<T> _:
                case OptionNone<T> _:
                    return Option<T>.None;

                default: throw new ArgumentOutOfRangeException(nameof(option));
            }
        }


        //--------------------------------------------------
        /// <summary>
        /// Transform the value contained in
        /// <paramref name="option" />.
        /// </summary>
        [NotNull]
        public static Option<TResult> Map<T, TResult>(
            [NotNull] this Option<T> option,
            [NotNull] Func<T, TResult> valueSelector)
        {
            if (option is null) throw new ArgumentNullException(nameof(option));
            if (valueSelector is null) throw new ArgumentNullException(nameof(valueSelector));

            switch (option)
            {
                case OptionNone<T> _: return Option<TResult>.None;
                case OptionSome<T> someOption: return Option.Some(valueSelector(someOption.Value));
                default: throw new ArgumentOutOfRangeException(nameof(option));
            }
        }


        //--------------------------------------------------
        /// <summary>
        /// If <paramref name="option" /> has a value, run
        /// <paramref name="valueSelector" />, or else run
        /// <paramref name="noValueSelector" />.
        /// </summary>
        [NotNull]
        public static TResult Match<T, TResult>(
            [NotNull] this Option<T> option,
            [NotNull] Func<T, TResult> valueSelector,
            [NotNull] Func<TResult> noValueSelector)
        {
            if (option is null) throw new ArgumentNullException(nameof(option));
            if (valueSelector is null) throw new ArgumentNullException(nameof(valueSelector));
            if (noValueSelector is null) throw new ArgumentNullException(nameof(noValueSelector));

            switch (option)
            {
                case OptionNone<T> _: return noValueSelector();
                case OptionSome<T> someOption: return valueSelector(someOption.Value);
                default: throw new ArgumentOutOfRangeException(nameof(option));
            }
        }


        //--------------------------------------------------
        /// <summary>
        /// Return a new <see cref="Option{TCast}"/> if the
        /// value in <paramref name="option" /> is an instance
        /// of <typeparamref name="TCast"/>. 
        /// </summary>
        [NotNull]
        public static Option<TCast> OfType<T, TCast>(
            [NotNull] this Option<T> option,
            TypeHint<TCast> tHint) where TCast : T
        {
            if (option is null) throw new ArgumentNullException(nameof(option));

            return option.Bind(x => x is TCast result ? result.ToOption() : Option<TCast>.None);
        }

    }

}