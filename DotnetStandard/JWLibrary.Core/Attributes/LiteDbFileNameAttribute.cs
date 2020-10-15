using System;
using System.Collections.Generic;
using System.Text;

namespace JWLibrary.Core {
    public class LiteDbFileNameAttribute : Attribute {
        public string FileName { get; private set; }
        public LiteDbFileNameAttribute(string filename) {
            FileName = filename;
        }
    }
}
