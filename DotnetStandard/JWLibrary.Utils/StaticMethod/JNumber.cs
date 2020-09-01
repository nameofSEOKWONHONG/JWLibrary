using JWLibrary.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JWLibrary.StaticMethod
{
    public static class JNumber
    {
        public static string jToFormatNumber<T>(this T val, ENUM_NUMBER_FORMAT_TYPE type, ENUM_GET_ALLOW_TYPE allow)
        {
            if (val.GetType() == typeof(DateTime)) throw new NotSupportedException("DateTime is not support.");
            if (val.GetType() == typeof(float)) throw new NotSupportedException("float is not support.");

            var result = type switch
            {
                ENUM_NUMBER_FORMAT_TYPE.Comma => string.Format("{0:#,###}", val),
                ENUM_NUMBER_FORMAT_TYPE.Rate => string.Format("{0:##.##}", val),
                ENUM_NUMBER_FORMAT_TYPE.Mobile => allow switch
                    {
                        ENUM_GET_ALLOW_TYPE.Allow => string.Format("{0}-{1}-{2}", val.ToString().jFirstGetByLength(3),
                            val.ToString().jMiddleGetByLength(3, 4),
                            val.ToString().jLastGetByLength(4)),
                        _ => string.Format("{0}-{1}-****", val.ToString().jFirstGetByLength(3),
                            val.ToString().jMiddleGetByLength(3, 4)),
                    },
                ENUM_NUMBER_FORMAT_TYPE.Phone => MakePhoneString(val, allow),
                ENUM_NUMBER_FORMAT_TYPE.RRN => MakeRRNString(val, allow),
                ENUM_NUMBER_FORMAT_TYPE.CofficePrice => string.Format("{0}.{1}", val.ToString().jFirstGetByLength(1),
                    val.ToString().jMiddleGetByLength(1, 1)),
                _ => throw new NotSupportedException("do not convert value"),
            };

            return result;
        }

        private static string MakePhoneString<T>(T val, ENUM_GET_ALLOW_TYPE allow)
        {
            if (allow == ENUM_GET_ALLOW_TYPE.Allow)
            {
                var temp = val.ToString();
                if (temp.jFirstGetByLength(2) == "02")
                {
                    if (temp.Length == 10)
                    {
                        return string.Format("{0}-{1}-{2}", temp.jFirstGetByLength(2),
                            temp.jMiddleGetByLength(2, 4),
                            temp.jLastGetByLength(4));
                    }
                    else
                    {
                        return string.Format("{0}-{1}-{2}", temp.jFirstGetByLength(2),
                            temp.jMiddleGetByLength(2, 3),
                            temp.jLastGetByLength(4));
                    }
                }
                else
                {
                    if (temp.Length == 11)
                    {
                        return string.Format("{0}-{1}-{2}", temp.jFirstGetByLength(3),
                            temp.jMiddleGetByLength(3, 4),
                            temp.jLastGetByLength(4));
                    }
                    else
                    {
                        return string.Format("{0}-{1}-{2}", temp.jFirstGetByLength(3),
                            temp.jMiddleGetByLength(3, 3),
                            temp.jLastGetByLength(4));
                    }
                }
            }
            else
            {
                var temp = val.ToString();
                if (temp.jFirstGetByLength(2) == "02")
                {
                    if (temp.Length == 10)
                    {
                        return string.Format("{0}-{1}-****", temp.jFirstGetByLength(2),
                            temp.jMiddleGetByLength(2, 4));
                    }
                    else
                    {
                        return string.Format("{0}-{1}-****", temp.jFirstGetByLength(2),
                            temp.jMiddleGetByLength(2, 3));
                    }
                }
                else
                {
                    if (temp.Length == 11)
                    {
                        return string.Format("{0}-{1}-****", temp.jFirstGetByLength(3),
                            temp.jMiddleGetByLength(3, 4));
                    }
                    else
                    {
                        return string.Format("{0}-{1}-****", temp.jFirstGetByLength(3),
                            temp.jMiddleGetByLength(3, 3));
                    }
                }
            }
        }
        private static string MakeRRNString<T>(T val, ENUM_GET_ALLOW_TYPE allow)
        {
            if (allow == ENUM_GET_ALLOW_TYPE.Allow)
            {
                return string.Format("{0}-{1}", val.ToString().jFirstGetByLength(6),
                    val.ToString().jLastGetByLength(7));
            }
            else
            {
                return string.Format("{0}-*******", val.ToString().jFirstGetByLength(6));
            }
        }


        public static string jMiddleGetByLength(this string value, int fromLen, int getLen)
        {
            return value.Substring(fromLen, getLen);
        }

        public static string jFirstGetByLength(this string value, int length)
        {
            return value.Substring(0, length);
        }

        public static string jLastGetByLength(this string value, int length)
        {
            return value.Substring(value.Length - length, length);
        }

        public static bool jIsNumber(this string str) {
            str = str.jIfNullOrEmpty(x => string.Empty);
            var regex = new Regex("^[0-9]*$", RegexOptions.ExplicitCapture | RegexOptions.Compiled);
            return regex.Match(str).Success;
        }

        public static bool jIsAlphabetOnly(this string str) {
            str = str.jIfNullOrEmpty(x => string.Empty);
            var regex = new Regex(@"^[a-zA-Z\-_]+$", RegexOptions.ExplicitCapture | RegexOptions.Compiled);
            return regex.Match(str).Success;
        }

        public static bool jIsAlphabetAndNumber(this string str) {
            str = str.jIfNullOrEmpty(x => string.Empty);
            var regex = new Regex(@"^[a-zA-Z0-9]+$", RegexOptions.ExplicitCapture | RegexOptions.Compiled);
            return regex.Match(str).Success;
        }

        public static bool jIsNumeric(this string str) {
            str = str.jIfNullOrEmpty(x => string.Empty);
            var regex = new Regex(@"^(?<digit>-?\d+)(\.(?<scale>\d*))?$", RegexOptions.ExplicitCapture | RegexOptions.Compiled);
            return regex.Match(str).Success;
        }
    }
    public enum ENUM_GET_ALLOW_TYPE
    {
        Allow,
        NotAllow
    }

    public enum ENUM_NUMBER_FORMAT_TYPE
    {
        Comma,
        Rate,
        Mobile,
        RRN,
        CofficePrice,
        Phone,
    }
}
