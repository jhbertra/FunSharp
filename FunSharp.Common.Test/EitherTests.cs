using NUnit.Framework;

namespace FunSharp.Common.Test
{

    public static class EitherTests
    {

        [TestCase(1, 2, ExpectedResult = false, TestName = "Equals: Left A == Left B => false")]
        [TestCase(1, 1, ExpectedResult = true, TestName = "Equals: Left A == Left A => true")]
        [TestCase("A", 2, ExpectedResult = false, TestName = "Equals: Right A == Left B => false")]
        [TestCase(1, "B", ExpectedResult = false, TestName = "Equals: Left A == Right B => false")]
        [TestCase("A", "B", ExpectedResult = false, TestName = "Equals: Right A == Right B => false")]
        [TestCase("A", "A", ExpectedResult = true, TestName = "Equals: Right A == Right A => true")]
        public static bool EqualsTests(object leftVal, object rightVal)
        {
            var left = leftVal is int a ? Either<int, string>.Left(a) : Either<int, string>.Right((string) leftVal);
            var right = rightVal is int b ? Either<int, string>.Left(b) : Either<int, string>.Right((string) rightVal);

            return left.Equals(right);
        }


        [TestCase(1, ExpectedResult = "Left(1)")]
        [TestCase("A", ExpectedResult = "Right(A)")]
        public static string ToStringTests(object val)
        {
            return (val is int a
                    ? Either<int, string>.Left(a)
                    : Either<int, string>.Right((string) val))
                .ToString();
        }


        [TestCase(1, ExpectedResult = true)]
        [TestCase("A", ExpectedResult = false)]
        public static bool IsLeftTests(object val)
        {
            return (val is int a
                    ? Either<int, string>.Left(a)
                    : Either<int, string>.Right((string) val))
                .IsLeft;
        }


        [TestCase(1, ExpectedResult = false)]
        [TestCase("A", ExpectedResult = true)]
        public static bool IsRightTests(object val)
        {
            return (val is int a
                    ? Either<int, string>.Left(a)
                    : Either<int, string>.Right((string) val))
                .IsRight;
        }


        [TestCase(1, ExpectedResult = "Left(False)")]
        [TestCase("Foo", ExpectedResult = "Right(3)")]
        public static string BiMapTests(object val)
        {
            return (val is int a
                    ? Either<int, string>.Left(a)
                    : Either<int, string>.Right((string) val))
                .Pipe(Either.BiMap<int, bool, string, int>(x => x % 2 == 0, x => x.Length))
                .ToString();
        }


        [TestCase(1, true, ExpectedResult = "Left(False)")]
        [TestCase(1, false, ExpectedResult = "Right(Odd)")]
        [TestCase("Foo", true, ExpectedResult = "Right(Foo)")]
        [TestCase("Foo", false, ExpectedResult = "Right(Foo)")]
        public static string BindLeftTests(object val, bool returnLeft)
        {
            return (val is int a
                    ? Either<int, string>.Left(a)
                    : Either<int, string>.Right((string) val))
                .Pipe(Either.BindLeft<int, bool, string>(x => returnLeft
                    ? Either.Left(x % 2 == 0, new TypeHint<string>())
                    : Either.Right(x % 2 == 0 ? "Even" : "Odd", new TypeHint<bool>())))
                .ToString();
        }


        [TestCase(1, true, ExpectedResult = "Left(1)")]
        [TestCase(1, false, ExpectedResult = "Left(1)")]
        [TestCase("Foo", true, ExpectedResult = "Left(3)")]
        [TestCase("Foo", false, ExpectedResult = "Right(False)")]
        public static string BindRightTests(object val, bool returnLeft)
        {
            return (val is int a
                    ? Either<int, string>.Left(a)
                    : Either<int, string>.Right((string) val))
                .Pipe(Either.BindRight<int, string, bool>(x => returnLeft
                    ? Either.Left(x.Length, new TypeHint<bool>())
                    : Either.Right(x.Length == 0, new TypeHint<int>())))
                .ToString();
        }


        [TestCase(1, 2, ExpectedResult = 1)]
        [TestCase("Foo", 2, ExpectedResult = 2)]
        public static int DefaultLeftWithTests(object val, int defaultVal)
        {
            return (val is int a
                    ? Either<int, string>.Left(a)
                    : Either<int, string>.Right((string) val))
                .Pipe(Either.DefaultLeftWith<int, string>(defaultVal));
        }


        [TestCase(1, "Bar", ExpectedResult = "Bar")]
        [TestCase("Foo", "Bar", ExpectedResult = "Foo")]
        public static string DefaultRightWithTests(object val, string defaultVal)
        {
            return (val is int a
                    ? Either<int, string>.Left(a)
                    : Either<int, string>.Right((string) val))
                .Pipe(Either.DefaultRightWith<int, string>(defaultVal));
        }


        [TestCase(1, ExpectedResult = "Right(1)")]
        [TestCase("Foo", ExpectedResult = "Left(Foo)")]
        public static string FlipTests(object val)
        {
            return (val is int a
                    ? Either<int, string>.Left(a)
                    : Either<int, string>.Right((string) val))
                .Flip()
                .ToString();
        }


        [TestCase(1, ExpectedResult = "Left(False)")]
        [TestCase("Foo", ExpectedResult = "Right(Foo)")]
        public static string MapLeftTests(object val)
        {
            return (val is int a
                    ? Either<int, string>.Left(a)
                    : Either<int, string>.Right((string) val))
                .Pipe(Either.MapLeft<int, bool, string>(x => x % 2 == 0))
                .ToString();
        }


        [TestCase(1, ExpectedResult = "Left(1)")]
        [TestCase("Foo", ExpectedResult = "Right(3)")]
        public static string MapRightTests(object val)
        {
            return (val is int a
                    ? Either<int, string>.Left(a)
                    : Either<int, string>.Right((string) val))
                .Pipe(Either.MapRight<int, string, int>(x => x.Length))
                .ToString();
        }


        [TestCase(1, ExpectedResult = false)]
        [TestCase("Foo", ExpectedResult = true)]
        public static bool MatchTests(object val)
        {
            return (val is int a
                    ? Either<int, string>.Left(a)
                    : Either<int, string>.Right((string) val))
                .Pipe(Either.Match<int, string, bool>(x => x % 2 == 0, x => x.Length > 0));
        }


        [TestCase(1, ExpectedResult = "Some(1)")]
        [TestCase("Foo", ExpectedResult = "None")]
        public static string ToLeftOptionTests(object val)
        {
            return (val is int a
                    ? Either<int, string>.Left(a)
                    : Either<int, string>.Right((string) val))
                .ToLeftOption()
                .ToString();
        }


        [TestCase(1, ExpectedResult = "None")]
        [TestCase("Foo", ExpectedResult = "Some(Foo)")]
        public static string ToRightOptionTests(object val)
        {
            return (val is int a
                    ? Either<int, string>.Left(a)
                    : Either<int, string>.Right((string) val))
                .ToRightOption()
                .ToString();
        }


        [TestCase(1, ExpectedResult = "[1]")]
        [TestCase("Foo", ExpectedResult = "[]")]
        public static string ToLeftEnumerableTests(object val)
        {
            return (val is int a
                    ? Either<int, string>.Left(a)
                    : Either<int, string>.Right((string) val))
                .ToLeftEnumerable()
                .Pipe(x => $"[{string.Join(", ", x)}]");
        }


        [TestCase(1, ExpectedResult = "[]")]
        [TestCase("Foo", ExpectedResult = "[Foo]")]
        public static string ToRightEnumerableTests(object val)
        {
            return (val is int a
                    ? Either<int, string>.Left(a)
                    : Either<int, string>.Right((string) val))
                .ToRightEnumerable()
                .Pipe(x => $"[{string.Join(", ", x)}]");
        }

    }

}