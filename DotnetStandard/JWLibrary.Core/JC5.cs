using C5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JWLibrary.Core {
    public class JList<T> : C5.ArrayList<T> {
        public JList() : base(100, MemoryType.Normal) {

        }

        public JList(int capacity)
            : base(capacity, MemoryType.Normal) {

        }

        public JList(int capacity, MemoryType memoryType)
            : base(capacity, memoryType) {
        }

        public JList(IEnumerable<T> iterator) {
            this.AddAll(iterator);
        }
    }

    public class JLKList<T>: C5.LinkedList<T> {
        public JLKList() { }
    }

    public class JHList<T> : C5.HashedArrayList<T> {
        public JHList() { }
        public JHList(int capacity) : base(capacity) { }
        public JHList(int capacity, MemoryType memoryType) : base(capacity, memoryType) { }
    }

    public class JHLKList<T> : C5.HashedLinkedList<T> {
        public JHLKList() { }
        public JHLKList(MemoryType memoryType) : base(memoryType) { }

    }

    public class JHDictionary<K, V> : C5.HashDictionary<K, V> {

    }

    public class JTDictionary<K, V> : C5.TreeDictionary<K, V> {

    }
}
