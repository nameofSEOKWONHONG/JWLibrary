using C5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JWLibrary.Core {

    public class JList<T> : C5.ArrayList<T> {
        public JList() {

        }

        public JList(int capacity) 
            : base(capacity){

        }

        public JList(int capacity, MemoryType memoryType) 
            : base(capacity, memoryType) {
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

    public class JHDictionary<T1, T2> : C5.HashDictionary<T1, T2> {

    }

    public class JTDictionary<T1, T2> : C5.TreeDictionary<T1, T2> {

    }

}
