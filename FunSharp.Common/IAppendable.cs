using JetBrains.Annotations;

namespace FunSharp.Common
{

    [PublicAPI]
    public interface IAppendable<T>
    {

        [NotNull] T Append(T t);

    }

}