using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace FunSharp.Common
{

    //**************************************************
    //* Module
    //**************************************************

    //--------------------------------------------------
    /// <inheritdoc cref="List{T}"/>
    [PublicAPI]
    public static class List
    {

        //--------------------------------------------------
        /// <inheritdoc cref="List{T}.Cons"/>
        [NotNull]
        public static List<T> Cons<T>([NotNull] T value, List<T> tail) => List<T>.Cons(value, tail);


        //--------------------------------------------------
        /// <inheritdoc cref="List{T}.Empty"/>
        [NotNull]
        public static List<T> Empty<T>() => List<T>.Empty;

        
        //--------------------------------------------------
        /// <inheritdoc cref="ListExtensions.Bind{T,TResult}"/>
        /// <remarks>
        /// This is a curried version of the extension method
        /// of the same name.
        /// </remarks>
        [NotNull]
        public static Func<List<T>, List<TResult>> Bind<T, TResult>(
            [NotNull] Func<T, List<TResult>> getSubList)
        {
            return x => x.Bind(getSubList);
        }


        //--------------------------------------------------
        /// <inheritdoc cref="ListExtensions.Filter{T}"/>
        /// <remarks>
        /// This is a curried version of the extension method
        /// of the same name.
        /// </remarks>
        [NotNull]
        public static Func<List<T>, List<T>> Filter<T>(
            [NotNull] Func<T, bool> predicate)
        {
            return x => x.Filter(predicate);
        }


        //--------------------------------------------------
        /// <inheritdoc cref="ListExtensions.Map{T,TResult}"/>
        /// <remarks>
        /// This is a curried version of the extension method
        /// of the same name.
        /// </remarks>
        [NotNull]
        public static Func<List<T>, List<TResult>> Map<T, TResult>(
            [NotNull] Func<T, TResult> valueSelector)
        {
            return x => x.Map(valueSelector);
        }


        //--------------------------------------------------
        /// <inheritdoc cref="ListExtensions.Match{T,TResult}"/>
        /// <remarks>
        /// This is a curried version of the extension method
        /// of the same name.
        /// </remarks>
        [NotNull]
        public static Func<List<T>, TResult> Match<T, TResult>(
            [NotNull] Func<T, List<T>, TResult> consSelector,
            [NotNull] Func<TResult> emptySelector)
        {
            return x => x.Match(consSelector, emptySelector);
        }


        //--------------------------------------------------
        /// <param name="typeHint"></param>
        /// <inheritdoc cref="ListExtensions.OfType{T,TCast}"/>
        /// <remarks>
        /// This is a curried version of the extension method
        /// of the same name.
        /// </remarks>
        [NotNull]
        public static Func<List<T>, List<TCast>> OfType<T, TCast>(TypeHint<TCast> typeHint = default) where TCast : T
        {
            return x => x.OfType(typeHint);
        }

    }


    //**************************************************
    //* Types
    //**************************************************

    //--------------------------------------------------
    /// <summary>
    /// An immutable, singly-linked-list.
    /// </summary>
    [PublicAPI]
    public abstract class List<T> :
        StructuralEquality<List<T>>,
        IUnionType,
        IEnumerable<T>,
        IAppendable<List<T>>,
        IEmpty
    {

        //**************************************************
        //* Constructors
        //**************************************************

        //--------------------------------------------------
        /// <summary>
        /// Create a new list with a head and a tail.
        /// </summary>
        [NotNull]
        public static List<T> Cons([NotNull] T value, List<T> tail) => new ListCons<T>(value, tail);


        //--------------------------------------------------
        /// <summary>
        /// Create a new list with no elements.
        /// </summary>
        [NotNull]
        public static List<T> Empty => new ListEmpty<T>();


        //--------------------------------------------------
        protected List([NotNull] string tag)
        {
            this.Tag = tag;
        }


        //**************************************************
        //* Properties
        //**************************************************

        //--------------------------------------------------
        /// <inheritdoc />
        public bool IsEmpty => this is ListEmpty<T>;


        //--------------------------------------------------
        /// <inheritdoc />
        public string Tag { get; }


        //**************************************************
        //* Methods
        //**************************************************

        //--------------------------------------------------
        /// <inheritdoc />
        public List<T> Append(List<T> t)
        {
            var stack = new Stack<T>();
            foreach (var item in this)
            {
                stack.Push(item);
            }

            while (stack.Count > 0)
            {
                t = List.Cons(stack.Pop(), t);
            }

            return t;
        }

        //--------------------------------------------------
        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            var list = this;
            while (list is ListCons<T> cons)
            {
                yield return cons.Head;
                list = cons.Tail;
            }
        }


        //--------------------------------------------------
        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();


        //--------------------------------------------------
        /// <inheritdoc />
        public override string ToString() => $"[{string.Join(", ", this.Select(x => x.ToString()))}]";


        //**************************************************
        //* Operators
        //**************************************************

        public static List<T> operator +([NotNull] List<T> left, [NotNull] List<T> right) => left.Append(right);

    }


    //--------------------------------------------------
    /// <inheritdoc />
    /// <remarks>
    /// This type expresses a list that is constructed from
    /// a head and a tail.
    /// </remarks>
    [PublicAPI]
    public sealed class ListCons<T> : List<T>
    {

        //--------------------------------------------------
        /// <inheritdoc />
        public ListCons([NotNull] T head, [NotNull] List<T> tail) : base(nameof(List.Cons))
        {
            if (head == null) throw new ArgumentNullException(nameof(head));

            this.Head = head;
            this.Tail = tail ?? throw new ArgumentNullException(nameof(tail));
        }


        //--------------------------------------------------
        /// <summary>
        /// The head element of the list.
        /// </summary>
        [NotNull] public readonly T Head;


        //--------------------------------------------------
        /// <summary>
        /// The tail element of the list.
        /// </summary>
        [NotNull] public readonly List<T> Tail;


        //--------------------------------------------------
        /// <inheritdoc />
        protected override IEnumerable<(string FieldName, object FieldValue)> GetFields()
        {
            yield return (nameof(Head), this.Head);
            yield return (nameof(Tail), this.Tail);
        }

    }


    //--------------------------------------------------
    /// <inheritdoc />
    /// <remarks>
    /// This type expresses a list that is empty.
    /// </remarks>
    [PublicAPI]
    public sealed class ListEmpty<T> : List<T>
    {

        //--------------------------------------------------
        /// <inheritdoc />
        public ListEmpty() : base(nameof(List.Empty))
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