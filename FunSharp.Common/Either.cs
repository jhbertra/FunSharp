using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace FunSharp.Common
{

    //**************************************************
    //* Module
    //**************************************************

    //--------------------------------------------------
    /// <inheritdoc cref="Either{TLeft,TRight}"/>
    [PublicAPI]
    public static class Either
    {

        //--------------------------------------------------
        /// <inheritdoc cref="Either{TLeft,TRight}.Left"/>
        [NotNull]
        public static Either<TLeft, TRight> Left<TLeft, TRight>([NotNull] TLeft value, TypeHint<TRight> tRightHint)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            return Either<TLeft, TRight>.Left(value);
        }


        //--------------------------------------------------
        /// <inheritdoc cref="Either{TLeft,TRight}.Right"/>
        [NotNull]
        public static Either<TLeft, TRight> Right<TLeft, TRight>([NotNull] TRight value, TypeHint<TLeft> tLeftHint)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            return Either<TLeft, TRight>.Right(value);
        }

    }


    //**************************************************
    //* Types
    //**************************************************

    //--------------------------------------------------
    /// <summary>
    /// Encodes a choice between two alternatives.
    /// </summary>
    /// <typeparam name="TLeft">The type of the first alternative</typeparam>
    /// <typeparam name="TRight">The type of the second alternative</typeparam>
    [PublicAPI]
    public abstract class Either<TLeft, TRight> : StructuralEquality<Either<TLeft, TRight>>, IUnionType
    {

        //**************************************************
        //* Constructors
        //**************************************************

        //--------------------------------------------------
        /// <summary>
        /// Create a new <see cref="Either{TLeft,TRight}"/>
        /// resolved as the left choice.
        /// </summary>
        /// <param name="value">Resolved value of the choice.</param>
        [NotNull]
        public static Either<TLeft, TRight> Left([NotNull] TLeft value) => new EitherLeft<TLeft, TRight>(value);


        //--------------------------------------------------
        /// <summary>
        /// Create a new <see cref="Either{TLeft,TRight}"/>
        /// resolved as the right choice.
        /// </summary>
        /// <param name="value">Resolved value of the choice.</param>
        [NotNull]
        public static Either<TLeft, TRight> Right([NotNull] TRight value) => new EitherRight<TLeft, TRight>(value);


        //--------------------------------------------------
        protected Either([NotNull] string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(tag));

            this.Tag = tag;
        }


        //**************************************************
        //* Properties
        //**************************************************

        //--------------------------------------------------
        /// <summary>
        /// <code>true</code> if the encoded choice is the
        /// left alternative.
        /// </summary>
        public bool IsLeft => this is EitherLeft<TLeft, TRight>;


        //--------------------------------------------------
        /// <summary>
        /// <code>true</code> if the encoded choice is the
        /// right alternative.
        /// </summary>
        public bool IsRight => this is EitherRight<TLeft, TRight>;


        //--------------------------------------------------
        /// <inheritdoc />
        public string Tag { get; }

    }


    //--------------------------------------------------
    /// <inheritdoc />
    /// <remarks>
    /// This case class expresses that the left alternative
    /// was chosen.
    /// </remarks>
    [PublicAPI]
    public sealed class EitherLeft<TLeft, TRight> : Either<TLeft, TRight>
    {

        //--------------------------------------------------
        /// <inheritdoc />
        public EitherLeft([NotNull] TLeft value) : base(nameof(Either.Left))
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            this.Value = value;
        }


        //--------------------------------------------------
        /// <summary>
        /// The value of the choice.
        /// </summary>
        [NotNull] public readonly TLeft Value;


        //--------------------------------------------------
        /// <inheritdoc />
        protected override IEnumerable<(string FieldName, object FieldValue)> GetFields()
        {
            yield return (nameof(this.Value), Value);
        }

    }


    //--------------------------------------------------
    /// <inheritdoc />
    /// <remarks>
    /// This case class expresses that the right alternative
    /// was chosen.
    /// </remarks>
    [PublicAPI]
    public sealed class EitherRight<TLeft, TRight> : Either<TLeft, TRight>
    {

        //--------------------------------------------------
        /// <inheritdoc />
        public EitherRight([NotNull] TRight value) : base(nameof(Either.Right))
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            this.Value = value;
        }


        //--------------------------------------------------
        /// <summary>
        /// The value of the choice.
        /// </summary>
        [NotNull] public readonly TRight Value;


        //--------------------------------------------------
        /// <inheritdoc />
        protected override IEnumerable<(string FieldName, object FieldValue)> GetFields()
        {
            yield return (nameof(this.Value), Value);
        }

    }

}