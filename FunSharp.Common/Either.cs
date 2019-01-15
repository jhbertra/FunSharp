using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace FunSharp.Common
{

    //**************************************************
    //* Module
    //**************************************************

    [PublicAPI]
    public static class Either
    {

        public static Either<TLeft, TRight> Left<TLeft, TRight>([NotNull] TLeft value, Type<TRight> tRight)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (tRight is null) throw new ArgumentNullException(nameof(tRight));

            return Either<TLeft, TRight>.Left(value);
        }

        public static Either<TLeft, TRight> Right<TLeft, TRight>([NotNull] TRight value, Type<TLeft> tLeft)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (tLeft is null) throw new ArgumentNullException(nameof(tLeft));

            return Either<TLeft, TRight>.Right(value);
        }
        
    }


    //**************************************************
    //* Types
    //**************************************************

    [PublicAPI]
    public abstract class Either<TLeft, TRight> : StructuralEquality<Either<TLeft, TRight>>, IUnionType
    {

        //**************************************************
        //* Constructors
        //**************************************************

        [NotNull]
        public static Either<TLeft, TRight> Left([NotNull] TLeft value) => new EitherLeft<TLeft, TRight>(value);

        [NotNull]
        public static Either<TLeft, TRight> Right([NotNull] TRight value) => new EitherRight<TLeft, TRight>(value);

        protected Either([NotNull] string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(tag));

            this.Tag = tag;
        }


        //**************************************************
        //* Properties
        //**************************************************

        public bool IsLeft => this is EitherLeft<TLeft, TRight>;

        public bool IsRight => this is EitherLeft<TLeft, TRight>;

        public string Tag { get; }

    }


    [PublicAPI]
    public sealed class EitherLeft<TLeft, TRight> : Either<TLeft, TRight>
    {

        public EitherLeft([NotNull] TLeft value) : base("Left")
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

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
            if (value == null) throw new ArgumentNullException(nameof(value));

            this.Value = value;
        }

        [NotNull] public readonly TRight Value;

        protected override IEnumerable<(string FieldName, object FieldValue)> GetFields()
        {
            yield return (nameof(this.Value), Value);
        }

    }

}