using System;

namespace JWLibrary.Database {
    [Obsolete("no more use - replace LiteDbFlex", true)]
    public class LiteDbFileNameAttribute : Attribute {

        public LiteDbFileNameAttribute(string fileName, string tableName) {
            FileName = fileName;
            TableName = tableName;
        }

        public string FileName { get; }
        public string TableName { get; }
    }
}