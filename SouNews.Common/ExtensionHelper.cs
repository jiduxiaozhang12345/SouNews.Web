using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;
using System.Linq;
using System.Data;

namespace SouNews.Common {
    /// <summary>
    /// 专门处理扩张方法类
    /// </summary>
    public static class ExtensionHelper {

        /// <summary>
        /// 扩张方法 转换字符串默认为 ""
        /// </summary>
        /// <returns></returns>
        public static string ToEString(this object s) {
            if (s == null) {
                return "";
            } else {
                return s.ToString();
            }
        }

        /// <summary>
        /// 扩张方法 转换字符串默认为 ""
        /// </summary>
        /// <returns></returns>
        public static string ToEString(this object s ,string defaultStr) {
            if (s == null) {
                return defaultStr;
            } else {
                return s.ToString();
            }
        }

        /// <summary>
        /// 字符串截取从第一位截取
        /// </summary>
        /// <returns></returns>
        public static string ESubStart(this object s ,int length) {
            string temp = s.ToEString();
            if (temp.Length <= length) {
                return temp;
            } else {
                return temp.Substring(0 ,length);
            }
        }

        /// <summary>
        /// 字符串截取从第一位截取
        /// </summary>
        /// <returns></returns>
        public static string ESubEnd(this object s ,int length) {
            string temp = s.ToEString();
            if (temp.Length <= length) {
                return temp;
            } else {
                return temp.Substring(temp.Length - length ,length);
            }
        }

        /// <summary>
        /// 判断字符串或对象是否为空或null
        /// </summary>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this object s) {
            if (s == null) {
                return true;
            } else {
                return string.IsNullOrEmpty(s.ToEString());
            }
        }

        /// <summary>
        /// 给前端展示的保留小数用
        /// </summary>
        /// <returns></returns>
        public static string ToMoney(this decimal? s) {
            if (s == null) {
                return Decimal.Parse("0").ToString("f2");
            } else {
                return Decimal.Parse(s.ToEString()).ToString("f2");
            }
        }

        /// <summary>
        /// 给前端展示的保留小数用
        /// </summary>
        /// <returns></returns>
        public static string ToMoney(this decimal s) {
            return s.ToString("f2");
        }

        /// <summary>
        /// 转换 int  默认为 0
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static int ToEInt(this object t) {
            if (t == null) {
                return 0;
            } else {
                try {
                    return Convert.ToInt32(t);
                } catch {
                    return 0;
                }

            }
        }

        public static long ToLong(this object t) {
            if (t == null) {
                return 0;
            } else {
                try {
                    return Convert.ToInt64(t);
                } catch {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 转换 Short  默认为 0
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static short ToEShort(this object t) {
            if (t == null) {
                return 0;
            } else {
                return Convert.ToInt16(t);
            }
        }

        /// <summary>
        /// 转换 byte  默认为 0
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static byte ToEByte(this object t) {
            if (t == null) {
                return 0;
            } else {
                return Convert.ToByte(t.ToString());
            }
        }

        /// <summary>
        /// 转换 DateTime  默认为 DateTime.MinValue
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static DateTime ToEDate(this object t) {
            if (t == null) {
                return DateTime.MinValue;
            } else {
                return DateTime.Parse(t.ToEString());
            }
        }

        /// <summary>
        /// 转换 DateTime  默认为 DateTime.MinValue
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ToEShortDate(this object t) {
            if (t == null) {
                return DateTime.MinValue.ToString("yyyy-MM-dd");
            } else {
                return DateTime.Parse(t.ToEString()).ToString("yyyy-MM-dd");
            }
        }


        /// <summary>
        /// 转换 DateTime  默认为 DateTime.MinValue
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static DateTime ToEDateFormat(this DateTime? t) {
            return DateTime.Parse(t.ToEDate().ToString("yyyy-MM-dd HH:mm:ss"));
        }

        /// <summary>
        /// 转换 Decimal  默认为 0;
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Decimal ToEDecimal(this object t) {
            if (t == null) {
                return 0;
            } else {
                try {
                    return decimal.Parse(t.ToEString());
                } catch {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 转换 Double  默认为 0;
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Double ToEDouble(this object t) {
            if (t == null) {
                return 0;
            } else {
                try {
                    return double.Parse(t.ToEString());
                } catch {
                    return 0;
                }
            }
        }


        /// <summary>
        /// 转换 MD5加密  默认为 0;
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ToEMD5(this object t) {
            if (t == null) {
                return null;
            } else {
                string input = t.ToEString();
                return EncodingHelper.MD5Encrypt(input).ToUpper();
            }
        }

        /// <summary>  
        /// 非法字符转换  
        /// </summary>  
        /// <param name="str"></param>  
        /// <returns></returns>  
        public static bool IsHasCheckVar(this object t) {
            string input = t.ToEString().Trim();
            string checkvar = "｀＂；：％☆？！〈（＆＃";
            foreach (char c in checkvar) {
                if (input.Contains(c))
                    return true;
            }
            return false;
        }

        /// <summary>  
        /// 清除短信商认定的敏感词 
        /// 说明: 不同短信商可能规则不同，更换短信商时，这里可以对应调整。
        /// </summary>  
        /// <param name="str"></param>  
        /// <returns></returns>  
        public static string FilterSensitiveWords(this object t) {
            List<string> SensitiveWordsForReplace = new List<string>() { "移动" ,"独立" ,"礼" ,"特价" ,"上市" ,"居" ,"享受" ,"制服" ,"骗子" ,"生肖" ,"屌" ,"咪咪" ,"骚" ,"师傅" ,"【" ,"】" };
            List<string> SensitiveWordsForSeprate = new List<string>() { "台湾" ,"代理" ,"大师" ,"广告" ,"黄色" ,"丝袜" ,"色男" ,"SB" ,"茉莉花" ,"花花公子" ,"发票" ,"三点" };
            string input = t.ToEString().Trim();

            foreach (string r in SensitiveWordsForReplace) {
                input = input.Replace(r ,"");
            }
            foreach (string s in SensitiveWordsForReplace) {
                var news = s.ToList();
                news.Insert(1 ,' ');
                input = input.Replace(s ,string.Join(" " ,news.ToArray()));
            }
            return input;
        }

        /// <summary>
        /// 枚举转 ArrayList
        /// </summary>
        /// <param name="enumType"> typeof(枚举)</param>
        /// <returns></returns>
        public static IList ToList(this Type enumType) {
            ArrayList list = new ArrayList();
            foreach (int item in Enum.GetValues(enumType)) {
                list.Add(Enum.GetName(enumType ,item));
            }
            return list;
        }

        /// <summary>
        /// 枚举转  Dictionary
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static Dictionary<string ,string> ToDictionary(this Type enumType) {
            Dictionary<string ,string> list = new Dictionary<string ,string>();
            foreach (int item in Enum.GetValues(enumType)) {
                list.Add(Enum.GetName(enumType ,item) ,item.ToEString());
            }
            return list;
        }

        /// <summary>
        /// 扩张方法 转换字符串默认为 ""
        /// </summary>
        /// <returns></returns>
        public static string ToMaskString(this object s) {
            string str = s.ToEString();
            if (string.IsNullOrEmpty(str)) {
                return "";
            } else {

                if (RegularValidateHelper.IsMobilePhoneNumber(str) && str.Length == 11) {
                    return str.Substring(0 ,3) + "****" + str.Substring(7 ,4);
                } else {
                    if (str.Length == 2) {
                        return str.Substring(0 ,1) + GetMask(1);
                    }
                    if (str.Length > 2) {

                        return str.Substring(0 ,1) + GetMask(str.Length - 2) + str.Substring(str.Length - 1 ,1);
                    }
                }
                return s.ToString();
            }
        }

        private static string GetMask(int length) {
            string str = "";
            for (int i = 0 ; i < length ; i++) {
                str += "*";
            }
            return str;
        }

        /// <summary>
        /// 验证是否特殊用户【内部用户】
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static bool SpecialUser(string username) {
            var ret = false;
            var regStr = @"^(13[6-9]0000\d{4}|[a-z]+)$";
            if (RegularValidateHelper.IsMatch(username ,regStr)) {
                ret = true;
            }
            return ret;
        }
    }

}
