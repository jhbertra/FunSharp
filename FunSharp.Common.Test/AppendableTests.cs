using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace FunSharp.Common.Test
{

    public static class AppendableTests
    {

        [Test]
        public static void InvalidArgumentsTest()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => Appendable.Concat(null, new IntAppendable(0)));
            Assert.Throws<ArgumentNullException>(() => Appendable.Concat(new IntAppendable(0), default(IntAppendable[])));
            Assert.Throws<ArgumentNullException>(() => Appendable.Concat(new IntAppendable(0), default(IEnumerable<IntAppendable>)));
            Assert.Throws<ArgumentNullException>(() => default(IntAppendable).Repeat(new IntAppendable(0), 0));
            Assert.Throws<ArgumentNullException>(() => new IntAppendable(0).Repeat(null, 0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new IntAppendable(0).Repeat(new IntAppendable(0), -1));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [TestCase(new int[0], ExpectedResult = 0)]
        [TestCase(new[] {1}, ExpectedResult = 1)]
        [TestCase(new[] {1, 2, 3}, ExpectedResult = 6)]
        public static int ConcatTests(int[] values)
        {
            return Appendable
                .Concat(
                    new IntAppendable(0),
                    values.Select(x => new IntAppendable(x)).ToArray())
                .Value;
        }

        [TestCase(0, ExpectedResult = 0)]
        [TestCase(1, ExpectedResult = 5)]
        [TestCase(5, ExpectedResult = 25)]
        public static int RepeatTests(int repeats)
        {
            return Appendable
                .Repeat(new IntAppendable(0), repeats)
                .Invoke(new IntAppendable(5))
                .Value;
        }

        private sealed class IntAppendable : IAppendable<IntAppendable>
        {

            public IntAppendable(int value)
            {
                this.Value = value;
            }

            public readonly int Value;

            public IntAppendable Append(IntAppendable t)
            {
                return new IntAppendable(this.Value + t.Value);
            }

        }

    }

}