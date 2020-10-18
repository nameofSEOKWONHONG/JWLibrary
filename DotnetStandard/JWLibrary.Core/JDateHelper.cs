using JWLibrary.Core;
using System;

namespace JWLibrary.Core {

    public static class JDateHelper {

        public static DateTime jToDateTime(this string date) {
            DateTime datetime = DateTime.MinValue;
            DateTime.TryParse(date, out datetime);
            return datetime;
        }

        public static string jToDateTime(this DateTime date, ConvertFormat format = ConvertFormat.Default) {
            return date.ToString(format.jToString());
        }

        public static string jToDateTime(this DateTime date, string format = null) {
            if (format.jIsNullOrEmpty()) format = "yyyy-MM-dd";
            return date.ToString(format);
        }
    }

    public enum ConvertFormat {
        [StringValue("yyyy-MM-dd")]
        Default,
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