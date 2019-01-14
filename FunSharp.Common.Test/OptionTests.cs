using NUnit.Framework;

namespace FunSharp.Common.Test
{

    public static class OptionTests
    {

        [TestCase(null, null, ExpectedResult = true, TestName = "Equals: None == None => true")]
        [TestCase("A", null, ExpectedResult = false, TestName = "Equals: Some A == None => false")]
        [TestCase(null, "B", ExpectedResult = false, TestName = "Equals: None == Some B => false")]
        [TestCase("A", "B", ExpectedResult = false, TestName = "Equals: Some A == Some B => false")]
        [TestCase("A", "A", ExpectedResult = true, TestName = "Equals: Some A == Some A => true")]
        public static bool EqualsTests(string leftVal, string rightVal)
        {
            var left = leftVal is string a ? Option.Some(a) : Option<string>.None;
            var right = rightVal is string b ? Option.Some(b) : Option<string>.None;

            return left.Equals(right);
        }

        [TestCase(null, ExpectedResult = "None")]
        [TestCase("A", ExpectedResult = "Some(A)")]
        public static string ToStringTests(string val)
        {
            return (val is string a ? Option.Some(a) : Option<string>.None).ToString();
        }

    }

}