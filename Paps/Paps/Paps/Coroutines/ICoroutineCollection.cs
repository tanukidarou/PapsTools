using System.Collections;
using System.Collections.Generic;

namespace Paps.Coroutines
{
    public interface ICoroutineScheduler : ICoroutine, ICollection<ICoroutine>
    {
        ICoroutine Add(IEnumerator iterator);
        ICoroutine<T> Add<T>(IEnumerator<T> iterator);
    }
}
