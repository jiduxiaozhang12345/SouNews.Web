using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace SouNews.Common
{
    public class EncodingHelper
    {
        /// <summary>
        /// MD5加密，输出32位加密串  转换成大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MD5Encrypt(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encryptedBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(str));
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < encryptedBytes.Length; i++)
            {
                sb.AppendFormat("{0:x2}", encryptedBytes[i]);
            }
            return sb.ToString().ToUpper();
        }

        /// <summary>
        /// 将字符转换为Ascii码
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static int CharToAscii(char c)
        {
            byte[] array = Encoding.ASCII.GetBytes(new char[] { c }, 0, 1);
            int asciicode = (short)(array[0]);
            return asciicode;
        }

        /// <summary>
        /// 将Ascii码转换为字符
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static char AaciiToChar(int a)
        {
            byte[] array = new byte[] { (byte)(a) };
            char[] charArray = Encoding.ASCII.GetChars(array);
            return charArray[0];
        }

        /// <summary>
        /// 重复字符
        /// </summary>
        /// <param name="c">字符</param>
        /// <param name="count">重复次数</param>
        /// <returns></returns>
        public static string RepeatChar(char c, int count)
        {
            string str = "";
            for (int i = 0; i < count; i++)
            {
                str += c;
            }

            return str;
        }
    }
}