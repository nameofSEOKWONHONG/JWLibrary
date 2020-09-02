using JWLibrary.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Linq;

using Dapper;

namespace JWLibrary.Database {
    /// <summary>
    /// Database Client Extension
    /// </summary>
    public static class JDBClientExtension {
        private static Dictionary<SQL_COMPILER_TYPE, SqlKata.Compilers.Compiler> _COMPILER_MAP = new Dictionary<SQL_COMPILER_TYPE, SqlKata.Compilers.Compiler>() {
            { SQL_COMPILER_TYPE.MSSQL, new SqlKata.Compilers.SqlServerCompiler() },
            { SQL_COMPILER_TYPE.MYSQL, new SqlKata.Compilers.MySqlCompiler() },
            { SQL_COMPILER_TYPE.ORACLE, new SqlKata.Compilers.OracleCompiler() },
            { SQL_COMPILER_TYPE.POSTGRESQL, new SqlKata.Compilers.PostgresCompiler() },
            { SQL_COMPILER_TYPE.SQLLITE, new SqlKata.Compilers.SqliteCompiler() },
        };

        #region [sync]
        #region [self impletment func method]
        public static T jGet<T>(this IDbConnection connection, Func<IDbConnection, T> func)
             where T : class, new() {
            try {
                connection.Open();
                return func(connection);
            } finally {
                connection.Close();
            }
        }

        public static IEnumerable<T> jGetAll<T>(this IDbConnection connection, Func<IDbConnection, IEnumerable<T>> func)
            where T : class, new() {
            try {
                connection.Open();
                return func(connection);
            } finally {
                connection.Close();
            }
        }

        public static bool jExecute(this IDbConnection connection, Func<IDbConnection, bool> func) {
            try {
                connection.Open();
                return func(connection);
            } finally {
                connection.Close();
            }
        }
        #endregion

        #region [sql implement and result. after call func method]
        public static T jGet<T>(this IDbConnection connection, string sql, Func<T, T> func = null)
            where T : class, new() {
            var result = default(T);
            try {
                connection.Open();
                result = connection.QueryFirstOrDefault<T>(sql);
                if (func.jIsNotNull()) return func(result);
                return result;
            } finally {
                connection.Close();
            }
        }

        public static IEnumerable<T> jGetAll<T>(this IDbConnection connection, string sql, Func<IEnumerable<T>, IEnumerable<T>> func)
            where T : class, new() {
            var result = default(IEnumerable<T>);
            try {
                connection.Open();
                result = connection.Query<T>(sql);
                if (func.jIsNotNull()) return func(result);
                return result;
            } finally {
                connection.Close();
            }
        }

        public static bool jExecute(this IDbConnection connection, string sql, Action<bool> action = null) {
            try {
                connection.Open();
                var result = connection.Execute(sql) > 0;
                action(result);
                return result;
            } finally {
                connection.Close();
            }
        }
        #endregion

        #region [using sqlkata]
        public static T jGet<T>(this IDbConnection connection, SqlKata.Query query, SQL_COMPILER_TYPE sqlCompilerType = SQL_COMPILER_TYPE.MSSQL, Func<T, T> func = null)
            where T : class, new() {
            var result = default(T);
            try {
                connection.Open();
                var compiler = _COMPILER_MAP.Where(m => m.Key == sqlCompilerType).jFirst();
                var compilerResult = compiler.Value.Compile(query);
                result = connection.QueryFirstOrDefault<T>(compilerResult.ToString());
                if (func.jIsNotNull()) return func(result);
                return result;
            } finally {
                connection.Close();
            }
        }

        public static IEnumerable<T> jGetAll<T>(this IDbConnection connection, SqlKata.Query query, SQL_COMPILER_TYPE sqlCompilerType = SQL_COMPILER_TYPE.MSSQL, Func<IEnumerable<T>, IEnumerable<T>> func = null)
            where T : class, new() {
            var result = default(IEnumerable<T>);
            try {
                connection.Open();
                var compiler = _COMPILER_MAP.Where(m => m.Key == sqlCompilerType).jFirst();
                var compilerResult = compiler.Value.Compile(query);
                result = connection.Query<T>(compilerResult.ToString());
                if (func.jIsNotNull()) return func(result);
                return result;
            } finally {
                connection.Close();
            }
        }

        public static bool jExecute<T>(this IDbConnection connection, SqlKata.Query query, SQL_COMPILER_TYPE sqlCompilerType = SQL_COMPILER_TYPE.MSSQL, Action<bool> action = null)
            where T : class, new() {
            try {
                connection.Open();
                var compiler = _COMPILER_MAP.Where(m => m.Key == sqlCompilerType).jFirst();
                var compilerResult = compiler.Value.Compile(query);
                var result = connection.Execute(compilerResult.ToString()) > 0;
                action(result);
                return result;
            } finally {
                connection.Close();
            }
        }
        #endregion

        public static void BulkInsert<T>(this IDbConnection connection, IEnumerable<T> bulkDatas, string tableName = null)
            where T : class, new() {
            try {
                if (tableName.jIsNullOrEmpty()) throw new NullReferenceException("table name is null or empty.");

                var entity = new T();
                var dt = bulkDatas.jToDataTable();

                using (SqlBulkCopy objbulk = new SqlBulkCopy((SqlConnection)connection, SqlBulkCopyOptions.Default, null)) {
                    objbulk.DestinationTableName = tableName.jIsNullOrEmpty() == true ? entity.GetType().Name : tableName;
                    foreach (var property in entity.GetType().GetProperties()) {
                        objbulk.ColumnMappings.Add(property.Name, property.Name);
                    }

                    connection.Open();
                    objbulk.WriteToServer(dt);
                }
            } finally {
                connection.Close();

            }
        }
        #endregion

        #region [async]
        #region [self implement func method async]
        public static async Task<T> jGetAsync<T>(this IDbConnection connection, Func<IDbConnection, Task<T>> asyncFunc)
            where T : class, new() {
            try {
                connection.Open();
                return await asyncFunc(connection);
            } finally {
                connection.Close();
            }
        }

        public static async Task<IEnumerable<T>> jGetAllAsync<T>(this IDbConnection connection, Func<IDbConnection, Task<IEnumerable<T>>> asyncFunc)
            where T : class, new() {
            try {
                connection.Open();
                return await asyncFunc(connection);
            } finally {
                connection.Close();
            }
        }

        public static async Task<bool> jExecuteAsync(this IDbConnection connection, Func<IDbConnection, Task<bool>> asyncFunc) {
            try {
                connection.Open();
                return await asyncFunc(connection);
            } finally {
                connection.Close();
            }
        }
        #endregion
        #endregion

    }

    public enum SQL_COMPILER_TYPE {
        MSSQL,
        MYSQL,
        ORACLE,
        FIREBIRD,
        SQLLITE,
        POSTGRESQL
    }
}