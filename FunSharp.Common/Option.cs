using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace FunSharp.Common
{

    [PublicAPI]
    public static class Option
    {

        [NotNull]
        public static Option<T> Some<T>([NotNull] T value) => Option<T>.Some(value);

        [NotNull]
        public static Option<T> None<T>() => Option<T>.None;

    }


    [PublicAPI]
    public abstract class Option<T> : StructuralEquality<Option<T>>, IUnionType
    {

        [NotNull]
        public static Option<T> Some([NotNull] T value) => new SomeOption<T>(value);

        [NotNull]
        public static Option<T> None => new NoneOption<T>();

        protected Option([NotNull] string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(tag));

            this.Tag = tag;
        }

        public string Tag { get; }

    }


    [PublicAPI]
    public sealed class SomeOption<T> : Option<T>
    {

        public SomeOption([NotNull] T value) : base("Some")
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.Value = value;
        }

        [NotNull] public readonly T Value;

        protected override IEnumerable<(string FieldName, object FieldValue)> GetFields()
        {
            yield return (nameof(this.Value), this.Value);
        }

    }


    [PublicAPI]
    public sealed class NoneOption<T> : Option<T>
    {

        public NoneOption() : base("None")
        {
        }

        protected override IEnumerable<(string FieldName, object FieldValue)> GetFields()
        {
            yield break;
        }

    }

}