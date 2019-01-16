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
    /// <inheritdoc cref="Update{T}"/>
    [PublicAPI]
    public static class Update
    {

        //--------------------------------------------------
        /// <inheritdoc cref="Update{T}.Set"/>
        [NotNull]
        public static Update<T> Set<T>([NotNull] T value) => Update<T>.Set(value);


        //--------------------------------------------------
        /// <inheritdoc cref="Update{T}.Ignore"/>
        [NotNull]
        public static Update<T> Ignore<T>() => Update<T>.Ignore;


        //--------------------------------------------------
        /// <inheritdoc cref="Update{T}.Clear"/>
        [NotNull]
        public static Update<T> Clear<T>() => Update<T>.Clear;


        //--------------------------------------------------
        /// <inheritdoc cref="UpdateExtensions.Bind{T,TResult}"/>
        /// <remarks>
        /// This is a curried version of the extension method
        /// of the same name.
        /// </remarks>
        [NotNull]
        public static Func<Update<T>, Update<TResult>> Bind<T, TResult>(
            [NotNull] Func<T, Update<TResult>> getNextUpdate)
        {
            return x => x.Bind(getNextUpdate);
        }


        //--------------------------------------------------
        /// <summary>
        /// Pick the first <see cref="Update{T}"/> with a
        /// value, or nothing if none of them do.
        /// </summary>
        [NotNull]
        public static Update<T> Choose<T>(params Update<T>[] options) =>
            options.Aggregate(
                Update<T>.Ignore,
                (state, x) => state || x);


        //--------------------------------------------------
        /// <inheritdoc cref="UpdateExtensions.DefaultWith{T}"/>
        /// <remarks>
        /// This is a curried version of the extension method
        /// of the same name.
        /// </remarks>
        [NotNull]
        public static Func<Update<T>, T> DefaultWith<T>(
            [NotNull] T defaultValue)
        {
            return x => x.DefaultWith(defaultValue);
        }


        //--------------------------------------------------
        /// <inheritdoc cref="UpdateExtensions.Filter{T}"/>
        /// <remarks>
        /// This is a curried version of the extension method
        /// of the same name.
        /// </remarks>
        [NotNull]
        public static Func<Update<T>, Update<T>> Filter<T>(
            [NotNull] Func<T, bool> predicate)
        {
            return x => x.Filter(predicate);
        }


        //--------------------------------------------------
        /// <summary>
        /// Encode <paramref name="condition" /> as an
        /// <see cref="Update{T}"/>, where the result has a
        /// value if <paramref name="condition" /> is true.
        /// </summary>
        [NotNull]
        public static Update<Unit> Guard(bool condition) =>
            condition
                ? Update.Set<Unit>(default)
                : Update<Unit>.Ignore;


        //--------------------------------------------------
        /// <inheritdoc cref="UpdateExtensions.Map{T,TResult}"/>
        /// <remarks>
        /// This is a curried version of the extension method
        /// of the same name.
        /// </remarks>
        [NotNull]
        public static Func<Update<T>, Update<TResult>> Map<T, TResult>(
            [NotNull] Func<T, TResult> valueSelector)
        {
            return x => x.Map(valueSelector);
        }


        //--------------------------------------------------
        /// <inheritdoc cref="UpdateExtensions.Match{T,TResult}"/>
        /// <remarks>
        /// This is a curried version of the extension method
        /// of the same name.
        /// </remarks>
        [NotNull]
        public static Func<Update<T>, TResult> Match<T, TResult>(
            [NotNull] Func<T, TResult> setSelector,
            [NotNull] Func<TResult> ignoreSelector,
            [NotNull] Func<TResult> clearSelector)
        {
            return x => x.Match(setSelector, ignoreSelector, clearSelector);
        }


        //--------------------------------------------------
        /// <param name="typeHint"></param>
        /// <inheritdoc cref="UpdateExtensions.OfType{T,TCast}"/>
        /// <remarks>
        /// This is a curried version of the extension method
        /// of the same name.
        /// </remarks>
        [NotNull]
        public static Func<Update<T>, Update<TCast>> OfType<T, TCast>(TypeHint<TCast> typeHint = default) where TCast : T
        {
            return x => x.OfType(typeHint);
        }

    }


    //**************************************************
    //* Types
    //**************************************************

    //--------------------------------------------------
    /// <summary>
    /// Encodes an intent to update a value.
    /// </summary>
    [PublicAPI]
    public abstract class Update<T> : StructuralEquality<Update<T>>, IUnionType, IEnumerable<T>, IEmpty
    {

        //**************************************************
        //* Constructors
        //**************************************************

        //--------------------------------------------------
        /// <summary>
        /// Create a new <see cref="Update{T}"/> that sets
        /// a value.
        /// </summary>
        [NotNull]
        public static Update<T> Set([NotNull] T value) => new UpdateSet<T>(value);


        //--------------------------------------------------
        /// <summary>
        /// Create a new <see cref="Update{T}"/> that ignores
        /// existing values.
        /// </summary>
        [NotNull]
        public static Update<T> Ignore => new UpdateIgnore<T>();


        //--------------------------------------------------
        /// <summary>
        /// Create a new <see cref="Update{T}"/> that clears
        /// existing values.
        /// </summary>
        [NotNull]
        public static Update<T> Clear => new UpdateClear<T>();


        //--------------------------------------------------
        protected Update([NotNull] string tag)
        {
            this.Tag = tag;
        }


        //**************************************************
        //* Properties
        //**************************************************

        //--------------------------------------------------
        /// <inheritdoc />
        public bool IsEmpty => this is UpdateIgnore<T>;


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
            if (this is UpdateSet<T> some) yield return some.Value;
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

        public static Update<T> operator |(Update<T> a, Update<T> b) => a.IsEmpty ? b.IsEmpty ? Ignore : b : a;

        public static bool operator true(Update<T> a) => !a.IsEmpty;

        public static bool operator false(Update<T> a) => a.IsEmpty;

    }


    //--------------------------------------------------
    /// <inheritdoc />
    /// <remarks>
    /// This type expresses an update that sets a value.
    /// </remarks>
    [PublicAPI]
    public sealed class UpdateSet<T> : Update<T>
    {

        //--------------------------------------------------
        /// <inheritdoc />
        public UpdateSet([NotNull] T value) : base("Set")
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
    /// This type expresses an update that ignores values.
    /// </remarks>
    [PublicAPI]
    public sealed class UpdateIgnore<T> : Update<T>
    {

        //--------------------------------------------------
        /// <inheritdoc />
        public UpdateIgnore() : base("Ignore")
        {
        }


        //--------------------------------------------------
        /// <inheritdoc />
        protected override IEnumerable<(string FieldName, object FieldValue)> GetFields()
        {
            yield break;
        }

    }


    //--------------------------------------------------
    /// <inheritdoc />
    /// <remarks>
    /// This type expresses an update that clears values.
    /// </remarks>
    [PublicAPI]
    public sealed class UpdateClear<T> : Update<T>
    {

        //--------------------------------------------------
        /// <inheritdoc />
        public UpdateClear() : base("Clear")
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