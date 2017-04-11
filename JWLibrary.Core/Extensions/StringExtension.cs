using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JWLibrary.Core.Extensions
{
    public static class StringExtension
    {
        public static string ToMaskedString(this string value)
        {
            var pattern = "^(/d{3})(/d{4})(/d*)$";
            var regExp = new Regex(pattern);
            return regExp.Replace(value, "$1-$2-$3");
        }

        public static string ToRegNoMaskedString(this string value)
        {
            if (value.Contains("-"))
            {
                return string.Format("{0}-{1}", value.FirstGetByLength(6), "*******");
            }

            return string.Format("{0}{1}", value.FirstGetByLength(6), "*******");
        }

        public static string ToMobileMaskedString(this string value)
        {
            if (value.Contains("-"))
            {
                return string.Format("{0}-{1}-{2}", value.FirstGetByLength(3), value.MiddleGetByLength(5, 3), "***");
            }

            return string.Format("{0}{1}{2}", value.FirstGetByLength(3), value.MiddleGetByLength(4, 3), "***");
        }

        public static string ToPhoneMaskedString(this string value)
        {
            var temp = value.Split('-');
            var ret = string.Empty;
            var cnt = 0;

            if (value.Contains("-"))
            {
                foreach(var item in temp)
                {
                    if(cnt > 1)
                    ret += "-****";
                    cnt++;
                }

                return ret;
            }

            foreach (var item in temp)
            {
                if (cnt > 1)
                    ret += "****";
                cnt++;
            }

            return ret;
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

    }
}
