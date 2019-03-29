using System.Collections;
using System.Collections.Generic;
using System;

namespace Paps.Coroutines
{
    public class Coroutine : ICoroutine
    {
        public IEnumerator Iterator { get; set; }

        public virtual object Current { get; protected set; }

        protected Func<IEnumerator> iteratorFactoryMethod;

        public IEnumerator Nested
        {
            get
            {
                if(Current is IEnumerator iterator)
                {
                    return iterator;
                }

                return null;
            }
        }

        public Coroutine(IEnumerator iterator = null, Func<IEnumerator> iteratorFactoryMethod = null)
        {
            Iterator = iterator;
            this.iteratorFactoryMethod = iteratorFactoryMethod;
        }

        public void SetIteratorFactoryMethod(Func<IEnumerator> method)
        {
            iteratorFactoryMethod = method;
        }

        protected bool NestedMoveNext()
        {
            if(Nested != null)
            {
                return Nested.MoveNext();
            }

            return false;
        }

        public virtual bool MoveNext()
        {
            if(NestedMoveNext())
            {
                return true;
            }

            bool moveNext = Iterator.MoveNext();

            Current = Iterator.Current;

            return moveNext;
        }

        public virtual void Reset()
        {
            Iterator = iteratorFactoryMethod();
        }
    }

    public class Coroutine<T> : Coroutine, ICoroutine<T>, IDisposable
    {
        private IEnumerator<T> internalIterator;

        public new IEnumerator<T> Iterator
        {
            get
            {
                return internalIterator;
            }

            set
            {
                internalIterator = value;
                base.Iterator = value;
            }
        }

        public T YieldResult { get; protected set; }

        protected new Func<IEnumerator<T>> iteratorFactoryMethod;

        public Coroutine(IEnumerator<T> iterator = null, Func<IEnumerator<T>> iteratorFactoryMethod = null)
        {
            Iterator = iterator;
            this.iteratorFactoryMethod = iteratorFactoryMethod;
        }

        private new void SetIteratorFactoryMethod(Func<IEnumerator> method)
        {
            base.SetIteratorFactoryMethod(method);
        }

        public void SetIteratorFactoryMethod(Func<IEnumerator<T>> method)
        {
            iteratorFactoryMethod = method;
        }

        public override bool MoveNext()
        {
            if(NestedMoveNext())
            {
                return true;
            }

            bool moveNext = Iterator.MoveNext();

            YieldResult = Iterator.Current;

            return moveNext;
        }

        public void Dispose()
        {
            Iterator.Dispose();
        }
    }
}
