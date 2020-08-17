using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace JWLibrary.StaticMethod
{
    public class JList<T> : IList<T>
        where T : class, new()
    {
        private HashSet<T> _hashset;
        public JList() {
            _hashset = new HashSet<T>();
        }

        public JList(int capacity) {
            _hashset = new HashSet<T>();
        }

        public JList(IEnumerable<T> enumerable) {
            _hashset = new HashSet<T>(enumerable);
        }

        public T this[int index] {
            get {
                if(index > this.Count - 1) return null;

                return _hashset.ToList()[index];
            }
            set {
                var item = _hashset.ElementAt(index);
                item  = value;
            }
        }

        public int Count {
            get {
                return _hashset.Count;
            }
        }

        public bool IsReadOnly {get{return false;}}

        public virtual void Add(T item)
        {
            _hashset.Add(item);
        }

        public virtual void Clear()
        {
            _hashset.Clear();
            _hashset = null;
            GC.SuppressFinalize(this);
            GC.Collect();
        }

        public virtual bool Contains(T item)
        {
            return _hashset.Contains(item);
        }

        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            _hashset.CopyTo(array, arrayIndex);
        }

        public virtual IEnumerator<T> GetEnumerator()
        {
            return _hashset.GetEnumerator();
        }

        public virtual int IndexOf(T item)
        {
            return _hashset.ToList().IndexOf(item);
        }

        public virtual void Insert(int index, T item)
        {
            var hashset = new HashSet<T>();
            for(var i=0; i < _hashset.Count; i++)
            {
                if(i == index) {
                    hashset.Add(item);
                }
                hashset.Add(_hashset.ElementAt(i));
            }
            _hashset = hashset;
        }

        public virtual bool Remove(T item)
        {
            return _hashset.Remove(item);
        }

        public virtual void RemoveAt(int index)
        {
            var hashset = new HashSet<T>();
            for(var i=0; i<_hashset.Count; i++) {
                if(i != index) {
                    hashset.Add(_hashset.ElementAt(i));
                }
            }
            _hashset = hashset;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _hashset.GetEnumerator();
        }
    }
}