using System.Collections.Generic;

namespace JWLibrary.Core {

    public class JCircleList<T> : JList<T>
        where T : class, new() {

        public JCircleList() {
        }

        public JCircleList(int capacity)
            : base(capacity) {
        }

        public JCircleList(IEnumerable<T> enumerable) {
            AddAll(enumerable);
        }

        public int Index { get; private set; }

        public T Next() {
            Index++;
            if (Index > base.Count - 1)
                Index = 0;
            else if (Index < 0) Index = 0;

            return base[Index];
        }

        public T Previous() {
            Index--;
            if (Index < 0)
                Index = 0;
            else if (Index > base.Count - 1) Index = 0;

            return base[Index];
        }
    }
}