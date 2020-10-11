using System;
using System.Collections.Generic;
using System.Text;

namespace JWLibrary.Core {
    public class DBFileNameAttribute : Attribute {
        public string FileName { get; private set; }
        public DBFileNameAttribute(string filename) {
            FileName = filename;
        }
    }
}
