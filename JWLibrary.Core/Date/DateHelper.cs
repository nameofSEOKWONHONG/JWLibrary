using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.Core.Date
{
    public static class DateHelper
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
    }
}
