using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace JWLibrary.Core {

    public static class JCollection {

        #region [count & length method]

        public static int jCount<T>(this IEnumerable<T> collection) {
            if (collection.jIsNull()) return 0;
            var result = 0;
            using (var enumerator = collection.GetEnumerator()) {
                while (enumerator.MoveNext())
                    result++;
            }

            return result;
        }

        #endregion [count & length method]

        #region [for & foreach]

        /// <summary>
        ///     use struct, no break
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iterator"></param>
        /// <param name="action"></param>
        public static void jForEach<T>(this IEnumerable<T> iterator, Action<T> action)
            where T : struct {
            if (iterator.jCount() > JConst.LOOP_WARNING_COUNT)
                Trace.TraceInformation($"OVER LOOP WARNING COUNT ({JConst.LOOP_WARNING_COUNT})");

            var index = 0;
            foreach (var item in iterator) {
                action(item);

                if (index % JConst.LOOP_LIMIT == 0)
                    JConst.SetInterval(JConst.SLEEP_INTERVAL);
                index++;
            }
        }

        /// <summary>
        ///     use struct, no break
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iterator"></param>
        /// <param name="action"></param>
        public static void jForEach<T>(this IEnumerable<T> iterator, Action<T, int> action)
            where T : struct {
            if (iterator.jCount() > JConst.LOOP_WARNING_COUNT)
                Trace.TraceInformation($"OVER LOOP WARNING COUNT ({JConst.LOOP_WARNING_COUNT})");

            var index = 0;
            iterator.jForEach<T>(item => {
                action(item, index);
                index++;
            });
        }

        /// <summary>
        ///     use class, allow break;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iterator"></param>
        /// <param name="func"></param>
        public static void jForEach<T>(this IEnumerable<T> iterator, Func<T, bool> func)
            where T : class, new() {
            if (iterator.jCount() > JConst.LOOP_WARNING_COUNT)
                Trace.TraceInformation($"OVER LOOP WARNING COUNT ({JConst.LOOP_WARNING_COUNT})");

            var index = 0;
            foreach (var item in iterator) {
                var isBreak = func(item);
                if (isBreak) break;

                if (index % JConst.LOOP_LIMIT == 0)
                    JConst.SetInterval(JConst.SLEEP_INTERVAL);
                index++;
            }
        }

        /// <summary>
        ///     use class, allow break;
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="iterator"></param>
        /// <param name="func"></param>
        public static void jForEach<T>(this IEnumerable<T> iterator, Func<T, int, bool> func)
            where T : class, new() {
            if (iterator.jCount() > JConst.LOOP_WARNING_COUNT)
                Trace.TraceInformation($"OVER LOOP WARNING COUNT ({JConst.LOOP_WARNING_COUNT})");

            var index = 0;
            iterator.jForEach<T>(item => {
                var isBreak = func(item, index);
                index++;

                if (isBreak) return true;
                return true;
            });
        }

        #endregion [for & foreach]

        #region [Datatable & DataReader]

        public static DataTable jToDataTable<T>(this IEnumerable<T> entities)
            where T : class, new() {
            var entity = new T();
            var properties = entity.GetType().GetProperties();

            var dt = new DataTable();
            foreach (var property in properties) dt.Columns.Add(property.Name, property.PropertyType);

            entities.jForEach(item => {
                var itemProperty = item.GetType().GetProperties();
                var row = dt.NewRow();
                foreach (var property in itemProperty) row[property.Name] = property.GetValue(item);
                dt.Rows.Add(row);
                return true;
            });

            return dt;
        }

        public static T jToObject<T>(this IDataReader reader)
            where T : class, new() {
            var properties = typeof(T).GetProperties().jToList();

            var newItem = new T();

            Enumerable.Range(0, reader.FieldCount - 1).jForEach(i => {
                if (!reader.IsDBNull(i)) {
                    var property = properties.Where(m => m.Name.Equals(reader.GetName(i))).jFirst();
                    if (property.jIsNotNull())
                        if (reader.GetFieldType(i).Equals(property.PropertyType))
                            property.SetValue(newItem, reader[i]);
                }
            });

            return newItem;
        }

        #endregion [Datatable & DataReader]
    }
}