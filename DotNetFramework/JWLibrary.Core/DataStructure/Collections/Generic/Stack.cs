using System;
using System.Collections;
using System.Collections.Generic;

namespace JWLibrary.Core.NetFramework.DataStructure.Collections.Generic
{
    public class Stack<T> : IEnumerable<T>, IEnumerable, ICollection
    {
        private CNode<T> head;
        public int Count { get; private set; }
        private readonly object syncRoot = new object();
        public object SyncRoot
        {
            get
            {
                return this;
            }
        }

        public bool IsSynchronized
        {
            get
            {
                return true;
            }
        }

        public Stack()
        {
            //head = new CNode<T>();
            //head.Link = null;
        }

        ~Stack()
        {
            head = null;
        }

        public void Push(T entity)
        {
            CNode<T> node = new CNode<T>();
            node.Item = entity;

            if (head == null)
            {
                head = node;
            }
            else
            {
                node.Link = head.Link;
                head.Link = node;
            }

            Count++;
        }

        public T Pop()
        {
            if (Count <= 0) return default(T);

            CNode<T> node = head;
            T item = node.Item;
            head = head.Link;
            node = null;
            Count--;
            return item;
        }

        public T GetItem(int index)
        {
            CNode<T> node = head;

            int cnt = 0;
            for (; node != null; node = node.Link)
            {
                if (index == cnt) return node.Item;
                cnt++;
            }

            return default(T);
        }

        public T Peek()
        {
            return head.Link.Item;
        }

        public T[] ToArray()
        {
            T[] Items = new T[Count];

            CNode<T> node = head;

            for (int i = 0; i < Count; i++)
            {
                Items[i] = node.Item;
                node = node.Link;
            }

            return Items;
        }

        public int Length()
        {
            int cnt = 0;
            CNode<T> node = head;

            for (; node != null; node = node.Link)
                cnt++;

            return cnt;
        }

        public void Clear()
        {
            head = null;
            Count = 0;

            GC.Collect();
        }

        public IEnumerator<T> GetEnumerator()
        {
            List<T> list = new List<T>();

            CNode<T> node = head;
            for (; node != null; node = node.Link)
            {
                list.Add(node.Item);
            }

            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            for (int i = index; i < array.Length; i++)
            {
                T t = (T)array.GetValue(i);

                if (t != null)
                    Push((T)array.GetValue(i));
            }
        }

        public class CNode<T>
        {
            public T Item { get; set; }
            public CNode<T> Link { get; set; }
        }
    }
}
