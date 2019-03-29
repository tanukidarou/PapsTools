using System;
using System.Collections;
using System.Collections.Generic;

namespace Paps.Coroutines
{
    public class CoroutineQueueScheduler : ICoroutineScheduler
    {
        private Queue<ICoroutine> coroutines;

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public object Current
        {
            get
            {
                return coroutines.Peek();
            }
        }

        public int Count
        {
            get
            {
                return coroutines.Count;
            }
        }

        public CoroutineQueueScheduler()
        {
            coroutines = new Queue<ICoroutine>();
        }

        public bool MoveNext()
        {
            if (coroutines.Count > 0)
            {
                if (coroutines.Peek().MoveNext() == false)
                {
                    coroutines.Dequeue();
                    MoveNext();
                }
                else
                {
                    return true;
                }
            }

            return false;
        }

        [Obsolete("You cannot remove an item from a queue. Use Remove() instead", true)]
        public bool Remove(ICoroutine item)
        {
            throw new NotSupportedException("You cannot remove an item from a queue. Use Remove() instead");
        }

        public ICoroutine Remove()
        {
            return coroutines.Dequeue();
        }

        public void Reset()
        {

        }

        public ICoroutine Add(IEnumerator iterator)
        {
            Coroutine coroutine = new Coroutine(iterator);

            coroutines.Enqueue(coroutine);

            return coroutine;
        }

        public ICoroutine<T> Add<T>(IEnumerator<T> iterator)
        {
            Coroutine<T> coroutine = new Coroutine<T>(iterator);

            coroutines.Enqueue(coroutine);

            return coroutine;
        }

        public void Add(ICoroutine item)
        {
            coroutines.Enqueue(item);
        }

        public void Clear()
        {
            coroutines.Clear();
        }

        public bool Contains(ICoroutine item)
        {
            return coroutines.Contains(item);
        }

        public void CopyTo(ICoroutine[] array, int arrayIndex)
        {
            coroutines.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ICoroutine> GetEnumerator()
        {
            return coroutines.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
