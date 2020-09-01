using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.Core.NetFramework.DataStructure
{
    public class CircleList<T> : List<T>, IEnumerable<T>
        where T : class
    {
        private int index = 0;
        public int Index
        {
            get
            {
                return index;
            }
        }

        public CircleList()
        {

        }

        ~CircleList()
        {
            this.Clear();
        }

        public void Init()
        {
            index = 0;
        }

        public T First()
        {
            index = 0;
            return this[0];
        }

        public T Now()
        {
            return this[index];
        }

        public T Next()
        {
            index++;

            if (index == this.Count)
            {
                index = 0;
            }

            return this[index];
        }

        public T Previous()
        {
            index--;

            if (index <= 0)
            {
                index = this.Count - 1;
            }

            return this[index];
        }

        public T End()
        {
            index = this.Count - 1;
            return this[this.Count - 1];
        }

        public void AddItem(T item)
        {
            this.Insert(this.index, item);
        }

        public void RemoveItem()
        {
            this.RemoveAt(this.index);
        }

        public void RemoveItem(int idx)
        {
            this.RemoveAt(idx);
            if (this.Count > index) { index = this.Count - 1; }
        }

        public CircleList<T> Arrange(bool desc)
        {
            List<T> templist = null;
            if (!desc)
            {
                templist = this.OrderBy(item => item).ToList();
            }
            else
            {
                templist = this.OrderByDescending(item => item).ToList();
            }
            var circularList = new CircleList<T>();
            circularList.AddRange(templist);
            return circularList;
        }
    }
}
