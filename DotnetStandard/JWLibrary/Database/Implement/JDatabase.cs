using JWLibrary.Core;
using LiteDB;
using MySql.Data.MySqlClient;
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
            where TDatabase : IDbConnection {
            if (typeof(TDatabase) == typeof(SqlConnection)) {
                return JDataBaseInfo.Value.ConKeyValues["MSSQL"];
            } else if (typeof(TDatabase) == typeof(MySqlConnection)) {
                return JDataBaseInfo.Value.ConKeyValues["MYSQL"];
            } else {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// create litedb instance.
        /// litetime is single instance. 
        /// so, create once.
        /// also, create instance per use;
        /// **example**
        /// JDatabase.Resolve<ILiteDatabase>().jGetCollection<Customer>("customers")...codes...
        /// var db = JDatabase.Resolve<ILiteDatabse>();
        /// .... your code ....
        /// db.Dispose();
        /// </summary>
        /// <typeparam name="TDatabase"></typeparam>
        /// <param name="fileConnection"></param>
        /// <returns></returns>
        public static ILiteDatabase Resolve<TDatabase>(string fileConnection = null)
            where TDatabase : ILiteDatabase {
            if (fileConnection.jIsNotNull()) {
                return new LiteDatabase(fileConnection);
            }
            return JDataBaseInfo.Value.NoSqlConKeyValues["LITEDB"];
        }

        public static ILiteDatabase Resolve<TDatabase, TEntity>()
            where TDatabase : ILiteDatabase
            where TEntity : class {
            var fileConnection = typeof(TEntity).jGetAttributeValue((LiteDbFileNameAttribute dn) => dn.FileName).jToAppPath();
            if (fileConnection.jIsNotNull()) {
                return new LiteDatabase(fileConnection);
            }
            return JDataBaseInfo.Value.NoSqlConKeyValues["LITEDB"];
        }


    }
}