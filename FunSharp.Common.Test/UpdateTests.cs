using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace FunSharp.Common.Test
{

    public static class UpdateTests
    {

        [Test]
        public static void ArgumentExceptionTests()
        {
            var option = Update.Ignore<string>();
            var invalid = new UpdateInvalid<string>();

            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => Update.Set(default(string)));
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Assert.Throws<ArgumentNullException>(() => default(Update<string>).Bind(x => Update.Ignore<bool>()));
            Assert.Throws<ArgumentNullException>(() => option.Bind<string, bool>(null));
            Assert.Throws<ArgumentOutOfRangeException>(() => invalid.Bind(x => Update.Ignore<bool>()));
            Assert.Throws<ArgumentNullException>(() => default(Update<string>).DefaultWith(""));
            Assert.Throws<ArgumentNullException>(() => option.DefaultWith(null));
            Assert.Throws<ArgumentOutOfRangeException>(() => invalid.DefaultWith(""));
            Assert.Throws<ArgumentNullException>(() => default(Update<string>).Filter(x => x.Length == 0));
            Assert.Throws<ArgumentNullException>(() => option.Filter(null));
            Assert.Throws<ArgumentOutOfRangeException>(() => invalid.Filter(x => x.Length == 0));
            Assert.Throws<ArgumentNullException>(() => default(Update<string>).Map(x => x));
            Assert.Throws<ArgumentNullException>(() => option.Map<string, bool>(null));
            Assert.Throws<ArgumentOutOfRangeException>(() => invalid.Map(x => x));
            Assert.Throws<ArgumentNullException>(() => default(Update<string>).Match(x => x, () => string.Empty, () => string.Empty));
            Assert.Throws<ArgumentNullException>(() => option.Match(null, () => string.Empty, () => string.Empty));
            Assert.Throws<ArgumentNullException>(() => option.Match(x => x, null, () => string.Empty));
            Assert.Throws<ArgumentNullException>(() => option.Match(x => x, () => string.Empty, null));
            Assert.Throws<ArgumentOutOfRangeException>(() => invalid.Match(x => x, () => string.Empty, () => string.Empty));
            Assert.Throws<ArgumentNullException>(() => default(Update<string>).OfType<string, string>());
            Assert.Throws<ArgumentNullException>(() => default(Update<string>).Resolve(Option<string>.None));
            Assert.Throws<ArgumentNullException>(() => option.Resolve(null));
            Assert.Throws<ArgumentOutOfRangeException>(() => invalid.Resolve(Option<string>.None));
            Assert.Throws<ArgumentNullException>(() => option.SelectMany<string, int, bool>(null, (i, b) => true));
            Assert.Throws<ArgumentNullException>(() => option.SelectMany<string, int, bool>(x => Update.Ignore<int>(), null));
            Assert.Throws<ArgumentNullException>(() => default(Update<string>).ToEitherLeft(new object()));
            Assert.Throws<ArgumentNullException>(() => option.ToEitherLeft(default(object)));
            Assert.Throws<ArgumentNullException>(() => default(Update<string>).ToEitherRight(new object()));
            Assert.Throws<ArgumentNullException>(() => option.ToEitherRight(default(object)));
            Assert.Throws<ArgumentNullException>(() => default(Update<string>).ToOption());
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [TestCase(null, null, ExpectedResult = true, TestName = "Equals: Ignore == Ignore => true")]
        [TestCase(null, "", ExpectedResult = false, TestName = "Equals: Ignore == Clear => false")]
        [TestCase("", null, ExpectedResult = false, TestName = "Equals: Clear == Ignore => false")]
        [TestCase("", "", ExpectedResult = true, TestName = "Equals: Clear == Clear => true")]
        [TestCase("A", null, ExpectedResult = false, TestName = "Equals: Set A == Ignore => false")]
        [TestCase("A", "", ExpectedResult = false, TestName = "Equals: Set A == Clear => false")]
        [TestCase(null, "B", ExpectedResult = false, TestName = "Equals: Ignore == Set B => false")]
        [TestCase("", "B", ExpectedResult = false, TestName = "Equals: Clear == Set B => false")]
        [TestCase("A", "B", ExpectedResult = false, TestName = "Equals: Set A == Set B => false")]
        [TestCase("A", "A", ExpectedResult = true, TestName = "Equals: Set A == Set A => true")]
        public static bool EqualsTests(string leftVal, string rightVal)
        {
            var left = UpdateTests.MakeUpdate(leftVal);
            var right = UpdateTests.MakeUpdate(rightVal);

            return left.Equals(right);
        }


        [TestCase(null, ExpectedResult = null)]
        [TestCase("A", ExpectedResult = "A")]
        public static object GetEnumeratorTests(string val)
        {
            var enumerator = ((IEnumerable) UpdateTests.MakeUpdate(val)).GetEnumerator();
            return enumerator.MoveNext() ? enumerator.Current : null;
        }


        [TestCase(false, ExpectedResult = "Ignore")]
        [TestCase(true, ExpectedResult = "Set(())")]
        public static string GuardTests(bool condition)
        {
            return Update.Guard(condition).ToString();
        }


        [TestCase(null, ExpectedResult = true)]
        [TestCase("A", ExpectedResult = false)]
        public static bool IsEmptyTests(string val)
        {
            return UpdateTests.MakeUpdate(val).IsEmpty;
        }

        [TestCase(null, null, ExpectedResult = "Ignore")]
        [TestCase("A", null, ExpectedResult = "Set(A)")]
        [TestCase(null, "B", ExpectedResult = "Set(B)")]
        [TestCase("A", "B", ExpectedResult = "Set(A)")]
        public static string OrTests(string leftVal, string rightVal)
        {
            var left = UpdateTests.MakeUpdate(leftVal);
            var right = UpdateTests.MakeUpdate(rightVal);

            return (left || right).ToString();
        }


        [TestCase(null, ExpectedResult = "Ignore")]
        [TestCase("A", ExpectedResult = "Set(A)")]
        public static string ToStringTests(string val)
        {
            return UpdateTests.MakeUpdate(val).ToString();
        }


        [TestCase(null, false, ExpectedResult = "Ignore")]
        [TestCase(null, true, ExpectedResult = "Ignore")]
        [TestCase("", false, ExpectedResult = "Clear")]
        [TestCase("", true, ExpectedResult = "Clear")]
        [TestCase("Foo", false, ExpectedResult = "Ignore")]
        [TestCase("Foo", true, ExpectedResult = "Set(3)")]
        public static string BindTests(string val, bool returnValue)
        {
            return UpdateTests
                .MakeUpdate(val)
                .Pipe(Update.Bind<string, int>(x => returnValue ? Update.Set(x.Length) : Update.Ignore<int>()))
                .ToString();
        }


        [TestCase(new bool[0], ExpectedResult = "Ignore")]
        [TestCase(new[] { false, false }, ExpectedResult = "Ignore")]
        [TestCase(new[] { false, true, false }, ExpectedResult = "Set(1)")]
        public static string ChooseTests(bool[] values)
        {
            return values
                .Select((x, i) => x ? Update.Set(i) : Update.Ignore<int>())
                .ToArray()
                .Pipe(Update.Choose)
                .ToString();
        }


        [TestCase(null, ExpectedResult = "B")]
        [TestCase("", ExpectedResult = "B")]
        [TestCase("A", ExpectedResult = "A")]
        public static string DefaultWithTests(string val)
        {
            return UpdateTests
                .MakeUpdate(val)
                .Pipe(Update.DefaultWith("B"));
        }


        [TestCase(null, ExpectedResult = "Ignore")]
        [TestCase("", ExpectedResult = "Clear")]
        [TestCase("A", ExpectedResult = "Ignore")]
        [TestCase("B", ExpectedResult = "Set(B)")]
        public static string FilterTests(string val)
        {
            return UpdateTests
                .MakeUpdate(val)
                .Pipe(Update.Filter<string>(x => x == "B"))
                .ToString();
        }


        [TestCase(null, ExpectedResult = "Ignore")]
        [TestCase("", ExpectedResult = "Clear")]
        [TestCase("A", ExpectedResult = "Set(a)")]
        public static string MapTests(string val)
        {
            return UpdateTests
                .MakeUpdate(val)
                .Pipe(Update.Map<string, string>(x => x.ToLower()))
                .ToString();
        }


        [TestCase(null, ExpectedResult = "Do Nothing")]
        [TestCase("", ExpectedResult = "Erase")]
        [TestCase("A", ExpectedResult = "Set: A")]
        public static string MatchTests(string val)
        {
            return UpdateTests
                .MakeUpdate(val)
                .Pipe(Update.Match<string, string>(
                    x => $"Set: {x}",
                    () => "Do Nothing",
                    () => "Erase"));
        }


        [TestCase(null, false, ExpectedResult = "Ignore")]
        [TestCase(null, true, ExpectedResult = "Ignore")]
        [TestCase("", false, ExpectedResult = "Ignore")]
        [TestCase("", true, ExpectedResult = "Ignore")]
        [TestCase("A", false, ExpectedResult = "Ignore")]
        [TestCase("A", true, ExpectedResult = "Set(Set(A))")]
        public static string OfTypeTests(string val, bool wrapValue)
        {
            var innerUpdate = UpdateTests.MakeUpdate(val);
            
            return (wrapValue ? innerUpdate.ToUpdate() : Update.Ignore<Update<string>>())
                .Pipe(Update.OfType<Update<string>, UpdateSet<string>>())
                .ToString();
        }


        [TestCase(null, null, ExpectedResult = "None")]
        [TestCase(null, "A", ExpectedResult = "Some(A)")]
        [TestCase("", null, ExpectedResult = "None")]
        [TestCase("", "A", ExpectedResult = "None")]
        [TestCase("B", null, ExpectedResult = "Some(B)")]
        [TestCase("B", "A", ExpectedResult = "Some(B)")]
        public static string ResolveTests(string val, string existingVal)
        {
            var update = UpdateTests.MakeUpdate(val);
            var existing = existingVal is string a ? Option.Some(a) : Option<string>.None;
            
            return update.Resolve(existing).ToString();
        }


        [TestCase(null, ExpectedResult = "Ignore")]
        [TestCase("Foo", ExpectedResult = "Set(3)")]
        public static string SelectTests(string val)
        {
            return (from x in UpdateTests.MakeUpdate(val)
                    select x.Length)
                .ToString();
        }


        [TestCase(null, true, ExpectedResult = "Ignore")]
        [TestCase(null, false, ExpectedResult = "Ignore")]
        [TestCase("", true, ExpectedResult = "Clear")]
        [TestCase("", false, ExpectedResult = "Clear")]
        [TestCase("Foo", true, ExpectedResult = "Ignore")]
        [TestCase("Foo", false, ExpectedResult = "Set(False)")]
        public static string SelectManyTests(string val, bool returnSet)
        {
            return (from x in UpdateTests.MakeUpdate(val)
                    from y in returnSet
                        ? Update.Ignore<bool>()
                        : (x.Length == 0).ToUpdate()
                    select y)
                .ToString();
        }


        [TestCase(null, 1, ExpectedResult = "Right(1)")]
        [TestCase("", 1, ExpectedResult = "Right(1)")]
        [TestCase("A", 1, ExpectedResult = "Left(A)")]
        public static string ToEitherLeftTests(string val, int rightValue)
        {
            return UpdateTests
                .MakeUpdate(val)
                .ToEitherLeft(rightValue)
                .ToString();
        }


        [TestCase(null, 1, ExpectedResult = "Left(1)")]
        [TestCase("", 1, ExpectedResult = "Left(1)")]
        [TestCase("A", 1, ExpectedResult = "Right(A)")]
        public static string ToEitherRightTests(string val, int rightValue)
        {
            return UpdateTests
                .MakeUpdate(val)
                .ToEitherRight(rightValue)
                .ToString();
        }


        [TestCase(null, ExpectedResult = "None")]
        [TestCase("", ExpectedResult = "None")]
        [TestCase("A", ExpectedResult = "Some(A)")]
        public static string ToOptionTests(string val)
        {
            return UpdateTests
                .MakeUpdate(val)
                .ToOption()
                .ToString();
        }


        [TestCase(null, ExpectedResult = "Ignore")]
        [TestCase("", ExpectedResult = "Clear")]
        [TestCase("A", ExpectedResult = "Ignore")]
        [TestCase("B", ExpectedResult = "Set(1)")]
        public static string WhereTests(string val)
        {
            return (from x in UpdateTests.MakeUpdate(val)
                    where x == "B"
                    select x.Length)
                .ToString();
        }
        
        private static Update<string> MakeUpdate(string val)
        {
            return val is string a
                ? a == string.Empty
                    ? Update.Clear<string>()
                    : Update.Set(a)
                : Update.Ignore<string>();
        }
        
        private sealed class UpdateInvalid<T> : Update<T>
        {

            public UpdateInvalid() : base("Invalid")
            {
            }


            protected override IEnumerable<(string FieldName, object FieldValue)> GetFields()
            {
                throw new NotImplementedException();
            }

        }

    }

}