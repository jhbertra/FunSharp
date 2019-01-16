using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace FunSharp.Common.Test
{

    public static class OptionTests
    {

        [Test]
        public static void ArgumentExceptionTests()
        {
            var option = Option.None<string>();
            var invalid = new OptionInvalid<string>();

            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => Option.Some(default(string)));
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => default(Option<string>).Bind(x => Option.None<bool>()));
            Assert.Throws<ArgumentNullException>(() => option.Bind<string, bool>(null));
            Assert.Throws<ArgumentOutOfRangeException>(() => invalid.Bind(x => Option.None<bool>()));
            Assert.Throws<ArgumentNullException>(() => default(Option<string>).DefaultWith(""));
            Assert.Throws<ArgumentNullException>(() => option.DefaultWith(null));
            Assert.Throws<ArgumentOutOfRangeException>(() => invalid.DefaultWith(""));
            Assert.Throws<ArgumentNullException>(() => default(Option<string>).Filter(x => x.Length == 0));
            Assert.Throws<ArgumentNullException>(() => option.Filter(null));
            Assert.Throws<ArgumentOutOfRangeException>(() => invalid.Filter(x => x.Length == 0));
            Assert.Throws<ArgumentNullException>(() => default(Option<string>).Map(x => x));
            Assert.Throws<ArgumentNullException>(() => option.Map<string, bool>(null));
            Assert.Throws<ArgumentOutOfRangeException>(() => invalid.Map(x => x));
            Assert.Throws<ArgumentNullException>(() => default(Option<string>).Match(x => x, () => string.Empty));
            Assert.Throws<ArgumentNullException>(() => option.Match(null, () => string.Empty));
            Assert.Throws<ArgumentNullException>(() => option.Match(x => x, null));
            Assert.Throws<ArgumentOutOfRangeException>(() => invalid.Match(x => x, () => string.Empty));
            Assert.Throws<ArgumentNullException>(() => default(Option<string>).OfType<string, string>());
            Assert.Throws<ArgumentNullException>(() => option.SelectMany<string, int, bool>(null, (i, b) => true));
            Assert.Throws<ArgumentNullException>(() => option.SelectMany<string, int, bool>(x => Option.None<int>(), null));
            Assert.Throws<ArgumentNullException>(() => default(Option<string>).ToEitherLeft(new object()));
            Assert.Throws<ArgumentNullException>(() => option.ToEitherLeft(default(object)));
            Assert.Throws<ArgumentNullException>(() => default(Option<string>).ToEitherRight(new object()));
            Assert.Throws<ArgumentNullException>(() => option.ToEitherRight(default(object)));
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [TestCase(null, null, ExpectedResult = true, TestName = "Equals: None == None => true")]
        [TestCase("A", null, ExpectedResult = false, TestName = "Equals: Some A == None => false")]
        [TestCase(null, "B", ExpectedResult = false, TestName = "Equals: None == Some B => false")]
        [TestCase("A", "B", ExpectedResult = false, TestName = "Equals: Some A == Some B => false")]
        [TestCase("A", "A", ExpectedResult = true, TestName = "Equals: Some A == Some A => true")]
        public static bool EqualsTests(string leftVal, string rightVal)
        {
            var left = OptionTests.MakeOption(leftVal);
            var right = OptionTests.MakeOption(rightVal);

            return left.Equals(right);
        }


        [TestCase(null, ExpectedResult = null)]
        [TestCase("A", ExpectedResult = "A")]
        public static object GetEnumeratorTests(string val)
        {
            var enumerator = ((IEnumerable) OptionTests.MakeOption(val)).GetEnumerator();
            return enumerator.MoveNext() ? enumerator.Current : null;
        }


        [TestCase(null, ExpectedResult = true)]
        [TestCase("A", ExpectedResult = false)]
        public static bool IsEmptyTests(string val)
        {
            return OptionTests.MakeOption(val).IsEmpty;
        }

        [TestCase(null, null, ExpectedResult = "None")]
        [TestCase("A", null, ExpectedResult = "Some(A)")]
        [TestCase(null, "B", ExpectedResult = "Some(B)")]
        [TestCase("A", "B", ExpectedResult = "Some(A)")]
        public static string OrTests(string leftVal, string rightVal)
        {
            var left = OptionTests.MakeOption(leftVal);
            var right = OptionTests.MakeOption(rightVal);

            return (left || right).ToString();
        }


        [TestCase(null, ExpectedResult = "None")]
        [TestCase("A", ExpectedResult = "Some(A)")]
        public static string ToStringTests(string val)
        {
            return OptionTests.MakeOption(val).ToString();
        }


        [TestCase(null, false, ExpectedResult = "None")]
        [TestCase(null, true, ExpectedResult = "None")]
        [TestCase("Foo", false, ExpectedResult = "None")]
        [TestCase("Foo", true, ExpectedResult = "Some(3)")]
        public static string BindTests(string val, bool returnValue)
        {
            return OptionTests
                .MakeOption(val)
                .Pipe(Option.Bind<string, int>(x => returnValue ? Option.Some(x.Length) : Option.None<int>()))
                .ToString();
        }


        [TestCase(new bool[0], ExpectedResult = "None")]
        [TestCase(new[] { false, false }, ExpectedResult = "None")]
        [TestCase(new[] { false, true, false }, ExpectedResult = "Some(1)")]
        public static string ChooseTests(bool[] values)
        {
            return values
                .Select((x, i) => x ? Option.Some(i) : Option.None<int>())
                .ToArray()
                .Pipe(Option.Choose)
                .ToString();
        }


        [TestCase(null, ExpectedResult = "B")]
        [TestCase("A", ExpectedResult = "A")]
        public static string DefaultWithTests(string val)
        {
            return OptionTests
                .MakeOption(val)
                .Pipe(Option.DefaultWith("B"));
        }


        [TestCase(null, ExpectedResult = "None")]
        [TestCase("A", ExpectedResult = "None")]
        [TestCase("B", ExpectedResult = "Some(B)")]
        public static string FilterTests(string val)
        {
            return OptionTests
                .MakeOption(val)
                .Pipe(Option.Filter<string>(x => x == "B"))
                .ToString();
        }


        [TestCase(null, ExpectedResult = "None")]
        [TestCase("A", ExpectedResult = "Some(a)")]
        public static string MapTests(string val)
        {
            return OptionTests
                .MakeOption(val)
                .Pipe(Option.Map<string, string>(x => x.ToLower()))
                .ToString();
        }


        [TestCase(null, ExpectedResult = "Nothing")]
        [TestCase("A", ExpectedResult = "Just: A")]
        public static string MatchTests(string val)
        {
            return OptionTests
                .MakeOption(val)
                .Pipe(Option.Match<string, string>(
                    x => $"Just: {x}",
                    () => "Nothing"));
        }


        [TestCase(null, false, ExpectedResult = "None")]
        [TestCase(null, true, ExpectedResult = "None")]
        [TestCase("A", false, ExpectedResult = "None")]
        [TestCase("A", true, ExpectedResult = "Some(Some(A))")]
        public static string OfTypeTests(string val, bool wrapValue)
        {
            var innerOption = OptionTests.MakeOption(val);
            
            return (wrapValue ? innerOption.ToOption() : Option.None<Option<string>>())
                .Pipe(Option.OfType<Option<string>, OptionSome<string>>())
                .ToString();
        }


        [TestCase(null, ExpectedResult = "None")]
        [TestCase("Foo", ExpectedResult = "Some(3)")]
        public static string SelectTests(string val)
        {
            return (from x in OptionTests.MakeOption(val)
                    select x.Length)
                .ToString();
        }


        [TestCase(null, true, ExpectedResult = "None")]
        [TestCase(null, false, ExpectedResult = "None")]
        [TestCase("Foo", true, ExpectedResult = "None")]
        [TestCase("Foo", false, ExpectedResult = "Some(False)")]
        public static string SelectManyTests(string val, bool returnSome)
        {
            return (from x in OptionTests.MakeOption(val)
                    from y in returnSome
                        ? Option.None<bool>()
                        : (x.Length == 0).ToOption()
                    select y)
                .ToString();
        }


        [TestCase(null, 1, ExpectedResult = "Right(1)")]
        [TestCase("A", 1, ExpectedResult = "Left(A)")]
        public static string ToEitherLeftTests(string val, int rightValue)
        {
            return OptionTests
                .MakeOption(val)
                .ToEitherLeft(rightValue)
                .ToString();
        }


        [TestCase(null, 1, ExpectedResult = "Left(1)")]
        [TestCase("A", 1, ExpectedResult = "Right(A)")]
        public static string ToEitherRightTests(string val, int rightValue)
        {
            return OptionTests
                .MakeOption(val)
                .ToEitherRight(rightValue)
                .ToString();
        }


        [TestCase(null, ExpectedResult = "None")]
        [TestCase("A", ExpectedResult = "None")]
        [TestCase("B", ExpectedResult = "Some(1)")]
        public static string WhereTests(string val)
        {
            return (from x in OptionTests.MakeOption(val)
                    where x == "B"
                    select x.Length)
                .ToString();
        }
        
        private static Option<string> MakeOption(string val)
        {
            return (val is string a ? Option.Some(a) : Option.None<string>());
        }
        
        private sealed class OptionInvalid<T> : Option<T>
        {

            public OptionInvalid() : base("Invalid")
            {
            }


            protected override IEnumerable<(string FieldName, object FieldValue)> GetFields()
            {
                throw new NotImplementedException();
            }

        }

    }

}