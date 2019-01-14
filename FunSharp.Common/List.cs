using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace FunSharp.Common
{

    [PublicAPI]
    public static class List
    {

        [NotNull]
        public static List<T> Cons<T>([NotNull] T value, List<T> tail) => List<T>.Cons(value, tail);

        [NotNull]
        public static List<T> Empty<T>() => List<T>.Empty;

    }


    [PublicAPI]
    public abstract class List<T> : StructuralEquality<List<T>>, IUnionType, IEnumerable<T>
    {

        [NotNull]
        public static List<T> Cons([NotNull] T value, List<T> tail) => new ConsList<T>(value, tail);

        [NotNull]
        public static List<T> Empty => new EmptyList<T>();

        protected List([NotNull] string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(tag));

            this.Tag = tag;
        }

        public string Tag { get; }

        public IEnumerator<T> GetEnumerator()
        {
            var list = this;
            while (list is ConsList<T> cons)
            {
                yield return cons.Head;
                list = cons.Tail;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public override string ToString()
        {
            return $"[{string.Join(", ", this.Select(x => x.ToString()))}]";
        }

        public bool IsEmpty => this is EmptyList<T>;

        protected override IEnumerable<(string FieldName, object FieldValue)> GetFields()
        {
            return this.Select((x, i) => ($"[{i}]", (object) x));
        }

    }


    [PublicAPI]
    public sealed class ConsList<T> : List<T>
    {

        public ConsList([NotNull] T head, [NotNull] List<T> tail) : base("Cons")
        {
            if (head == null)
            {
                throw new ArgumentNullException(nameof(head));
            }

            this.Head = head;
            this.Tail = tail ?? throw new ArgumentNullException(nameof(tail));
        }

        [NotNull] public readonly T Head;
        [NotNull] public readonly List<T> Tail;

    }


    [PublicAPI]
    public sealed class EmptyList<T> : List<T>
    {

        public EmptyList() : base("Empty")
        {
        }

    }

}