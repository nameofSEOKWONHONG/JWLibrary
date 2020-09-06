﻿using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace JWLibrary.Database {

    public class JDataBase {

        private static Lazy<JDatabaseInfo> JDataBaseInfo = new Lazy<JDatabaseInfo>(() => {
            return new JDatabaseInfo();
        });

        public static IDbConnection Resolve<TDatabase>()
            where TDatabase : DbConnection {
            if (typeof(TDatabase) == typeof(SqlConnection)) {
                return JDataBaseInfo.Value.ConKeyValues["MSSQL"];
            } else if (typeof(TDatabase) == typeof(MySqlConnection)) {
                return JDataBaseInfo.Value.ConKeyValues["MYSQL"];
            } else {
                throw new NotImplementedException();
            }
        }
    }
}