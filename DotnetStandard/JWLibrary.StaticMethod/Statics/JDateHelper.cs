using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWLibrary.StaticMethod
{
    public static class JDateHelper
    {
        public static DateTime jToDateTime(this string date)
        {
            DateTime datetime = DateTime.MinValue;
            DateTime.TryParse(date, out datetime);
            return datetime;
        }

        public static string jToDateTime(this string date, string format = null)
        {
            DateTime datetime = DateTime.MinValue;
            DateTime.TryParse(date, out datetime);
            return datetime.ToString(format);
        }

        public static string jToDateTime(this DateTime date, string format = null)
        {
            return date.ToString(format);
        }
    }

    public enum ConvertFormat
    {
        [StringValue("yyyy-MM-dd")]
        YYYY_MM_DD,
        [StringValue("yyyyMMdd")]
        YYYYMMDD,
        [StringValue("yyyy/MM/dd")]
        YYYY_S_MM_S_DD,
        [StringValue("yyyyMMddHHmmss")]
        YYYYMMDDHHMMSS,
        [StringValue("HHmmss")]
        HHMMSS,
    }
}
