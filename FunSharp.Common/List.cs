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
            if (string.IsNullOrWhiteSpace(tag))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(tag));

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
        protected override IEnumerable<(string FieldName, object FieldValue)> GetFields() =>
            this.Select((x, i) => ($"[{i}]", (object) x));


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

    }

}