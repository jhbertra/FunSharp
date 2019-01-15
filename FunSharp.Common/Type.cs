using System.Collections.Generic;
using JetBrains.Annotations;

namespace FunSharp.Common
{

    [PublicAPI]
    public sealed class Type<T> : StructuralEquality<Type<T>>
    {
        
        [NotNull] public static readonly Type<T> Get = new Type<T>();

        private Type()
        {
        }

        protected override IEnumerable<(string FieldName, object FieldValue)> GetFields()
        {
            yield break;
        }

    }

}