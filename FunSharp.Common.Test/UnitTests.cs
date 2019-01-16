using NUnit.Framework;

namespace FunSharp.Common.Test
{

    public class UnitTests
    {

        [Test]
        public static void Append_ReturnsUnit()
        {
            Assert.AreEqual(default(Unit), default(Unit).Append(default));
        }

        [Test]
        public static void CompareTo_Returns0()
        {
            Assert.AreEqual(0, default(Unit).CompareTo(default));
        }

        [Test]
        public static void Equals_ReturnsTrueForSameType()
        {
            Assert.IsTrue(default(Unit).Equals(default));
            Assert.IsFalse(default(Unit).Equals(null));
        }

        [Test]
        public static void GetHashCode_ReturnsConsistentCode()
        {
            Assert.AreEqual(default(Unit).GetHashCode(), default(Unit).GetHashCode());
        }

        [Test]
        public static void op_Addition_ReturnsUnit()
        {
            Assert.AreEqual(default(Unit), default(Unit) + default(Unit));
        }

        [Test]
        public static void op_Equality_ReturnsTrue()
        {
            Assert.IsTrue(default(Unit) == default);
        }

        [Test]
        public static void op_Inequality_ReturnsFalse()
        {
            Assert.IsFalse(default(Unit) != default);
        }

        [Test]
        public static void op_GreaterThan_ReturnsFalse()
        {
            // ReSharper disable once EqualExpressionComparison
            Assert.IsFalse(default(Unit) > default(Unit));
        }

        [Test]
        public static void op_GreaterThanOrEqual_ReturnsTrue()
        {
            // ReSharper disable once EqualExpressionComparison
            Assert.IsTrue(default(Unit) >= default(Unit));
        }

        [Test]
        public static void op_LessThan_ReturnsFalse()
        {
            // ReSharper disable once EqualExpressionComparison
            Assert.IsFalse(default(Unit) < default(Unit));
        }

        [Test]
        public static void op_LessThanOrEqual_ReturnsTrue()
        {
            // ReSharper disable once EqualExpressionComparison
            Assert.IsTrue(default(Unit) <= default(Unit));
        }

    }

}