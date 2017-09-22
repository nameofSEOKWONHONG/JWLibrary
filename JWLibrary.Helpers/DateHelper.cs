using System;
using System.Collections.Generic;
using System.Text;

namespace JWLibrary.Helpers
{
    public static class DateHelper
    {
        public static DateTime ToDateTime(this string date)
        {
            DateTime dt = DateTime.MinValue;

            DateTime.TryParse(date, out dt);

            return dt;
        }

        public static string ToString(this DateTime date, DateFormatType type)
        {
            switch(type)
            {
                case DateFormatType.YYYYMMDD:
                    return date.ToString("yyyyMMdd");
                case DateFormatType.YYYY_D_MM_D_DD:
                    return date.ToString("yyyy.MM.dd");
                case DateFormatType.YYYY_S_MM_S_DD:
                    return date.ToString("yyyy/MM/dd");
                case DateFormatType.YYYYMMDDHHMMSS:
                    return date.ToString("yyyyMMddHHmmss");
                case DateFormatType.YYYYMMDD_HHMMSS:
                    return date.ToString("yyyyMMdd HHmmss");
                case DateFormatType.YYYY_H_MM_H_DD:
                    return date.ToString("yyyy-MM-dd");
                case DateFormatType.ORIGIN_DATE:
                    return date.ToString();
            }

            return string.Empty;
        }


        public enum DateFormatType
        {
            YYYYMMDD,
            YYYY_D_MM_D_DD,
            YYYY_S_MM_S_DD,
            YYYYMMDDHHMMSS,
            YYYYMMDD_HHMMSS,
            YYYY_H_MM_H_DD,
            ORIGIN_DATE
        }



    }
}
