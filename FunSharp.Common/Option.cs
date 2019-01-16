using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace FunSharp.Common
{

    //**************************************************
    //* Module
    //**************************************************

    //--------------------------------------------------
    /// <inheritdoc cref="Option{T}"/>
    [PublicAPI]
    public static class Option
    {

        //--------------------------------------------------
        /// <inheritdoc cref="Option{T}.Some"/>
        [NotNull]
        public static Option<T> Some<T>([NotNull] T value) => Option<T>.Some(value);


        //--------------------------------------------------
        /// <inheritdoc cref="Option{T}.None"/>
        [NotNull]
        public static Option<T> None<T>() => Option<T>.None;


        //--------------------------------------------------
        /// <inheritdoc cref="OptionExtensions.Bind{T,TResult}"/>
        /// <remarks>
        /// This is a curried version of the extension method
        /// of the same name.
        /// </remarks>
        [NotNull]
        public static Func<Option<T>, Option<TResult>> Bind<T, TResult>(
            [NotNull] Func<T, Option<TResult>> getNextOption)
        {
            return x => x.Bind(getNextOption);
        }


        //--------------------------------------------------
        /// <summary>
        /// Pick the first <see cref="Option{T}"/> with a
        /// value, or nothing if none of them do.
        /// </summary>
        [NotNull]
        public static Option<T> Choose<T>(params Option<T>[] options) =>
            options.Aggregate(
                Option<T>.None,
                (state, x) => state || x);


        //--------------------------------------------------
        /// <inheritdoc cref="OptionExtensions.DefaultWith{T}"/>
        /// <remarks>
        /// This is a curried version of the extension method
        /// of the same name.
        /// </remarks>
        [NotNull]
        public static Func<Option<T>, T> DefaultWith<T>(
            [NotNull] T defaultValue)
        {
            return x => x.DefaultWith(defaultValue);
        }


        //--------------------------------------------------
        /// <inheritdoc cref="OptionExtensions.Filter{T}"/>
        /// <remarks>
        /// This is a curried version of the extension method
        /// of the same name.
        /// </remarks>
        [NotNull]
        public static Func<Option<T>, Option<T>> Filter<T>(
            [NotNull] Func<T, bool> predicate)
        {
            return x => x.Filter(predicate);
        }


        //--------------------------------------------------
        /// <summary>
        /// Encode <paramref name="condition" /> as an
        /// <see cref="Option{T}"/>, where the result has a
        /// value if <paramref name="condition" /> is true.
        /// </summary>
        [NotNull]
        public static Option<Unit> Guard(bool condition) =>
            condition
                ? Option.Some<Unit>(default)
                : Option<Unit>.None;


        //--------------------------------------------------
        /// <inheritdoc cref="OptionExtensions.Map{T,TResult}"/>
        /// <remarks>
        /// This is a curried version of the extension method
        /// of the same name.
        /// </remarks>
        [NotNull]
        public static Func<Option<T>, Option<TResult>> Map<T, TResult>(
            [NotNull] Func<T, TResult> valueSelector)
        {
            return x => x.Map(valueSelector);
        }


        //--------------------------------------------------
        /// <inheritdoc cref="OptionExtensions.Match{T,TResult}"/>
        /// <remarks>
        /// This is a curried version of the extension method
        /// of the same name.
        /// </remarks>
        [NotNull]
        public static Func<Option<T>, TResult> Match<T, TResult>(
            [NotNull] Func<T, TResult> valueSelector,
            [NotNull] Func<TResult> noValueSelector)
        {
            return x => x.Match(valueSelector, noValueSelector);
        }


        //--------------------------------------------------
        /// <param name="typeHint"></param>
        /// <inheritdoc cref="OptionExtensions.OfType{T,TCast}"/>
        /// <remarks>
        /// This is a curried version of the extension method
        /// of the same name.
        /// </remarks>
        [NotNull]
        public static Func<Option<T>, Option<TCast>> OfType<T, TCast>(TypeHint<TCast> typeHint = default) where TCast : T
        {
            return x => x.OfType(typeHint);
        }

    }


    //**************************************************
    //* Types
    //**************************************************

    //--------------------------------------------------
    /// <summary>
    /// Encodes an optional value.
    /// </summary>
    [PublicAPI]
    public abstract class Option<T> : StructuralEquality<Option<T>>, IUnionType, IEnumerable<T>, IEmpty
    {

        //**************************************************
        //* Constructors
        //**************************************************

        //--------------------------------------------------
        /// <summary>
        /// Create a new <see cref="Option{T}"/> that has
        /// a value.
        /// </summary>
        [NotNull]
        public static Option<T> Some([NotNull] T value) => new OptionSome<T>(value);


        //--------------------------------------------------
        /// <summary>
        /// Create a new <see cref="Option{T}"/> that has
        /// no value.
        /// </summary>
        [NotNull]
        public static Option<T> None => new OptionNone<T>();


        //--------------------------------------------------
        protected Option([NotNull] string tag)
        {
            this.Tag = tag;
        }


        //**************************************************
        //* Properties
        //**************************************************

        //--------------------------------------------------
        /// <inheritdoc />
        public bool IsEmpty => this is OptionNone<T>;


        //--------------------------------------------------
        /// <inheritdoc />
        public string Tag { get; }


        //**************************************************
        //* Methods
        //**************************************************

        //--------------------------------------------------
        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            if (this is OptionSome<T> some) yield return some.Value;
        }


        //--------------------------------------------------
        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }


        //**************************************************
        //* Operators
        //**************************************************

        public static Option<T> operator |(Option<T> a, Option<T> b) => a.IsEmpty ? b.IsEmpty ? None : b : a;

        public static bool operator true(Option<T> a) => !a.IsEmpty;

        public static bool operator false(Option<T> a) => a.IsEmpty;

    }


    //--------------------------------------------------
    /// <inheritdoc />
    /// <remarks>
    /// This type expresses an option that has a value.
    /// </remarks>
    [PublicAPI]
    public sealed class OptionSome<T> : Option<T>
    {

        //--------------------------------------------------
        /// <inheritdoc />
        public OptionSome([NotNull] T value) : base("Some")
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            this.Value = value;
        }


        //--------------------------------------------------
        /// <summary>
        /// The wrapped value.
        /// </summary>
        [NotNull] public readonly T Value;


        //--------------------------------------------------
        /// <inheritdoc />
        protected override IEnumerable<(string FieldName, object FieldValue)> GetFields()
        {
            yield return (nameof(this.Value), this.Value);
        }

    }


    //--------------------------------------------------
    /// <inheritdoc />
    /// <remarks>
    /// This type expresses an option that has no value.
    /// </remarks>
    [PublicAPI]
    public sealed class OptionNone<T> : Option<T>
    {

        //--------------------------------------------------
        /// <inheritdoc />
        public OptionNone() : base("None")
        {
        }


        //--------------------------------------------------
        /// <inheritdoc />
        protected override IEnumerable<(string FieldName, object FieldValue)> GetFields()
        {
            yield break;
        }

    }

}