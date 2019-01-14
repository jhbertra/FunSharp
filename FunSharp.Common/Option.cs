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
        public static Option<T> Some([NotNull] T value) => new Some<T>(value);

        [NotNull]
        public static Option<T> None => new None<T>();

        protected Option([NotNull] string tag)
        {
            Tag = tag ?? throw new ArgumentNullException(nameof(tag));
        }

        public string Tag { get; }

    }


    [PublicAPI]
    public sealed class Some<T> : Option<T>
    {

        public Some([NotNull] T value) : base("Some")
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
        }

        [NotNull] public readonly T Value;

        protected override IEnumerable<(string FieldName, object FieldValue)> GetFields()
        {
            yield return (nameof(this.Value), this.Value);
        }

    }


    [PublicAPI]
    public sealed class None<T> : Option<T>
    {

        public None() : base("None")
        {
        }

        protected override IEnumerable<(string FieldName, object FieldValue)> GetFields()
        {
            yield break;
        }

    }

}