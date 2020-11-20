using JWLibrary.Core;
using LiteDB;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;

namespace JWLibrary.Database {

    public class JDataBase {

        private static readonly Lazy<JDatabaseInfo> JDataBaseInfo = new Lazy<JDatabaseInfo>(() => {
            return new JDatabaseInfo();
        });

        public static IDbConnection Resolve<TDatabase>()
            where TDatabase : IDbConnection {
            if (typeof(TDatabase) == typeof(SqlConnection))
                return JDataBaseInfo.Value.ConKeyValues["MSSQL"];
            if (typeof(TDatabase) == typeof(MySqlConnection))
                return JDataBaseInfo.Value.ConKeyValues["MYSQL"];
            throw new NotImplementedException();
        }
    }
}