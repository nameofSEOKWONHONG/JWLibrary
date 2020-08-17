using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.StaticMethod
{
    public static class NumberString
    {
        public static string jToFormatNumber<T>(this T val, FormatType type, GetAllow allow)
        {
            if (val.GetType() == typeof(DateTime)) throw new NotSupportedException("DateTime is not support.");
            if (val.GetType() == typeof(float)) throw new NotSupportedException("float is not support.");

            var result = type switch
            {
                FormatType.Comma => string.Format("{0:#,###}", val),
                FormatType.Rate => string.Format("{0:##.##}", val),
                FormatType.Mobile => allow switch
                    {
                        GetAllow.Allow => string.Format("{0}-{1}-{2}", val.ToString().jFirstGetByLength(3),
                            val.ToString().jMiddleGetByLength(3, 4),
                            val.ToString().jLastGetByLength(4)),
                        _ => string.Format("{0}-{1}-****", val.ToString().jFirstGetByLength(3),
                            val.ToString().jMiddleGetByLength(3, 4)),
                    },
                FormatType.Phone => MakePhoneString(val, allow),
                FormatType.RRN => MakeRRNString(val, allow),
                FormatType.CofficePrice => string.Format("{0}.{1}", val.ToString().jFirstGetByLength(1),
                    val.ToString().jMiddleGetByLength(1, 1)),
                _ => throw new NotSupportedException("do not convert value"),
            };

            return result;
        }

        private static string MakePhoneString<T>(T val, GetAllow allow)
        {
            if (allow == GetAllow.Allow)
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
        private static string MakeRRNString<T>(T val, GetAllow allow)
        {
            if (allow == GetAllow.Allow)
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

    }
    public enum GetAllow
    {
        Allow,
        NotAllow
    }

    public enum FormatType
    {
        Comma,
        Rate,
        Mobile,
        RRN,
        CofficePrice,
        Phone,
    }
}
