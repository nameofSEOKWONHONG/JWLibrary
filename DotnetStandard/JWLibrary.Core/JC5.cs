using C5;
using System.Collections.Generic;

namespace JWLibrary.Core {

    public class JList<T> : ArrayList<T> {

        public JList() : base(100) {
        }

        public JList(int capacity)
            : base(capacity) {
        }

        public JList(int capacity, MemoryType memoryType)
            : base(capacity, memoryType) {
        }

        public JList(IEnumerable<T> iterator) {
            AddAll(iterator);
        }
    }

    public class JLKList<T> : C5.LinkedList<T> {
    }

    public class JHList<T> : HashedArrayList<T> {

        public JHList() {
        }

        public JHList(int capacity) : base(capacity) {
        }

        public JHList(int capacity, MemoryType memoryType) : base(capacity, memoryType) {
        }
    }

    public class JHLKList<T> : HashedLinkedList<T> {

        public JHLKList() {
        }

        public JHLKList(MemoryType memoryType) : base(memoryType) {
        }
    }

    public class JHDictionary<K, V> : HashDictionary<K, V> {
    }

    public class JTDictionary<K, V> : TreeDictionary<K, V> {
    }
}