using System.Collections.Generic;
using NUnit.Framework;

namespace FunSharp.Common.Test
{

    public static class StructuralEqualityTests
    {

        [TestCase("Joe Blow", "Bob", ExpectedResult = false)]
        [TestCase("Joe Blow", null, ExpectedResult = false)]
        [TestCase("Joe Blow", "", ExpectedResult = false)]
        [TestCase("Joe Blow", "same", ExpectedResult = true)]
        [TestCase("Joe Blow", "Joe Blow", ExpectedResult = true)]
        public static bool ObjectEqualsTests(string fullName1, string fullName2)
        {
            var name1 = Name.Parse(fullName1);
            var name2 = fullName2 is null 
                ? null
                : fullName2 == string.Empty
                    ? new int[0]
                    : fullName2 == "same"
                        ? name1
                        : Name.Parse(fullName2) as object;

            return name1.Equals(name2);
        }

        [TestCase("Joe Blow", "Bob", ExpectedResult = false)]
        [TestCase("Joe Blow", null, ExpectedResult = false)]
        [TestCase("Joe Blow", "Joe Blow", ExpectedResult = true)]
        public static bool EqualsTests(string fullName1, string fullName2)
        {
            var name1 = Name.Parse(fullName1);
            var name2 = fullName2 is null
                ? null
                : Name.Parse(fullName2);

            return name1.Equals(name2);
        }

        [TestCase("Joe Blow", "Bob", ExpectedResult = false)]
        [TestCase("Joe Blow", null, ExpectedResult = false)]
        [TestCase("Joe Blow", "Joe Blow", ExpectedResult = true)]
        public static bool op_EqualityTests(string fullName1, string fullName2)
        {
            var name1 = Name.Parse(fullName1);
            var name2 = fullName2 is null
                ? null
                : Name.Parse(fullName2);

            return name1 == name2;
        }

        [TestCase("Joe Blow", "Bob", ExpectedResult = true)]
        [TestCase("Joe Blow", null, ExpectedResult = true)]
        [TestCase("Joe Blow", "Joe Blow", ExpectedResult = false)]
        public static bool op_InequalityTests(string fullName1, string fullName2)
        {
            var name1 = Name.Parse(fullName1);
            var name2 = fullName2 is null
                ? null
                : Name.Parse(fullName2);

            return name1 != name2;
        }

        [TestCase("Joe Blow", "Bob", ExpectedResult = false)]
        [TestCase("Joe Blow", "Joe Blow", ExpectedResult = true)]
        public static bool GetHashCodeTests(string fullName1, string fullName2)
        {
            var name1 = Name.Parse(fullName1);
            var name2 = Name.Parse(fullName2);

            return name1.GetHashCode() == name2.GetHashCode();
        }

        [TestCase("Joe Blow", ExpectedResult = "Name { FirstName = Joe, LastName = Some(Blow) }")]
        [TestCase("Bob", ExpectedResult = "Name { FirstName = Bob, LastName = None }")]
        public static string ToStringTests(string fullName)
        {
            return Name.Parse(fullName).ToString();
        }
        
        private sealed class Name : StructuralEquality<Name>
        {

            public static Name Parse(string fullName)
            {
                var names = fullName.Split(" ");
                return new Name(names[0], names.Length > 1 ? Option.Some(names[1]) : Option<string>.None);
            }

            public Name(string firstName, Option<string> lastName)
            {
                FirstName = firstName;
                LastName = lastName;
            }

            public readonly string FirstName;
            public readonly Option<string> LastName;

            protected override IEnumerable<(string FieldName, object FieldValue)> GetFields()
            {
                yield return (nameof(this.FirstName), this.FirstName);
                yield return (nameof(this.LastName), this.LastName);
            }

        }

    }

}