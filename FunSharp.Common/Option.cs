using System;
using System.Collections.Generic;
using System.Linq;
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

        public static Option<T> Choose<T>(params Option<T>[] options) =>
            options.Aggregate(
                Option<T>.None,
                (state, x) => state || x);

        public static Option<Unit> Guard(bool condition) => condition ? Option.Some<Unit>(default) : Option<Unit>.None;

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

        public bool IsEmpty => this is NoneOption<T>;

        public string Tag { get; }

        public static Option<T> operator |(Option<T> a, Option<T> b) => a.IsEmpty ? b.IsEmpty ? None : b : a;

        public static bool operator true(Option<T> a) => !a.IsEmpty;

        public static bool operator false(Option<T> a) => a.IsEmpty;


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