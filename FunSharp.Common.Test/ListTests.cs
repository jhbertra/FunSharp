using System.Linq;
using NUnit.Framework;

namespace FunSharp.Common.Test
{

    public static class ListTests
    {

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
            var list1 = values1.Reverse().Aggregate(List<int>.Empty, (xs, x) => List.Cons(x, xs));
            var list2 = values2.Reverse().Aggregate(List<int>.Empty, (xs, x) => List.Cons(x, xs));

            return list1.Equals(list2);
        }


        [TestCase(new int[0], ExpectedResult = "[]")]
        [TestCase(new[] {1}, ExpectedResult = "[1]")]
        [TestCase(new[] {1, 2}, ExpectedResult = "[1, 2]")]
        public static string ToStringTests(int[] values)
        {
            return values
                .Reverse()
                .Aggregate(List<int>.Empty, (xs, x) => List.Cons(x, xs))
                .ToString();
        }

    }

}