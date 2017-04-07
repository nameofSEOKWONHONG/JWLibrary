using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.Core.Date
{
    public static class DateHelper
    {
        public static string ConvertDateFormat(this DateTime date, DateHelperFormat format)
        {
            switch (format)
            {
                case DateHelperFormat.YYYY_MM_DD:
                    return date.ToString("yyyy-MM-dd");
                case DateHelperFormat.YYYYMMDD:
                    return date.ToString("yyyyMMdd");
                case DateHelperFormat.HHMMSS:
                    return date.ToString("HHmmss");
                case DateHelperFormat.YYYY_S_MM_S_DD:
                    return date.ToString("yyyy/MM/dd");
                case DateHelperFormat.YYYYMMDDHHMMSS:
                    return date.ToString("yyyyMMddHHmmss");
                default:
                    return date.ToLongDateString();
            }
        }
    }

    public enum DateHelperFormat
    {
        YYYY_MM_DD,
        YYYYMMDD,
        YYYY_S_MM_S_DD,
        YYYYMMDDHHMMSS,
        HHMMSS,
    }
}
