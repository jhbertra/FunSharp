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

    }

}