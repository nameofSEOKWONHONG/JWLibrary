using System;
using System.Collections.Generic;
using System.Text;

namespace JWLibrary.Database {
    public class LiteDbFileNameAttribute : Attribute {
        public string FileName { get; private set; }
        public string TableName { get; private set; }
        public LiteDbFileNameAttribute(string fileName, string tableName) {
            FileName = fileName;
            TableName = tableName;
        }
    }
}
