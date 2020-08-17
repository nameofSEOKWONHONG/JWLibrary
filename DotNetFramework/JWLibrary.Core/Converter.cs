namespace JWLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Data;
    using System.Reflection;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json;

    public static class Converters
    {
        /// <summary>
        /// DataTable을 List<T>형태로 변환한다.
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<TSource> ToList<TSource>(this DataTable dataTable) where TSource : new()
        {
            var dataList = new List<TSource>();

            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;

            var objFieldNames = (from PropertyInfo aProp in typeof(TSource).GetProperties(flags)
                                 select new { Name = aProp.Name, Type = Nullable.GetUnderlyingType(aProp.PropertyType) ?? aProp.PropertyType })
                                 .ToList();

            var dataTblFieldNames = (from DataColumn aHeader in dataTable.Columns
                                     select new { Name = aHeader.ColumnName, Type = aHeader.DataType })
                                     .ToList();

            var commonFields = objFieldNames.Intersect(dataTblFieldNames).ToList();

            foreach (DataRow dr in dataTable.AsEnumerable().ToList())
            {
                var aTSource = new TSource();
                foreach (var aField in commonFields)
                {
                    PropertyInfo propertyInfos = aTSource.GetType().GetProperty(aField.Name); //origin

                    if (propertyInfos == null)
                        propertyInfos = aTSource.GetType().GetProperty(aField.Name.ToUpper());

                    if (propertyInfos == null)
                        propertyInfos = aTSource.GetType().GetProperty(aField.Name.ToLower());

                    var value = (dr[aField.Name] == DBNull.Value) ? null : dr[aField.Name];
                    propertyInfos.SetValue(aTSource, value, null);
                }

                dataList.Add(aTSource);
            }

            return dataList;
        }

        public static JArray ToJArray(this DataTable dataTable)
        {
            JArray result = new JArray();
            JObject row;
            foreach (DataRow dr in dataTable.Rows)
            {
                row = new JObject();
                foreach (DataColumn col in dataTable.Columns)
                {
                    row.Add(col.ColumnName.Trim(), JToken.FromObject(dr[col]));
                }
                result.Add(row);
            }

            return result;
        }

        public static string ToJson(this DataTable dataTable)
        {
            string[] columns = dataTable.Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray();
            IEnumerable<Dictionary<string, object>> result = dataTable.Rows.Cast<DataRow>().Select(dr => columns.ToDictionary(c => c, c => dr[c]));

            return JsonConvert.SerializeObject(result);
        }

        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static JArray ToJArray(this object obj)
        {
            return JArray.FromObject(obj);
        }

        public static TSource ToEntity<TSource>(this string json)
        {
            return JsonConvert.DeserializeObject<TSource>(json);
        }

        public static DataTable ToDataTable(this string json)
        {
            return JsonConvert.DeserializeObject<DataTable>(json);
        }

        public static string ToFormat(this object val, FormatType type, bool isMask = false)
        {
            if (val == null)
                return null;

            if (val is DateTime)
                return DateTime.Parse(val.ToString()).ToString("yyyy-MM-dd HH:mm:ss");

            if (string.IsNullOrEmpty((string)val)
                return "0";

            if (val.ToString() == "0")
                return "0";

            switch (type)
            {
                case FormatType.Comma:
                    {
                        if (val is string)
                        {
                            return string.Format("{0:#,###}", Int64.Parse(val.ToString()));
                        }

                        return string.Format("{0:#,###}", val);
                    }
                case FormatType.Rate:
                    {
                        return string.Format("{0:##.##}", val);
                    }
                case FormatType.Mobile:
                    {
                        if (val.ToString().Length == 11 || val.ToString().Length == 13)
                        {
                            val = val.ToString().Replace("-", "");
                            if (!isMask)
                            {
                                return string.Format("{0}-{1}-{2}",
                                    val.ToString().FirstGetByLength(3),
                                    val.ToString().MiddleGetByLength(3, 4),
                                    val.ToString().LastGetByLength(4));
                            }

                            return string.Format("{0}-****-{1}",
                                val.ToString().FirstGetByLength(3),
                                val.ToString().LastGetByLength(4));
                        }
                        return val.ToString();
                    }
                case FormatType.Phone:
                    {
                        val = val.ToString().Replace("-", "");
                        var temp = val.ToString();
                        if (!isMask)
                        {
                            if (temp.FirstGetByLength(2) == "02")
                            {
                                if (temp.Length == 10)
                                {
                                    return string.Format("{0}-{1}-{2}", temp.FirstGetByLength(2),
                                    temp.MiddleGetByLength(2, 4),
                                    temp.LastGetByLength(4));
                                }
                                else
                                {
                                    return string.Format("{0}-{1}-{2}", temp.FirstGetByLength(2),
                                    temp.MiddleGetByLength(2, 3),
                                    temp.LastGetByLength(4));
                                }
                            }
                            else
                            {
                                if (temp.Length == 11)
                                {
                                    return string.Format("{0}-{1}-{2}", temp.FirstGetByLength(3),
                                    temp.MiddleGetByLength(3, 4),
                                    temp.LastGetByLength(4));
                                }
                                else
                                {
                                    return string.Format("{0}-{1}-{2}", temp.FirstGetByLength(3),
                                    temp.MiddleGetByLength(3, 3),
                                    temp.LastGetByLength(4));
                                }
                            }
                        }
                        else
                        {
                            if (temp.FirstGetByLength(2) == "02")
                            {
                                if (temp.Length == 10)
                                {
                                    return string.Format("{0}-****-{1}", temp.FirstGetByLength(2),
                                    temp.LastGetByLength(4));
                                }
                                else
                                {
                                    return string.Format("{0}-****-{1}", temp.FirstGetByLength(2),
                                    temp.LastGetByLength(4));
                                }
                            }
                            else
                            {
                                if (temp.Length == 11)
                                {
                                    return string.Format("{0}-****-{1}", temp.FirstGetByLength(3),
                                    temp.LastGetByLength(4));
                                }
                                else
                                {
                                    return string.Format("{0}-****-{1}", temp.FirstGetByLength(3),
                                    temp.LastGetByLength(4));
                                }
                            }
                        }
                    }
                case FormatType.RRN:
                    {
                        if (!isMask)
                        {
                            return string.Format("{0}-{1}", val.ToString().FirstGetByLength(6),
                            val.ToString().LastGetByLength(7));
                        }

                        return string.Format("{0}-*******", val.ToString().FirstGetByLength(6));
                    }
                case FormatType.YYYY_MM_DD:
                    {
                        return $"{val.ToString().FirstGetByLength(4)}-{val.ToString().MiddleGetByLength(4, 2)}-{val.ToString().LastGetByLength(2)}";
                    }
                case FormatType.YYYY_DOT_MM_DOT_DD:
                    {
                        return $"{val.ToString().FirstGetByLength(4)}.{val.ToString().MiddleGetByLength(4, 2)}.{val.ToString().LastGetByLength(2)}";
                    }
                case FormatType.Email:
                    {
                        if (isMask)
                        {
                            var split = val.ToString().Split('@');
                            var result = string.Empty;

                            if(split.Length > 1)
                            {
                                var suffix = string.Empty;
                                for (int i = 0; i < split[0].Length; i++)
                                {                                    
                                    if (i < 3)
                                    {
                                        suffix += split[0][i].ToString();
                                    }
                                    else
                                    {
                                        suffix += "*";
                                    }
                                }
                                result = suffix + "@" + split[1];
                                return result;
                            }
                            else
                            {
                                return val.ToString();
                            }
                        }
                        return val.ToString();
                    }
                case FormatType.Name:
                    {
                        if (isMask)
                        {
                            string result = string.Empty;
                            for (int i = 0; i < val.ToString().Length; i++)
                            {
                                if (i == 0 || i == val.ToString().Length - 1)
                                {
                                    result += val.ToString()[i];
                                }
                                else
                                {
                                    result += "*";
                                }
                            }

                            return result;
                        }
                        return val.ToString();
                    }
                case FormatType.REMOVE_T_DATETIME:
                    {
                        return val.ToString().Replace("T", " ");
                    }
                default:
                    {
                        return val.ToString();
                    }
            }
        }

        public static string MiddleGetByLength(this string value, int fromLen, int getLen)
        {
            return value.Substring(fromLen, getLen);
        }

        public static string FirstGetByLength(this string value, int length)
        {
            return value.Substring(0, length);
        }

        public static string LastGetByLength(this string value, int length)
        {
            return value.Substring(value.Length - length, length);
        }

        /// <summary>
        /// 객체의 멤버 프로퍼티와 값을 문자열로 반환한다.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToPropertyKeyValueString<T>(this T obj)
        {
            string writeParameter = string.Empty;

            var propertyInfos = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(x => x.MemberType == MemberTypes.Field || x.MemberType == MemberTypes.Property);

            foreach (var propertyInfo in propertyInfos)
            {
                writeParameter += $" key:{propertyInfo.Name}, value:{propertyInfo.GetValue(obj)}" + Environment.NewLine;
            }

            return writeParameter;
        }

        public static bool IsNullOrEmpty(this string str)
        {
            if (string.IsNullOrEmpty(str)) return true;

            return false;
        }

        /// <summary>
        /// Datetime을 포멧에 따라 문자열로 반환한다.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToDateString(this DateTime date, DateFormatType format)
        {
            switch (format)
            {
                case DateFormatType.YYYY_MM_DD:
                    return date.ToString("yyyy-MM-dd");
                case DateFormatType.YYYYMMDD:
                    return date.ToString("yyyyMMdd");
                case DateFormatType.HHMMSS:
                    return date.ToString("HHmmss");
                case DateFormatType.YYYY_S_MM_S_DD:
                    return date.ToString("yyyy/MM/dd");
                case DateFormatType.YYYYMMDDHHMMSS:
                    return date.ToString("yyyyMMddHHmmss");
                case DateFormatType.YYYY_C_MM_C_DD:
                    return date.ToString("yyyy:MM:dd");
                case DateFormatType.YYYYMM:
                    return date.ToString("yyyyMM");
                case DateFormatType.PARSE_DATE:
                    return date.ToString();
                default:
                    return date.ToLongDateString();
            }
        }

        public static string ToDateString(this string dateString, DateFormatType format)
        {
            if (dateString.IsNullOrEmpty()) return string.Empty;
            if (dateString.length < 10) throw new NotImlmentException("not convert string format.");

            if (dateString.IndexOf('-') <= 0)
            {
                dateString = dateString.Substring(0, 4) + "-" + dateString.Substring(4, 2) + "-" + dateString.Substring(6, 2);
            }

            if (dateString.IndexOf('.') <= 0) {
                dateString = dateString.Substring(0, 4) + "-" + dateString.Substring(4, 2) + "-" + dateString.Substring(6, 2);
            }

            switch (format)
            {
                case DateFormatType.YYYY_MM_DD:
                    return DateTime.Parse(dateString).ToString("yyyy-MM-dd");
                case DateFormatType.YYYYMMDD:
                    return DateTime.Parse(dateString).ToString("yyyyMMdd");
                case DateFormatType.HHMMSS:
                    return DateTime.Parse(dateString).ToString("HHmmss");
                case DateFormatType.YYYY_S_MM_S_DD:
                    return DateTime.Parse(dateString).ToString("yyyy/MM/dd");
                case DateFormatType.YYYYMMDDHHMMSS:
                    return DateTime.Parse(dateString).ToString("yyyyMMddHHmmss");
                case DateFormatType.YYYY_C_MM_C_DD:
                    return DateTime.Parse(dateString).ToString("yyyy:MM:dd");
                case DateFormatType.YYYYMM:
                    return DateTime.Parse(dateString).ToString("yyyyMM");
                case DateFormatType.PARSE_DATE:
                    return DateTime.Parse(dateString).ToString();
                default:
                    throw new NotImlmentException("not support convert format.");
            }
        }

        public static string ToHtmlPath(this string filePath)
        {
            return filePath.Replace("\\", "/");
        }

        public static string ToDateFileName(this string fileName)
        {
            var now = DateTime.Now;
            var hhmmss = now.ToString(DateFormatType.HHMMSS);

            var extPos = fileName.LastIndexOf(".");
            var prefix = "";
            var tempExt = "";

            if (extPos >= 0)
            {
                prefix = fileName.Substring(0, extPos);
                tempExt = fileName.Substring(extPos);
            }

            return prefix + "_" + hhmmss + tempExt;
        }

        public static string ToFileNameWithOutPath(this string fileName)
        {
            return System.IO.Path.GetFileName(fileName);
        }
    }

    public static class JArrayHelper
    {
        public static JToken GetJToken(this JToken jtoken, string key)
        {
            return jtoken[key];
        }

        public static TEntity GetEntity<TEntity>(this JToken jtoken)
        {
            return JsonConvert.DeserializeObject<TEntity>(jtoken.ToString());
        }

        public static TEntity GetEntity<TEntity>(this JToken jtoken, string key)
        {
            return JsonConvert.DeserializeObject<TEntity>(jtoken[key].ToString());
        }

        public static List<TEntity> GetEntites<TEntity>(this JToken jtoken)
        {
            return JsonConvert.DeserializeObject<List<TEntity>>(jtoken.ToString());
        }

        public static List<TEntity> GetEntites<TEntity>(this JToken jtoken, string key)
        {
            return JsonConvert.DeserializeObject<List<TEntity>>(jtoken[key].ToString());
        }
    }

    public class DataTableHelper
    {
        public static DataTable ToMaskFormat(DataTable dataTable, Dictionary<string, object> maskCols)
        {
            var columns = dataTable.Columns.Cast<DataColumn>().ToArray();

            var resultDt = new DataTable("Table");

            foreach (var col in columns)
            {
                resultDt.Columns.Add(col.ColumnName, typeof(string));
            }

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow dr = resultDt.NewRow();


                System.Threading.Tasks.Parallel.ForEach(columns, col =>
                {
                    if (maskCols.Keys.Contains(col.ColumnName))
                    {
                        object objKey = null;
                        maskCols.TryGetValue(col.ColumnName, out objKey);
                        dr[col.ColumnName] = dataTable.Rows[i][col.ColumnName].ToFormat((FormatType)objKey, false);
                    }
                    else
                    {
                        dr[col.ColumnName] = dataTable.Rows[i][col.ColumnName].ToString();
                    }
                });

                resultDt.Rows.Add(dr);
            }

            return resultDt;
        }

        public static DataTable ToComma(DataTable dataTable, string[] cols)
        {
            var columns = dataTable.Columns.Cast<DataColumn>().ToArray();

            var resultDt = new DataTable("Table");

            foreach (var col in columns)
            {
                resultDt.Columns.Add(col.ColumnName, typeof(string));
            }

            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                DataRow dr = resultDt.NewRow();

                System.Threading.Tasks.Parallel.ForEach(columns, col =>
                {
                    if (cols.Contains(col.ColumnName))
                    {
                        if (dataTable.Rows[i][col.ColumnName] is Int64)
                        {
                            dr[col.ColumnName] = dataTable.Rows[i][col.ColumnName].ToFormat(FormatType.Comma, false);
                        }
                        else if (dataTable.Rows[i][col.ColumnName] is Double)
                        {
                            dr[col.ColumnName] = dataTable.Rows[i][col.ColumnName].ToFormat(FormatType.Comma, false);
                        }
                        else if (dataTable.Rows[i][col.ColumnName] is float)
                        {
                            dr[col.ColumnName] = dataTable.Rows[i][col.ColumnName].ToFormat(FormatType.Rate, false);
                        }
                        else
                        {
                            dr[col.ColumnName] = dataTable.Rows[i][col.ColumnName].ToFormat(FormatType.Comma, false);
                        }
                    }
                    else
                    {
                        dr[col.ColumnName] = dataTable.Rows[i][col.ColumnName].ToString();
                    }
                });

                resultDt.Rows.Add(dr);
            }

            return resultDt;
        }
    }

    public enum DateFormatType
    {
        /// <summary>
        /// 년월
        /// </summary>
        YYYYMM,
        /// <summary>
        /// 년-월-일
        /// </summary>
        YYYY_MM_DD,
        /// <summary>
        /// 년월일
        /// </summary>
        YYYYMMDD,
        /// <summary>
        /// 년/월/일
        /// </summary>
        YYYY_S_MM_S_DD,
        /// <summary>
        /// 년월일시분초
        /// </summary>
        YYYYMMDDHHMMSS,
        /// <summary>
        /// 시분초
        /// </summary>
        HHMMSS,
        /// <summary>
        /// 년,월,일
        /// </summary>
        YYYY_C_MM_C_DD,
        /// <summary>
        /// 원본 내역을 그대로 변환한다.
        /// </summary>
        PARSE_DATE
    }

    public enum FormatType
    {
        Comma,
        Rate,
        Mobile,
        RRN,
        CofficePrice,
        Phone,
        YYYY_MM_DD,
        YYYY_DOT_MM_DOT_DD,
        REMOVE_T_DATETIME, //input) 2017-12-04T08:30:13
        Email,
        Name

    }
}
