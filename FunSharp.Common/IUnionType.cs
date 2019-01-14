using JetBrains.Annotations;

namespace FunSharp.Common
{

    public interface IUnionType
    {

        [NotNull] string Tag { get; }

    }

}