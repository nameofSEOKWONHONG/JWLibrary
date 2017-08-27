using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWLibrary.Core.Extensions
{
    public static class NumberStringExtension
    {
        public static string ToFormat(this object val, FormatType type, GetAllow allow)
        {
            if (val.GetType() == typeof(DateTime)) throw new NotSupportedException("DateTime is not support.");
            if (val.GetType() == typeof(float)) throw new NotSupportedException("float is not support.");

            if (type == FormatType.Comma)
                return string.Format("{0:#,###}", val);
            else if (type == FormatType.Rate)
                return string.Format("{0:##.##}", val);
            else if (type == FormatType.Mobile)
            {
                if (allow == GetAllow.Allow)
                {
                    return string.Format("{0}-{1}-{2}", val.ToString().FirstGetByLength(3),
                        val.ToString().MiddleGetByLength(3, 4),
                        val.ToString().LastGetByLength(4));
                }
                else
                {
                    return string.Format("{0}-{1}-****", val.ToString().FirstGetByLength(3),
                        val.ToString().MiddleGetByLength(3, 4));
                }
            }
            else if (type == FormatType.Phone)
            {
                if(allow == GetAllow.Allow)
                {
                    var temp = val.ToString();
                    if (temp.FirstGetByLength(2) == "02")
                    {
                        if(temp.Length == 10)
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
                    var temp = val.ToString();
                    if (temp.FirstGetByLength(2) == "02")
                    {
                        if (temp.Length == 10)
                        {
                            return string.Format("{0}-{1}-****", temp.FirstGetByLength(2),
                                temp.MiddleGetByLength(2, 4));
                        }
                        else
                        {
                            return string.Format("{0}-{1}-****", temp.FirstGetByLength(2),
                                temp.MiddleGetByLength(2, 3));
                        }
                    }
                    else
                    {
                        if (temp.Length == 11)
                        {
                            return string.Format("{0}-{1}-****", temp.FirstGetByLength(3),
                                temp.MiddleGetByLength(3, 4));
                        }
                        else
                        {
                            return string.Format("{0}-{1}-****", temp.FirstGetByLength(3),
                                temp.MiddleGetByLength(3, 3));
                        }
                    }
                }
            }
            else if (type == FormatType.RRN)
            {
                if (allow == GetAllow.Allow)
                {
                    return string.Format("{0}-{1}", val.ToString().FirstGetByLength(6),
                        val.ToString().LastGetByLength(7));
                }
                else
                {
                    return string.Format("{0}-*******", val.ToString().FirstGetByLength(6));
                }
            }
            else if (type == FormatType.CofficePrice)
            {
                return string.Format("{0}.{1}", val.ToString().FirstGetByLength(1),
                    val.ToString().MiddleGetByLength(1, 1));
            }
            else
                throw new NotSupportedException("do not convert value");
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
