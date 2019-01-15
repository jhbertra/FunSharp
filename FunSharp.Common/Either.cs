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
        /// <remarks>
        /// This is a curried version of the extension method
        /// of the same name.
        /// </remarks>
        [NotNull]
        public static Either<TLeft, TRight> Left<TLeft, TRight>([NotNull] TLeft value, TypeHint<TRight> tRightHint = default)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            return Either<TLeft, TRight>.Left(value);
        }


        //--------------------------------------------------
        /// <inheritdoc cref="Either{TLeft,TRight}.Right"/>
        /// <remarks>
        /// This is a curried version of the extension method
        /// of the same name.
        /// </remarks>
        [NotNull]
        public static Either<TLeft, TRight> Right<TLeft, TRight>([NotNull] TRight value, TypeHint<TLeft> tLeftHint = default)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            return Either<TLeft, TRight>.Right(value);
        }

        //--------------------------------------------------
        /// <inheritdoc cref="EitherExtensions.BindLeft{TLeft1,TLeft2,TRight}" />
        /// <remarks>
        /// This is a curried version of the extension method
        /// of the same name.
        /// </remarks>
        [NotNull]
        public static Func<Either<TLeft1, TRight>, Either<TLeft2, TRight>> BindLeft<TLeft1, TLeft2, TRight>(
            [NotNull] Func<TLeft1, Either<TLeft2, TRight>> makeNextChoice)
        {
            if (makeNextChoice is null) throw new ArgumentNullException(nameof(makeNextChoice));

            return x => x.BindLeft(makeNextChoice);
        }


        //--------------------------------------------------
        /// <inheritdoc cref="EitherExtensions.BindRight{TLeft,TRight1,TRight2}" />
        /// <remarks>
        /// This is a curried version of the extension method
        /// of the same name.
        /// </remarks>
        [NotNull]
        public static Func<Either<TLeft, TRight1>, Either<TLeft, TRight2>> BindRight<TLeft, TRight1, TRight2>(
            [NotNull] Func<TRight1, Either<TLeft, TRight2>> makeNextChoice)
        {
            if (makeNextChoice is null) throw new ArgumentNullException(nameof(makeNextChoice));

            return x => x.BindRight(makeNextChoice);
        }


        //--------------------------------------------------
        /// <inheritdoc cref="EitherExtensions.DefaultLeftWith{TLeft,TRight}" />
        /// <remarks>
        /// This is a curried version of the extension method
        /// of the same name.
        /// </remarks>
        [NotNull]
        public static Func<Either<TLeft, TRight>, TLeft> DefaultLeftWith<TLeft, TRight>([NotNull] TLeft defaultValue)
        {
            if (defaultValue == null) throw new ArgumentNullException(nameof(defaultValue));

            return x => x.DefaultLeftWith(defaultValue);
        }


        //--------------------------------------------------
        /// <inheritdoc cref="EitherExtensions.DefaultRightWith{TLeft,TRight}" />
        /// <remarks>
        /// This is a curried version of the extension method
        /// of the same name.
        /// </remarks>
        [NotNull]
        public static Func<Either<TLeft, TRight>, TRight> DefaultRightWith<TLeft, TRight>([NotNull] TRight defaultValue)
        {
            if (defaultValue == null) throw new ArgumentNullException(nameof(defaultValue));

            return x => x.DefaultRightWith(defaultValue);
        }


        //--------------------------------------------------
        /// <inheritdoc cref="EitherExtensions.MapLeft{TLeft1,TLeft2,TRight}" />
        /// <remarks>
        /// This is a curried version of the extension method
        /// of the same name.
        /// </remarks>
        [NotNull]
        public static Func<Either<TLeft1, TRight>, Either<TLeft2, TRight>> MapLeft<TLeft1, TLeft2, TRight>(
            [NotNull] Func<TLeft1, TLeft2> valueSelector)
        {
            if (valueSelector is null) throw new ArgumentNullException(nameof(valueSelector));

            return x => x.MapLeft(valueSelector);
        }


        //--------------------------------------------------
        /// <inheritdoc cref="EitherExtensions.MapRight{TLeft,TRight1,TRight2}" />
        /// <remarks>
        /// This is a curried version of the extension method
        /// of the same name.
        /// </remarks>
        [NotNull]
        public static Func<Either<TLeft, TRight1>, Either<TLeft, TRight2>> MapRight<TLeft, TRight1, TRight2>(
            [NotNull] Func<TRight1, TRight2> valueSelector)
        {
            if (valueSelector is null) throw new ArgumentNullException(nameof(valueSelector));

            return x => x.MapRight(valueSelector);
        }


        //--------------------------------------------------
        /// <inheritdoc cref="EitherExtensions.BiMap{TLeft1,TLeft2,TRight1,TRight2}" />
        /// <remarks>
        /// This is a curried version of the extension method
        /// of the same name.
        /// </remarks>
        [NotNull]
        public static Func<Either<TLeft1, TRight1>, Either<TLeft2, TRight2>> BiMap<TLeft1, TLeft2, TRight1, TRight2>(
            [NotNull] Func<TLeft1, TLeft2> leftSelector,
            [NotNull] Func<TRight1, TRight2> rightSelector)
        {
            if (leftSelector is null) throw new ArgumentNullException(nameof(leftSelector));
            if (rightSelector is null) throw new ArgumentNullException(nameof(rightSelector));

            return x => x.BiMap(leftSelector, rightSelector);
        }


        //--------------------------------------------------
        /// <inheritdoc cref="EitherExtensions.Match{TLeft,TRight,TResult}" />
        /// <remarks>
        /// This is a curried version of the extension method
        /// of the same name.
        /// </remarks>
        [NotNull]
        public static Func<Either<TLeft, TRight>, TResult> Match<TLeft, TRight, TResult>(
            [NotNull] Func<TLeft, TResult> leftSelector,
            [NotNull] Func<TRight, TResult> rightSelector)
        {
            if (leftSelector is null) throw new ArgumentNullException(nameof(leftSelector));
            if (rightSelector is null) throw new ArgumentNullException(nameof(rightSelector));

            return x => x.Match(leftSelector, rightSelector);
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