using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.Core.DataStructure.Collections
{
    public class Stack
    {
        private readonly int max;
        private object[] element;
        public int Count { get; private set; }

        public Stack(int max)
        {
            this.max = max;
            element = new object[max];
            Count = -1;
        }

        public void Push(object obj)
        {
            if (IsFull())
                throw new Exception("overflow.");

            element[++Count] = obj;
        }

        public object Pop()
        {
            if (IsEmpty())
                throw new Exception("is empty.");

            return element[Count--];
        }

        public virtual void CopyTo(Array array, int index)
        {
            for (int i = index; i < array.Length; i++)
            {
                Push(array.GetValue(i));
            }
        }

        private bool IsEmpty()
        {
            if (Count <= -1)
                return true;

            return false;
        }

        private bool IsFull()
        {
            if (Count >= max)
                return true;

            return false;
        }
    }
}
