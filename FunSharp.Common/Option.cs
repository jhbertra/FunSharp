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


        [NotNull]
        public static Func<Option<T>, Option<TResult>> Bind<T, TResult>(
            [NotNull] Func<T, Option<TResult>> getNextOption)
        {
            if (getNextOption is null) throw new ArgumentNullException(nameof(getNextOption));

            return x => x.Bind(getNextOption);
        }


        [NotNull]
        public static Func<Option<T>, T> DefaultWith<T>(
            [NotNull] T defaultValue)
        {
            if (defaultValue == null) throw new ArgumentNullException(nameof(defaultValue));

            return x => x.DefaultWith(defaultValue);
        }


        [NotNull]
        public static Func<Option<T>, Option<T>> Filter<T>(
            [NotNull] Func<T, bool> predicate)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            return x => x.Filter(predicate);
        }


        [NotNull]
        public static Func<Option<T>, Option<TResult>> Map<T, TResult>(
            [NotNull] Func<T, TResult> valueSelector)
        {
            if (valueSelector is null) throw new ArgumentNullException(nameof(valueSelector));

            return x => x.Map(valueSelector);
        }


        [NotNull]
        public static Func<Option<T>, TResult> Match<T, TResult>(
            [NotNull] Func<T, TResult> valueSelector,
            [NotNull] Func<TResult> noValueSelector)
        {
            if (valueSelector is null) throw new ArgumentNullException(nameof(valueSelector));
            if (noValueSelector is null) throw new ArgumentNullException(nameof(noValueSelector));

            return x => x.Match(valueSelector, noValueSelector);
        }


        [NotNull]
        public static Func<Option<T>, Option<TCast>> OfType<T, TCast>(
            [NotNull] Type<TCast> type) where TCast : T
        {
            if (type is null) throw new ArgumentNullException(nameof(type));

            return x => x.OfType(type);
        }

    }


    //**************************************************
    //* Types
    //**************************************************

    [PublicAPI]
    public abstract class Option<T> : StructuralEquality<Option<T>>, IUnionType, IEnumerable<T>, IEmpty
    {

        //**************************************************
        //* Constructors
        //**************************************************

        [NotNull]
        public static Option<T> Some([NotNull] T value) => new OptionSome<T>(value);

        [NotNull]
        public static Option<T> None => new OptionNone<T>();

        protected Option([NotNull] string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(tag));

            this.Tag = tag;
        }

        
        //**************************************************
        //* Properties
        //**************************************************

        public bool IsEmpty => this is OptionNone<T>;

        public string Tag { get; }

        
        //**************************************************
        //* Methods
        //**************************************************

        public IEnumerator<T> GetEnumerator()
        {
            if (this is OptionSome<T> some)
            {
                yield return some.Value;
            }
        }

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


    [PublicAPI]
    public sealed class OptionSome<T> : Option<T>
    {

        public OptionSome([NotNull] T value) : base("Some")
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            this.Value = value;
        }

        [NotNull] public readonly T Value;

        protected override IEnumerable<(string FieldName, object FieldValue)> GetFields()
        {
            yield return (nameof(this.Value), this.Value);
        }

    }


    [PublicAPI]
    public sealed class OptionNone<T> : Option<T>
    {

        public OptionNone() : base("None")
        {
        }

        protected override IEnumerable<(string FieldName, object FieldValue)> GetFields()
        {
            yield break;
        }

    }

}