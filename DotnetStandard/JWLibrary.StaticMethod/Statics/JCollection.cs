using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;

namespace JWLibrary.StaticMethod
{
    public static class JCollection
    {

        #region [count & length method]
        public static int jCount(this ICollection collection)
        {
            return collection.jIsNull() ? 0 : collection.Count;
        }
        #endregion

        #region  [for & foreach]
        public static void jForEach<T>(this IEnumerable<T> iterator, Func<T, bool> func)
            where T : class, new()
        {
            var index = 0;
            foreach (var item in iterator)
            {
                var isContinue = func(item);
                if(!isContinue) break;
                
                if (index % JConst.LOOP_LIMIT == 0) 
                    JConst.SetInterval(JConst.SLEEP_INTERVAL);                
                index++;                
            }
        }

        public static void jForEach<T>(this IEnumerable<T> iterator, Func<T, int, bool> func)
            where T : class, new()
        {
            var index = 0;
            foreach (var item in iterator)
            {
                var isContinue = func(item, index);
                if(!isContinue) break;

                if (index % JConst.LOOP_LIMIT == 0) 
                    JConst.SetInterval(JConst.SLEEP_INTERVAL);                
                index++;                
            }
        }
        #endregion

        #region  [object deep copay]
        public static TDest jToCopy<TSrc, TDest>(this TSrc src)
            where TSrc : class, new()
            where TDest : class, new()
        {
            var tdest = new TDest();

            var tsrcProperties = src.GetType().GetProperties();
            var tdestProperties = tdest.GetType().GetProperties();

            foreach (var srcProperty in tsrcProperties)
            {
                foreach (var tdestProperty in tdestProperties)
                {
                    if (srcProperty.Name == tdestProperty.Name && srcProperty.PropertyType == tdestProperty.PropertyType)
                    {
                        tdestProperty.SetValue(tdest, srcProperty.GetValue(src));
                        break;
                    }
                }
            }

            return tdest;
        }
        #endregion

 

        #region [Datatable]
        public static DataTable jToDataTable<T>(this IEnumerable<T> entities)
            where T : class, new() 
        {
            var entity = new T();
            var properties = entity.GetType().GetProperties();

            DataTable dt = new DataTable();
            foreach(var property in properties) {
                dt.Columns.Add(property.Name, property.PropertyType);
            }

            entities.jForEach(item => {
                var itemProperty = item.GetType().GetProperties();
                var row = dt.NewRow();
                foreach(var property in itemProperty) {
                    row[property.Name] = property.GetValue(item);
                }
                dt.Rows.Add(row);
                return true;
            });

            return dt;
        }
        #endregion
    }
}