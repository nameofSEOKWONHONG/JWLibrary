using JWLibrary.Core;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace JWLibrary.Database {
    public static class JDBNoSqlClientExtension {
        #region [litedb]
        public static ILiteCollection<T> jGetCollection<T>(this ILiteDatabase liteDatabase, string entityName)
            where T : class {
            return liteDatabase.GetCollection<T>();
        }
        public static ILiteDatabase jBeginTrans(this ILiteDatabase liteDatabase) {
            if (liteDatabase.BeginTrans().jIsFalse()) {
                throw new Exception("litedb transaction failed on begintrans");
            }
            return liteDatabase;
        }

        public static T jGet<T>(this ILiteCollection<T> liteCollection, Func<T, bool> predicate) {
            return liteCollection.FindOne(x => predicate(x));
        }

        public static IEnumerable<T> jGetAll<T>(this ILiteCollection<T> liteCollection) {
            return liteCollection.FindAll();
        }

        public static bool jUpdate<T>(this ILiteCollection<T> liteCollection, T entity) {
            return liteCollection.Update(entity);
        }

        public static BsonValue jInsert<T>(this ILiteCollection<T> liteCollection, T entity) {
            return liteCollection.Insert(entity);
        }

        public static bool jDelete<T>(this ILiteCollection<T> liteCollection, BsonValue id) {
            return liteCollection.Delete(id);
        }

        public static bool jDeleteAll<T>(this ILiteCollection<T> liteCollection) {
            return liteCollection.DeleteAll() > 0;
        }

        public static bool jDeleteMany<T>(this ILiteCollection<T> liteCollection, Expression<Func<T, bool>> predicate) {
            return liteCollection.DeleteMany(predicate) > 0;
        }

        public static bool jUpsert<T>(this ILiteCollection<T> liteCollection, T entity) {
            return liteCollection.Upsert(entity);
        }
        public static bool jCommit(this ILiteDatabase liteDatabase) {
            var result = liteDatabase.Commit();
            if (result.jIsFalse()) {
                throw new Exception("litedb transaction failed on commit");
            }
            return result;
        }
        public static bool jRollback(this ILiteDatabase liteDatabase) {
            var result = liteDatabase.Rollback();
            if (result.jIsFalse()) {
                throw new Exception("litedb transaction failed on rollback");
            }
            return result;
        }
        #endregion [litedb]
    }
}
