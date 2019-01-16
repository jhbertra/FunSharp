using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace FunSharp.Common.Test
{

    public static class ListTests
    {

        [Test]
        public static void ArgumentExceptionTests()
        {
            var list = List.Empty<string>();
            var invalid = new ListInvalid<string>();

            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => List.Cons(default(string), list));
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => default(List<string>).Bind(x => List.Empty<bool>()));
            Assert.Throws<ArgumentNullException>(() => list.Bind<string, bool>(null));
            Assert.Throws<ArgumentNullException>(() => default(List<string>).Filter(x => x.Length == 0));
            Assert.Throws<ArgumentNullException>(() => list.Filter(null));
            Assert.Throws<ArgumentNullException>(() => default(List<string>).Map(x => x));
            Assert.Throws<ArgumentNullException>(() => list.Map<string, bool>(null));
            Assert.Throws<ArgumentNullException>(() => default(List<string>).Match((x, xs) => x, () => string.Empty));
            Assert.Throws<ArgumentNullException>(() => list.Match(null, () => string.Empty));
            Assert.Throws<ArgumentNullException>(() => list.Match((x, xs) => x, null));
            Assert.Throws<ArgumentOutOfRangeException>(() => invalid.Match((x, xs) => x, () => string.Empty));
            Assert.Throws<ArgumentNullException>(() => default(List<string>).OfType<string, string>());
            Assert.Throws<ArgumentNullException>(() => list.SelectMany<string, int, bool>(null, (i, b) => true));
            Assert.Throws<ArgumentNullException>(() => list.SelectMany<string, int, bool>(x => x.Length.ToList(), null));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [TestCase(new int[0], new int[0], ExpectedResult = "[]")]
        [TestCase(new[] {1}, new int[0], ExpectedResult = "[1]")]
        [TestCase(new int[0], new[] {1}, ExpectedResult = "[1]")]
        [TestCase(new[] {1}, new[] {2}, ExpectedResult = "[1, 2]")]
        [TestCase(new[] {1, 2}, new int[0], ExpectedResult = "[1, 2]")]
        [TestCase(new int[0], new[] {1, 2}, ExpectedResult = "[1, 2]")]
        [TestCase(new[] {1, 2}, new[] {3}, ExpectedResult = "[1, 2, 3]")]
        [TestCase(new[] {0}, new[] {1, 2}, ExpectedResult = "[0, 1, 2]")]
        [TestCase(new[] {1}, new[] {2}, ExpectedResult = "[1, 2]")]
        [TestCase(new[] {1, 2}, new[] {3, 4}, ExpectedResult = "[1, 2, 3, 4]")]
        public static string AppendTests(int[] values1, int[] values2)
        {
            var list1 = values1
                .Reverse()
                .Aggregate(List.Empty<int>(), (xs, x) => List.Cons(x, xs));
            
            var list2 = values2
                .Reverse()
                .Aggregate(List.Empty<int>(), (xs, x) => List.Cons(x, xs));

            return list1.Append(list2).ToString();
        }

        [TestCase(new int[0], new int[0], ExpectedResult = true, TestName = "Equals: [] == [] => true")]
        [TestCase(new[] {1}, new int[0], ExpectedResult = false, TestName = "Equals: [A] == [] => false")]
        [TestCase(new int[0], new[] {2}, ExpectedResult = false, TestName = "Equals: [] == [B] => false")]
        [TestCase(new[] {1}, new[] {2}, ExpectedResult = false, TestName = "Equals: [A] == [B] => false")]
        [TestCase(new[] {1}, new[] {1}, ExpectedResult = true, TestName = "Equals: [A] == [A] => true")]
        [TestCase(new[] {1, 2}, new[] {1}, ExpectedResult = false, TestName = "Equals: [A, B] == [A] => false")]
        [TestCase(new[] {1, 2}, new[] {2, 1}, ExpectedResult = false, TestName = "Equals: [A, B] == [B, A] => false")]
        [TestCase(new[] {1, 2}, new[] {1, 2}, ExpectedResult = true, TestName = "Equals: [A, B] == [A, B] => true")]
        public static bool EqualsTests(int[] values1, int[] values2)
        {
            var list1 = values1.Reverse().Aggregate(List.Empty<int>(), (xs, x) => List.Cons(x, xs));
            var list2 = values2.Reverse().Aggregate(List.Empty<int>(), (xs, x) => List.Cons(x, xs));

            return list1.Equals(list2);
        }


        [TestCase(new[] {1}, ExpectedResult = false)]
        [TestCase(new int[0], ExpectedResult = true)]
        public static bool IsEmptyTests(int[] values)
        {
            return values
                .Reverse()
                .Aggregate(List.Empty<int>(), (xs, x) => List.Cons(x, xs))
                .IsEmpty;
        }


        [TestCase(new int[0], ExpectedResult = "[]")]
        [TestCase(new[] {1}, ExpectedResult = "[1]")]
        [TestCase(new[] {1, 2}, ExpectedResult = "[1, 2]")]
        public static string ToStringTests(int[] values)
        {
            return values
                .Reverse()
                .Aggregate(List.Empty<int>(), (xs, x) => List.Cons(x, xs))
                .ToString();
        }


        [TestCase(new int[0], ExpectedResult = "Empty")]
        [TestCase(new[] {1}, ExpectedResult = "Cons")]
        public static string TagTests(int[] values)
        {
            return values
                .Reverse()
                .Aggregate(List.Empty<int>(), (xs, x) => List.Cons(x, xs))
                .Tag;
        }


        [TestCase(new int[0], ExpectedResult = false)]
        [TestCase(new[] {1}, ExpectedResult = true)]
        public static bool GetEnumeratorTests(int[] values)
        {
            return ((IEnumerable) values
                .Reverse()
                .Aggregate(List.Empty<int>(), (xs, x) => List.Cons(x, xs)))
                .GetEnumerator()
                .MoveNext();
        }


        [TestCase(new int[0], ExpectedResult = "[]")]
        [TestCase(new[] {1}, ExpectedResult = "['1']")]
        [TestCase(new[] {1, 2, 3}, ExpectedResult = "['1', '2', '2', '3', '3', '3']")]
        public static string BindTests(int[] values)
        {
            return values
                .Reverse()
                .Aggregate(List.Empty<int>(), (xs, x) => List.Cons(x, xs))
                .Pipe(
                    List.Bind<int, string>(
                        x => x
                            .ToString()
                            .Pipe(y => $"'{x}'")
                            .ToList()
                            .Repeat(List<string>.Empty, x)))
                .ToString();
        }


        [TestCase(new int[0], ExpectedResult = "[]")]
        [TestCase(new[] {1}, ExpectedResult = "[]")]
        [TestCase(new[] {1, 2, 3}, ExpectedResult = "[2]")]
        public static string FilterTests(int[] values)
        {
            return values
                .Reverse()
                .Aggregate(List.Empty<int>(), (xs, x) => List.Cons(x, xs))
                .Pipe(List.Filter<int>(x => x == 2))
                .ToString();
        }


        [TestCase(new int[0], ExpectedResult = "[]")]
        [TestCase(new[] {1}, ExpectedResult = "[2]")]
        [TestCase(new[] {1, 2, 3}, ExpectedResult = "[2, 4, 6]")]
        public static string MapTests(int[] values)
        {
            return values
                .Reverse()
                .Aggregate(List.Empty<int>(), (xs, x) => List.Cons(x, xs))
                .Pipe(List.Map<int, int>(x => x * 2))
                .ToString();
        }


        [TestCase(new int[0], ExpectedResult = "Nothing")]
        [TestCase(new[] {1}, ExpectedResult = "Head: 1, Tail: []")]
        [TestCase(new[] {1, 2, 3}, ExpectedResult = "Head: 1, Tail: [2, 3]")]
        public static string MatchTests(int[] values)
        {
            return values
                .Reverse()
                .Aggregate(List.Empty<int>(), (xs, x) => List.Cons(x, xs))
                .Pipe(List.Match<int, string>(
                    (x, xs) => $"Head: {x}, Tail: {xs}",
                    () => "Nothing"));
        }


        [TestCase(new bool[0], ExpectedResult = "[]")]
        [TestCase(new[] {false}, ExpectedResult = "[]")]
        [TestCase(new[] {false, true, false}, ExpectedResult = "[Some(Foo)]")]
        public static string OfTypeTests(bool[] values)
        {
            return values
                .Reverse()
                .Select(x => Option.Guard(x).Bind(_ => Option.Some("Foo")))
                .Aggregate(List.Empty<Option<string>>(), (xs, x) => List.Cons(x, xs))
                .Pipe(List.OfType<Option<string>, OptionSome<string>>())
                .ToString();
        }


        [TestCase(new int[0], ExpectedResult = "[]")]
        [TestCase(new[] {1}, ExpectedResult = "[2]")]
        [TestCase(new[] {1, 2, 3}, ExpectedResult = "[2, 4, 6]")]
        public static string SelectTests(int[] values)
        {
            return (from x in values
                        .Reverse()
                        .Aggregate(List.Empty<int>(), (xs, x) => List.Cons(x, xs))
                    select x * 2)
                .ToString();
        }


        [TestCase(new int[0], ExpectedResult = "[]")]
        [TestCase(new[] {1}, ExpectedResult = "['1']")]
        [TestCase(new[] {1, 2, 3}, ExpectedResult = "['1', '2', '2', '3', '3', '3']")]
        public static string SelectManyTests(int[] values)
        {
            return (from x in values
                        .Reverse()
                        .Aggregate(List.Empty<int>(), (xs, x) => List.Cons(x, xs))
                    from y in x
                        .ToString()
                        .Pipe(y => $"'{x}'")
                        .ToList()
                        .Repeat(List<string>.Empty, x)
                    select y)
                .ToString();
        }


        [TestCase(new int[0], ExpectedResult = "[]")]
        [TestCase(new[] {1}, ExpectedResult = "[]")]
        [TestCase(new[] {1, 2, 3}, ExpectedResult = "[2]")]
        public static string WhereTests(int[] values)
        {
            return (from x in values
                .Reverse()
                .Aggregate(List.Empty<int>(), (xs, x) => List.Cons(x, xs))
                    where x == 2
                    select x)
                .ToString();
        }
        
        private sealed class ListInvalid<T> : List<T>
        {

            public ListInvalid() : base("Invalid")
            {
            }

            protected override IEnumerable<(string FieldName, object FieldValue)> GetFields()
            {
                throw new NotImplementedException();
            }

        }

    }

}