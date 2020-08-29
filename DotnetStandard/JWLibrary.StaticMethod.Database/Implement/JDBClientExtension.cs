using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace JWLibrary.StaticMethod.Database
{
    /// <summary>
    /// Database Client Extension
    /// </summary>
    public static class JDBClientExtension
    {
        #region [sync]
        public static T Get<T>(this IDbConnection connection, Func<IDbConnection, T> func)
            where T : class, new()
        {
            try {
                connection.Open();
                return func(connection);
            }
            finally {
                connection.Close();
            }
        }

        public static IEnumerable<T> GetAll<T>(this IDbConnection connection, Func<IDbConnection, IEnumerable<T>> func)
            where T : class, new()
            {
                try {
                    connection.Open();
                    return func(connection);
                }
                finally {
                    connection.Close();
                }
            }

        public static bool Execute(this IDbConnection connection, Func<IDbConnection, bool> func) {
            try {
                connection.Open();
                return func(connection);
            }
            finally {
                connection.Close();
            }
        }

        public static void BulkInsert<T>(this IDbConnection connection, IEnumerable<T> bulkDatas, string tableName = null)
            where T : class, new()
        {
            try
            {
                var entity = new T();
                var dt = bulkDatas.jToDataTable();

                using(SqlBulkCopy objbulk = new SqlBulkCopy((SqlConnection)connection, SqlBulkCopyOptions.Default, null))
                {
                    objbulk.DestinationTableName = tableName.jIsNullOrEmpty() == true ? entity.GetType().Name : tableName;
                    foreach(var property in entity.GetType().GetProperties()) {
                        objbulk.ColumnMappings.Add(property.Name, property.Name);
                    }

                    connection.Open();
                    objbulk.WriteToServer(dt);
                }
            }
            finally {
                connection.Close();

            }
        }
        #endregion

        #region [async]
        public static async Task<T> GetAsync<T>(this IDbConnection connection, Func<IDbConnection, Task<T>> asyncFunc)
            where T : class, new()
            {
                try {
                    connection.Open();
                    return await asyncFunc(connection);
                }
                finally {
                    connection.Close();
                }
            }



        public static async Task<IEnumerable<T>> GetAllAsync<T>(this IDbConnection connection, Func<IDbConnection, Task<IEnumerable<T>>> asyncFunc)
            where T : class, new()
        {
            try
            {
                connection.Open();
                return await asyncFunc(connection);
            }
            finally
            {
                connection.Close();
            }
        }

        public static async Task<bool> ExecuteAsync(this IDbConnection connection, Func<IDbConnection, Task<bool>> asyncFunc) {
            try {
                connection.Open();
                return await asyncFunc(connection);
            }
            finally {
                connection.Close();
            }
        }
        #endregion

    }
}