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
    public static class List
    {

        [NotNull]
        public static List<T> Cons<T>([NotNull] T value, List<T> tail) => List<T>.Cons(value, tail);

        [NotNull]
        public static List<T> Empty<T>() => List<T>.Empty;

    }


    //**************************************************
    //* Types
    //**************************************************

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

        [NotNull]
        public static List<T> Cons([NotNull] T value, List<T> tail) => new ListCons<T>(value, tail);

        [NotNull]
        public static List<T> Empty => new ListEmpty<T>();

        protected List([NotNull] string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(tag));

            this.Tag = tag;
        }


        //**************************************************
        //* Properties
        //**************************************************

        public bool IsEmpty => this is ListEmpty<T>;

        public string Tag { get; }


        //**************************************************
        //* Methods
        //**************************************************

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

        public IEnumerator<T> GetEnumerator()
        {
            var list = this;
            while (list is ListCons<T> cons)
            {
                yield return cons.Head;
                list = cons.Tail;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        protected override IEnumerable<(string FieldName, object FieldValue)> GetFields() =>
            this.Select((x, i) => ($"[{i}]", (object) x));

        public override string ToString() => $"[{string.Join(", ", this.Select(x => x.ToString()))}]";


        //**************************************************
        //* Operators
        //**************************************************

        public static List<T> operator +([NotNull] List<T> left, [NotNull] List<T> right) => left.Append(right);

    }


    [PublicAPI]
    public sealed class ListCons<T> : List<T>
    {

        public ListCons([NotNull] T head, [NotNull] List<T> tail) : base("Cons")
        {
            if (head == null) throw new ArgumentNullException(nameof(head));

            this.Head = head;
            this.Tail = tail ?? throw new ArgumentNullException(nameof(tail));
        }

        [NotNull] public readonly T Head;
        [NotNull] public readonly List<T> Tail;

    }


    [PublicAPI]
    public sealed class ListEmpty<T> : List<T>
    {

        public ListEmpty() : base("Empty")
        {
        }

    }

}