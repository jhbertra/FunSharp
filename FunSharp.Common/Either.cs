using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace FunSharp.Common
{

    [PublicAPI]
    public abstract class Either<TLeft, TRight> : StructuralEquality<Either<TLeft, TRight>>, IUnionType
    {

        [NotNull]
        public static Either<TLeft, TRight> Left([NotNull] TLeft value) => new EitherLeft<TLeft, TRight>(value);

        [NotNull]
        public static Either<TLeft, TRight> Right([NotNull] TRight value) => new EitherRight<TLeft, TRight>(value);

        public bool IsLeft => this is EitherLeft<TLeft, TRight>;

        protected Either([NotNull] string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(tag));

            this.Tag = tag;
        }

        public string Tag { get; }

    }


    [PublicAPI]
    public sealed class EitherLeft<TLeft, TRight> : Either<TLeft, TRight>
    {

        public EitherLeft([NotNull] TLeft value) : base("Left")
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.Value = value;
        }

        [NotNull] public readonly TLeft Value;

        protected override IEnumerable<(string FieldName, object FieldValue)> GetFields()
        {
            yield return (nameof(this.Value), Value);
        }

    }


    [PublicAPI]
    public sealed class EitherRight<TLeft, TRight> : Either<TLeft, TRight>
    {

        public EitherRight([NotNull] TRight value) : base("Right")
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            this.Value = value;
        }

        [NotNull] public readonly TRight Value;

        protected override IEnumerable<(string FieldName, object FieldValue)> GetFields()
        {
            yield return (nameof(this.Value), Value);
        }

    }

}