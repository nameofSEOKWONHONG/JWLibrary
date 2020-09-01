using JWLibrary.Core;
using System.Collections.Generic;

namespace JWLibrary.StaticMethod
{
    public class JCircleList<T> : JList<T>
        where T : class, new()
    {
        public int Index {get; private set;}

        public JCircleList() {

        }

        public JCircleList(int capacity) 
            : base(capacity) 
        {
            
        }

        public JCircleList(IEnumerable<T> enumerable)
            : base(enumerable)
            {
                
            }            

        public T Next()
        {
            this.Index++;
            if(this.Index > base.Count -  1) 
            {
                this.Index = 0;
            }
            else if(this.Index < 0) 
            {
                Index = 0;
            }

            return base[this.Index];
        }

        public T Previous()
        {
            this.Index--;
            if(this.Index < 0) {
                this.Index = 0;
            }
            else if(this.Index > base.Count - 1) {
                this.Index = 0;
            }

            return base[this.Index];
        }
    }
}