using System.Collections;

namespace Paps.Coroutines
{
    public interface ICoroutine : IEnumerator
    {

    }

    public interface ICoroutine<T> : ICoroutine
    {
        T YieldResult { get; }
    }
}
