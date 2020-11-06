using System;

namespace JWLibrary.Database {

    public class LiteDbFileNameAttribute : Attribute {

        public LiteDbFileNameAttribute(string fileName, string tableName) {
            FileName = fileName;
            TableName = tableName;
        }

        public string FileName { get; }
        public string TableName { get; }
    }
}