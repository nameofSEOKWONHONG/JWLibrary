using Dapper;
using JWLibrary.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace JWLibrary.Database {

    /// <summary>
    /// Database Client Extension
    /// </summary>
    public static class JDBClientExtension {

        private static readonly Dictionary<SQL_COMPILER_TYPE, SqlKata.Compilers.Compiler> _COMPILER_MAP = new Dictionary<SQL_COMPILER_TYPE, SqlKata.Compilers.Compiler>() {
            { SQL_COMPILER_TYPE.MSSQL, new SqlKata.Compilers.SqlServerCompiler() },
            { SQL_COMPILER_TYPE.MYSQL, new SqlKata.Compilers.MySqlCompiler() },
            { SQL_COMPILER_TYPE.ORACLE, new SqlKata.Compilers.OracleCompiler() },
            { SQL_COMPILER_TYPE.POSTGRESQL, new SqlKata.Compilers.PostgresCompiler() },
            { SQL_COMPILER_TYPE.SQLLITE, new SqlKata.Compilers.SqliteCompiler() },
        };

        #region [sync]

        #region [self impletment func method]

        /// <summary>
        /// you can execute func method, use dbconnection, any code. (use dapper, ef, and so on...)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T jQuery<T>(this IDbConnection connection, Func<IDbConnection, T> func) {
            try {
                connection.Open();
                return func(connection);
            } finally {
                connection.Close();
            }
        }

        /// <summary>
        /// async execute(select, update, delete, insert) db, use any code on func method. (use dapper, ef and so on...)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static async Task<T> jQueryAsync<T>(this IDbConnection connection, Func<IDbConnection, Task<T>> func)
            where T : class, new() {
            try {
                connection.Open();
                return await func(connection);
            } finally {
                connection.Close();
            }
        }

        #endregion [self impletment func method]

        #region [sql implement and result. after call func method]

        /// <summary>
        /// get db data, use sql (use inner dapper)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <param name="func"></param>
        /// <returns></returns>
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

        public async static Task<T> jGetAsync<T>(this IDbConnection connection, string sql, Func<T, T> func = null)
            where T : class, new() {
            var result = default(T);
            try {
                connection.Open();
                result = await connection.QueryFirstOrDefaultAsync<T>(sql);
                if (func.jIsNotNull()) return func(result);
                return result;
            } finally {
                connection.Close();
            }
        }

        /// <summary>
        /// get db all data, use sql
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IEnumerable<T> jGetAll<T>(this IDbConnection connection, string sql, Func<IEnumerable<T>, IEnumerable<T>> func = null)
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

        public async static Task<IEnumerable<T>> jGetAllAsync<T>(this IDbConnection connection, string sql, Func<IEnumerable<T>, IEnumerable<T>> func = null)
            where T : class, new() {
            var result = default(IEnumerable<T>);
            try {
                connection.Open();
                result = await connection.QueryAsync<T>(sql);
                if (func.jIsNotNull()) return func(result);
                return result;
            } finally {
                connection.Close();
            }
        }

        public static bool jUpdate<T>(this IDbConnection connection, T entity)
            where T : class {
            var result = false;
            try {
                connection.Open();
                result = connection.Update<T>(entity) > 0;
            }
            finally {
                connection.Close();
            }
            return result;
        }

        public static async Task<bool> jUpdateAsync<T>(this IDbConnection connection, T entity)
            where T : class {
            var result = false;
            try {
                connection.Open();
                result = await connection.UpdateAsync<T>(entity) > 0;
            }
            finally {
                connection.Close();
            }
            return result;
        }

        public static bool jInsert<T>(this IDbConnection connection, T entity)
            where T : class {
            var result = false;

            try {
                connection.Open();
                result = connection.Insert<T>(entity) > 0;
            }
            finally {
                connection.Close();
            }

            return result;
        }

        /// <summary>
        /// execute(update, delete) db, use sql
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="sql"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static bool jExecute(this IDbConnection connection, string sql, Action<bool> action = null) {
            try {
                connection.Open();
                var result = connection.Execute(sql) > 0;
                if (action.jIsNotNull())
                    action(result);
                return result;
            } finally {
                connection.Close();
            }
        }

        public async static Task<bool> jExecuteAsync(this IDbConnection connection, string sql, Action<bool> action = null) {
            try {
                connection.Open();
                var result = await connection.ExecuteAsync(sql) > 0;
                if (action.jIsNotNull())
                    action(result);
                return result;
            } finally {
                connection.Close();
            }
        }

        #endregion [sql implement and result. after call func method]

        #region [using sqlkata]

        /// <summary>
        /// get db data, use sqlkata
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="query"></param>
        /// <param name="sqlCompilerType"></param>
        /// <param name="func"></param>
        /// <returns></returns>
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

        public async static Task<T> jGetAsync<T>(this IDbConnection connection, SqlKata.Query query, SQL_COMPILER_TYPE sqlCompilerType = SQL_COMPILER_TYPE.MSSQL, Func<T, T> func = null)
            where T : class, new() {
            var result = default(T);
            try {
                connection.Open();
                var compiler = _COMPILER_MAP.Where(m => m.Key == sqlCompilerType).jFirst();
                var compilerResult = compiler.Value.Compile(query);
                result = await connection.jGetAsync<T>(compilerResult.ToString());
                if (func.jIsNotNull()) return func(result);
                return result;
            } finally {
                connection.Close();
            }
        }

        /// <summary>
        /// get db all data, use sqlkata
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="query"></param>
        /// <param name="sqlCompilerType"></param>
        /// <param name="func"></param>
        /// <returns></returns>
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

        public async static Task<IEnumerable<T>> jGetAllAsync<T>(this IDbConnection connection, SqlKata.Query query, SQL_COMPILER_TYPE sqlCompilerType = SQL_COMPILER_TYPE.MSSQL, Func<IEnumerable<T>, IEnumerable<T>> func = null)
            where T : class, new() {
            var result = default(IEnumerable<T>);
            try {
                connection.Open();
                var compiler = _COMPILER_MAP.Where(m => m.Key == sqlCompilerType).jFirst();
                var compilerResult = compiler.Value.Compile(query);
                result = await connection.jGetAllAsync<T>(compilerResult.ToString());
                if (func.jIsNotNull()) return func(result);
                return result;
            } finally {
                connection.Close();
            }
        }

        /// <summary>
        /// execute(update, delete) db, use sqlkata
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="query"></param>
        /// <param name="sqlCompilerType"></param>
        /// <param name="action"></param>
        /// <returns></returns>
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

        public async static Task<bool> jExecuteAsync<T>(this IDbConnection connection, SqlKata.Query query, SQL_COMPILER_TYPE sqlCompilerType = SQL_COMPILER_TYPE.MSSQL, Action<bool> action = null)
            where T : class, new() {
            try {
                connection.Open();
                var compiler = _COMPILER_MAP.Where(m => m.Key == sqlCompilerType).jFirst();
                var compilerResult = compiler.Value.Compile(query);
                var result = await connection.jExecuteAsync(compilerResult.ToString());
                action(result);
                return result;
            } finally {
                connection.Close();
            }
        }

        #endregion [using sqlkata]

        /// <summary>
        /// bulk insert, use datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="bulkDatas"></param>
        /// <param name="tableName"></param>
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

        public async static Task BulkInsertAsync<T>(this IDbConnection connection, IEnumerable<T> bulkDatas, string tableName = null)
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
                    await objbulk.WriteToServerAsync(dt);
                }
            } finally {
                connection.Close();
            }
        }

        #endregion [sync]
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