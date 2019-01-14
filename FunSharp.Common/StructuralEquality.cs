using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace FunSharp.Common
{

    public abstract class StructuralEquality<T> : IEquatable<StructuralEquality<T>>
    {

        public bool Equals(StructuralEquality<T> other)
        {
            return !(other is null)
                && other.GetType() == this.GetType()
                && this
                    .GetFields()
                    .Select(
                        x =>
                        {
                            var (_, fieldValue) = x;
                            return fieldValue;
                        })
                    .Zip(
                        other
                            .GetFields()
                            .Select(
                                x =>
                                {
                                    var (_, fieldValue) = x;
                                    return fieldValue;
                                }),
                        (a, b) => a.Equals(b))
                    .Aggregate(true, (state, x) => state && x);
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }
            else if (obj is StructuralEquality<T> other)
            {
                return this.Equals(other);
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return this
                .GetFields()
                .Select(
                    x =>
                    {
                        var (_, fieldValue) = x;
                        return fieldValue;
                    })
                .Aggregate(
                    this.GetType().GetHashCode(),
                    (state, x) => (state * 257 ^ x.GetHashCode()));
        }

        public static bool operator ==(StructuralEquality<T> left, StructuralEquality<T> right)
        {
            return object.Equals(left, right);
        }

        public static bool operator !=(StructuralEquality<T> left, StructuralEquality<T> right)
        {
            return !object.Equals(left, right);
        }

        public override string ToString()
        {
            switch (this)
            {
                case IUnionType union:
                    var fieldValues = this.GetFields().Select(x => x.FieldValue).ToImmutableList();
                    return fieldValues.IsEmpty
                        ? union.Tag
                        : $"{union.Tag}({string.Join(", ", fieldValues.Select(x => x.ToString()))})";

                default:
                    var fields = this.GetFields().ToImmutableList();
                    return fields.IsEmpty
                        ? this.GetType().Name
                        : $"{this.GetType().Name} {{ {string.Join(", ", fields.Select(x => $"{x.FieldName} = {x.FieldValue}"))} }}";
            }
        }

        protected abstract IEnumerable<(string FieldName, object FieldValue)> GetFields();

    }

}