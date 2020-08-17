using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.Core.Extensions
{
    public static class DateStringExtension
    {
        public static string ConvertDateToString(this DateTime date, ConvertFormat format)
        {
            switch (format)
            {
                case ConvertFormat.YYYY_MM_DD:
                    return date.ToString("yyyy-MM-dd");
                case ConvertFormat.YYYYMMDD:
                    return date.ToString("yyyyMMdd");
                case ConvertFormat.HHMMSS:
                    return date.ToString("HHmmss");
                case ConvertFormat.YYYY_S_MM_S_DD:
                    return date.ToString("yyyy/MM/dd");
                case ConvertFormat.YYYYMMDDHHMMSS:
                    return date.ToString("yyyyMMddHHmmss");
                case ConvertFormat.YYYY_DOT_MM_DOT_DD:
                    return date.ToString("yyy.MM.dd");
                default:
                    return date.ToLongDateString();
            }
        }
    }

    public enum ConvertFormat
    {
        YYYY_MM_DD,
        YYYYMMDD,
        YYYY_S_MM_S_DD,
        YYYYMMDDHHMMSS,
        HHMMSS,
        YYYY_DOT_MM_DOT_DD
    }
}
