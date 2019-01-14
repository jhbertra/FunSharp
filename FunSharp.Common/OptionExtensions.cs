using System;
using JetBrains.Annotations;

namespace FunSharp.Common
{

    [PublicAPI]
    public static class OptionExtensions
    {

        [NotNull]
        public static Option<TResult> Bind<T, TResult>(
            [NotNull] this Option<T> option,
            [NotNull] Func<T, Option<TResult>> getNextOption)
        {
            if (option is null) { throw new ArgumentNullException(nameof(option)); }
            if (getNextOption is null) { throw new ArgumentNullException(nameof(getNextOption)); }

            switch (option)
            {
                case NoneOption<T> _: return Option<TResult>.None;
                case SomeOption<T> someOption: return getNextOption(someOption.Value);
                default: throw new ArgumentOutOfRangeException(nameof(option));
            }
        }

        [NotNull]
        public static T DefaultWith<T>(
            [NotNull] this Option<T> option,
            [NotNull] T defaultValue)
        {
            if (option is null) { throw new ArgumentNullException(nameof(option)); }
            if (defaultValue == null) { throw new ArgumentNullException(nameof(defaultValue)); }

            switch (option)
            {
                case NoneOption<T> _: return defaultValue;
                case SomeOption<T> someOption: return someOption.Value;
                default: throw new ArgumentOutOfRangeException(nameof(option));
            }
        }


        [NotNull]
        public static Option<TResult> Map<T, TResult>(
            [NotNull] this Option<T> option,
            [NotNull] Func<T, TResult> valueSelector)
        {
            if (option is null) { throw new ArgumentNullException(nameof(option)); }
            if (valueSelector is null) { throw new ArgumentNullException(nameof(valueSelector)); }

            switch (option)
            {
                case NoneOption<T> _: return Option<TResult>.None;
                case SomeOption<T> someOption: return Option.Some(valueSelector(someOption.Value));
                default: throw new ArgumentOutOfRangeException(nameof(option));
            }
        }


        [NotNull]
        public static TResult Match<T, TResult>(
            [NotNull] this Option<T> option,
            [NotNull] Func<T, TResult> valueSelector,
            [NotNull] Func<TResult> noValueSelector)
        {
            if (option is null) { throw new ArgumentNullException(nameof(option)); }
            if (valueSelector is null) { throw new ArgumentNullException(nameof(valueSelector)); }
            if (noValueSelector is null) { throw new ArgumentNullException(nameof(noValueSelector)); }

            switch (option)
            {
                case NoneOption<T> _: return noValueSelector();
                case SomeOption<T> someOption: return valueSelector(someOption.Value);
                default: throw new ArgumentOutOfRangeException(nameof(option));
            }
        }

    }

}