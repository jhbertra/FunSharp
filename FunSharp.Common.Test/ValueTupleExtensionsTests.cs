using System;
using NUnit.Framework;

namespace FunSharp.Common.Test
{

    public static class ValueTupleExtensionsTests
    {

        [Test]
        public static void ArgumentExceptionTests()
        {
            var t = (1, 1);
            // ReSharper disable AssignNullToNotNullAttribute
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => t.BiMap<int, string, int, string>(null, x => x.ToString()));
            Assert.Throws<ArgumentNullException>(() => t.BiMap<int, string, int, string>(x => x.ToString(), null));
            Assert.Throws<ArgumentNullException>(() => t.BindLeft<int, int, int>(null));
            Assert.Throws<ArgumentNullException>(() => t.BindRight<int, int, int>(null));
            Assert.Throws<ArgumentNullException>(() => t.MapLeft<int, int, int>(null));
            Assert.Throws<ArgumentNullException>(() => t.MapRight<int, int, int>(null));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            // ReSharper restore AssignNullToNotNullAttribute
        }


        [TestCase("Foo", "Bar", ExpectedResult = "(3, 6)")]
        public static string BiMapTests(string left, string right)
        {
            return (left, right)
                .BiMap(x => x.Length, y => y.Length * 2)
                .ToString();
        }


        [TestCase("Foo", "Bar", ExpectedResult = "(3, Foo)")]
        public static string BindLeftTests(string left, string right)
        {
            return (left, right)
                .BindLeft(x => (x.Length, x))
                .ToString();
        }


        [TestCase("Foo", "Bar", ExpectedResult = "(Bar, 3)")]
        public static string BindRightTests(string left, string right)
        {
            return (left, right)
                .BindRight(x => (x, x.Length))
                .ToString();
        }


        [TestCase("Foo", "Bar", ExpectedResult = "(Bar, Foo)")]
        public static string FlipTests(string left, string right)
        {
            return (left, right).Flip().ToString();
        }


        [TestCase("Foo", "Bar", ExpectedResult = "(3, Bar)")]
        public static string MapLeftTests(string left, string right)
        {
            return (left, right)
                .MapLeft(x => x.Length)
                .ToString();
        }


        [TestCase("Foo", "Bar", ExpectedResult = "(Foo, 3)")]
        public static string MapRightTests(string left, string right)
        {
            return (left, right)
                .MapRight(x => x.Length)
                .ToString();
        }

    }

}