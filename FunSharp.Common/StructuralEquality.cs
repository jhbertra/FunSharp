using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using JetBrains.Annotations;

namespace FunSharp.Common
{

    //--------------------------------------------------
    /// <summary>
    /// Base class for immutable objects whose equality
    /// is defined by the structure of their data.
    /// </summary>
    [PublicAPI]
    public abstract class StructuralEquality<T> : IEquatable<StructuralEquality<T>>
    {

        //**************************************************
        //* Methods
        //**************************************************

        //--------------------------------------------------
        /// <inheritdoc />
        public bool Equals(StructuralEquality<T> other)
        {
            if (other is null)
            {
                return false;
            }
            else if (other.GetType() != this.GetType())
            {
                return false;
            }
            else
            {
                using (var enumerator1 = this.GetFields().GetEnumerator())
                using (var enumerator2 = other.GetFields().GetEnumerator())
                {
                    var hasNext1 = enumerator1.MoveNext();
                    var hasNext2 = enumerator2.MoveNext();

                    while (hasNext1 || hasNext2)
                    {
                        var fieldValue1 = hasNext1 ? enumerator1.Current.FieldValue : null;
                        var fieldValue2 = hasNext2 ? enumerator2.Current.FieldValue : null;

                        if (!object.Equals(fieldValue1, fieldValue2))
                        {
                            return false;
                        }

                        hasNext1 = enumerator1.MoveNext();
                        hasNext2 = enumerator2.MoveNext();
                    }
                }

                return true;
            }
        }


        //--------------------------------------------------
        /// <inheritdoc />
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


        //--------------------------------------------------
        /// <inheritdoc />
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


        //--------------------------------------------------
        /// <inheritdoc />
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


        //--------------------------------------------------
        /// <summary>
        /// Enumerates the names and values of the data fields in
        /// this structure, for equality evaluation and display.
        /// </summary>
        protected abstract IEnumerable<(string FieldName, object FieldValue)> GetFields();


        //**************************************************
        //* Operators
        //**************************************************

        public static bool operator ==(StructuralEquality<T> left, StructuralEquality<T> right)
        {
            return object.Equals(left, right);
        }

        public static bool operator !=(StructuralEquality<T> left, StructuralEquality<T> right)
        {
            return !object.Equals(left, right);
        }

    }

}