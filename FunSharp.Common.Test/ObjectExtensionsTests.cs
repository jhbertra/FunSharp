using System.Linq;
using NUnit.Framework;

namespace FunSharp.Common.Test
{

    public static class ObjectExtensionsTests
    {

        [TestCase("3", false, ExpectedResult = 3)]
        [TestCase("3", true, ExpectedResult = -1)]
        [TestCase(null, false, ExpectedResult = -1)]
        public static int PipeTests(string input, bool nullFunction)
        {
            try
            {
                // ReSharper disable AssignNullToNotNullAttribute
                return nullFunction
                    ? input.Pipe<string, int>(null)
                    : input.Pipe(int.Parse);
                // ReSharper restore AssignNullToNotNullAttribute
            }
            catch
            {
                return -1;
            }
        }

        [TestCase("Foo", ExpectedResult = "Left(Foo)")]
        [TestCase(null, ExpectedResult = "")]
        public static string ToEitherLeftTests(string input)
        {
            try
            {
                // ReSharper disable AssignNullToNotNullAttribute
                return input.ToEitherLeft(new TypeHint<int>()).ToString();
                // ReSharper restore AssignNullToNotNullAttribute
            }
            catch
            {
                return "";
            }
        }

        [TestCase("Foo", ExpectedResult = "Right(Foo)")]
        [TestCase(null, ExpectedResult = "")]
        public static string ToEitherRightTests(string input)
        {
            try
            {
                // ReSharper disable AssignNullToNotNullAttribute
                return input.ToEitherRight(new TypeHint<int>()).ToString();
                // ReSharper restore AssignNullToNotNullAttribute
            }
            catch
            {
                return "";
            }
        }

        [TestCase("Foo", ExpectedResult = "[Foo]")]
        [TestCase(null, ExpectedResult = "")]
        public static string ToEnumerableTests(string input)
        {
            try
            {
                // ReSharper disable AssignNullToNotNullAttribute
                return input.ToEnumerable().Pipe(x => $"[{x.Single()}]");
                // ReSharper restore AssignNullToNotNullAttribute
            }
            catch
            {
                return "";
            }
        }

        [TestCase("Foo", ExpectedResult = "[Foo]")]
        [TestCase(null, ExpectedResult = "")]
        public static string ToListTests(string input)
        {
            try
            {
                // ReSharper disable AssignNullToNotNullAttribute
                return input.ToList().ToString();
                // ReSharper restore AssignNullToNotNullAttribute
            }
            catch
            {
                return "";
            }
        }

        [TestCase("Foo", ExpectedResult = "Some(Foo)")]
        [TestCase(null, ExpectedResult = "")]
        public static string ToOptionTests(string input)
        {
            try
            {
                // ReSharper disable AssignNullToNotNullAttribute
                return input.ToOption().ToString();
                // ReSharper restore AssignNullToNotNullAttribute
            }
            catch
            {
                return "";
            }
        }

        [TestCase("Foo", ExpectedResult = "Set(Foo)")]
        [TestCase(null, ExpectedResult = "")]
        public static string ToUpdateTests(string input)
        {
            try
            {
                // ReSharper disable AssignNullToNotNullAttribute
                return input.ToUpdate().ToString();
                // ReSharper restore AssignNullToNotNullAttribute
            }
            catch
            {
                return "";
            }
        }

    }

}