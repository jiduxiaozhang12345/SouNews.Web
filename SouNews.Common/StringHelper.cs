using System;
using System.Configuration;
using System.Text;
using System.Web;

namespace SouNews.Common {
    public class StringHelper {

        /// <summary>
        /// 随机生成固定长度字符串
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public static string MakeRndNum(int codeCount) {
            int rep = 0;
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> rep)));
            for (int i = 0 ; i < codeCount ; i++) {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0) {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }

        /// <summary>
        /// 随机生成固定长度数字
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public static string MakeRndNumber(int codeCount) {
            int rep = 0;
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + rep;
            rep++;
            Random random = new Random();
            for (int i = 0 ; i < codeCount ; i++) {
                int num = random.Next(10);
                str = str + num.ToString();
            }
            return str;
        }


        /// <summary>
        /// 字符串首字母大写
        /// </summary>
        /// <returns>返回首字母大写后的字符串</returns>
        public static string TitleUpper(string str) {
            if (str.ToEString() == "")
                return "";
            string firstStr = str.Substring(0,1).ToUpper();
            return firstStr + str.Substring(1,str.Length - 1);
        }

        public static string UrlEncoderUp(string content,Encoding e) {
            if (string.IsNullOrEmpty(content)) {
                return string.Empty;
            }
            StringBuilder builder = new StringBuilder();
            foreach (char c in content) {
                if (HttpUtility.UrlEncode(c.ToString(),e).Length > 1) {
                    builder.Append(HttpUtility.UrlEncode(c.ToString()).ToUpper());
                }
                else {
                    builder.Append(c);
                }
            }
            return builder.ToString();
        }


        /// <summary>
        /// 将IPv4格式的字符串转换为int型表示
        /// </summary>
        /// <param name="strIPAddress">IPv4格式的字符</param>
        /// <returns></returns>
        public static long IPToNumber(string strIPAddress) {
            //将目标IP地址字符串strIPAddress转换为数字
            string[] arrayIP = strIPAddress.Split('.');
            long sip1 = Int32.Parse(arrayIP[0]);
            long sip2 = Int32.Parse(arrayIP[1]);
            long sip3 = Int32.Parse(arrayIP[2]);
            long sip4 = Int32.Parse(arrayIP[3]);
            long tmpIpNumber;
            tmpIpNumber = sip1 * 256 * 256 * 256 + sip2 * 256 * 256 + sip3 * 256 + sip4;
            return tmpIpNumber;
        }


        /// <summary>
        /// 将int型表示的IP还原成正常IPv4格式。
        /// </summary>
        /// <param name="intIPAddress">int型表示的IP</param>
        /// <returns></returns>
        public static string NumberToIP(int intIPAddress) {
            int tempIPAddress;
            //将目标整形数字intIPAddress转换为IP地址字符串
            //-1062731518 192.168.1.2 
            //-1062731517 192.168.1.3 
            if (intIPAddress >= 0) {
                tempIPAddress = intIPAddress;
            } else {
                tempIPAddress = intIPAddress + 1;
            }
            int s1 = tempIPAddress / 256 / 256 / 256;
            int s21 = s1 * 256 * 256 * 256;
            int s2 = (tempIPAddress - s21) / 256 / 256;
            int s31 = s2 * 256 * 256 + s21;
            int s3 = (tempIPAddress - s31) / 256;
            int s4 = tempIPAddress - s3 * 256 - s31;
            if (intIPAddress < 0) {
                s1 = 255 + s1;
                s2 = 255 + s2;
                s3 = 255 + s3;
                s4 = 255 + s4;
            }
            string strIPAddress = s1.ToString() + "." + s2.ToString() + "." + s3.ToString() + "." + s4.ToString();
            return strIPAddress;
        }

    }
}
