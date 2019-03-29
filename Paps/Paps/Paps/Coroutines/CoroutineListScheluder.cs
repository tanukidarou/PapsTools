using System.Collections;
using System.Collections.Generic;

namespace Paps.Coroutines
{
    class CoroutineListScheluder : ICoroutineScheduler
    {
        private List<ICoroutine> coroutines;

        private int currentIndex = 0;

        public object Current
        {
            get
            {
                return null;
            }
        }

        public int Count
        {
            get
            {
                return coroutines.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public ICoroutine Add(IEnumerator iterator)
        {
            Coroutine coroutine = new Coroutine(iterator);

            Add(coroutine);

            return coroutine;
        }

        public ICoroutine<T> Add<T>(IEnumerator<T> iterator)
        {
            Coroutine<T> coroutine = new Coroutine<T>(iterator);

            Add(coroutine);

            return coroutine;
        }

        public void Add(ICoroutine item)
        {
            coroutines.Add(item);
        }

        public void Clear()
        {
            coroutines.Clear();
            currentIndex = 0;
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

        public bool MoveNext()
        {
            for (currentIndex = 0; currentIndex < Count; currentIndex++)
            {
                ICoroutine coroutine = coroutines[currentIndex];

                if (coroutine.MoveNext() == false)
                {
                    Remove(coroutine);
                }
            }

            if (Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Remove(ICoroutine item)
        {
            if(Remove(item))
            {
                currentIndex--;
                return true;
            }

            return false;
        }

        public void Reset()
        {
            
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
