using System;
using System.Collections;
using System.Collections.Generic;

namespace Paps.Collections
{
    public interface IPoolable
    {
        bool IsAvailable();
    }

    public class ObjectPool<T> : ICollection<T> where T : IPoolable
    {
        private HashSet<T> objects;

        private Func<T> factoryMethod;

        public ObjectPool(Func<T> factoryMethod = null)
        {
            objects = new HashSet<T>();

            this.factoryMethod = factoryMethod;
        }

        public int Count
        {
            get
            {
                return objects.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public T GetFirstAvailable()
        {
            foreach(T obj in objects)
            {
                if(obj.IsAvailable())
                {
                    return obj;
                }
            }

            return default;
        }

        public T[] GetAvailable(int count)
        {
            if(count < 1)
            {
                return null;
            }

            List<T> requested = null;

            foreach(T obj in objects)
            {
                if (obj.IsAvailable())
                {
                    if (requested == null)
                    {
                        requested = new List<T>();
                    }

                    requested.Add(obj);
                }
            }

            if(requested != null)
            {
                return requested.ToArray();
            }
            else
            {
                return null;
            }
        }

        public void SetFactoryMethod(Func<T> method)
        {
            factoryMethod = method;
        }

        public void Fill(int count, Func<T> method)
        {
            if (method == null)
                return;

            for(int i = 0; i < count; i++)
            {
                Add(method());
            }
        }

        public void Fill(int count)
        {
            if (factoryMethod == null)
                return;

            for(int i = 0; i < count; i++)
            {
                Add(factoryMethod());
            }
        }
        
        public bool Add(T item)
        {
            return objects.Add(item);
        }

        public void Clear()
        {
            objects.Clear();
        }

        public bool Contains(T item)
        {
            return objects.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            objects.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return objects.GetEnumerator();
        }

        public bool Remove(T item)
        {
            return objects.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        void ICollection<T>.Add(T item)
        {
            Add(item);
        }
    }
}
