using System.Collections;

namespace Paps.Coroutines
{
    public class Coroutine : IYieldInstruction
    {
        public IEnumerator Iterator { get; protected set; }
        public bool paused;

        public Coroutine(IEnumerator iterator)
        {
            Override(iterator);
        }

        public bool KeepWaiting
        {
            get
            {
                if(paused)
                {
                    return true;
                }
                else
                {
                    return RecursiveMoveNext(Iterator);
                }
            }
        }
        
        protected virtual bool RecursiveMoveNext(IEnumerator recursiveIterator)
        {
            if (recursiveIterator.Current is IYieldInstruction yieldInst)
            {
                if (yieldInst.KeepWaiting)
                {
                    return true;
                }
            }
            else if (recursiveIterator.Current is IEnumerator enumerator)
            {
                if (RecursiveMoveNext(enumerator))
                {
                    return true;
                }
            }

            bool moveNext = recursiveIterator.MoveNext();

            if(moveNext)
            {
                if (recursiveIterator.Current is IYieldInstruction || recursiveIterator.Current is IEnumerator)
                {
                    return RecursiveMoveNext(recursiveIterator);
                }

                return true;
            }

            return false;
        }

        public void Override(IEnumerator _iterator)
        {
            Iterator = _iterator;
        }
    }
}
