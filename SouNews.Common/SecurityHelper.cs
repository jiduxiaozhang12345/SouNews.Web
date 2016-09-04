using System;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Collections;
using System.Text;
using MSScriptControl;

namespace SouNews.Common {
    public class SecurityHelper {

        /// <summary>
        /// 初始化安全类
        /// </summary>
        public SecurityHelper() {
         
        }
   

        #region MD5加密
        /// <summary>
        /// 128位MD5算法加密字符串
        /// </summary>
        /// <param name="text">要加密的字符串</param>    
        public static string MD5(string text) {

            //如果字符串为空，则返回
            if (String.IsNullOrEmpty(text)) {
                return "";
            }
            //返回MD5值的字符串表示
            return MD5( Encoding.UTF8.GetBytes(text));
        }


        #region MD5加密
        /// <summary>
        /// 128位MD5算法加密字符串
        /// </summary>
        /// <param name="text">要加密的字符串</param>    
        public static string MD5(string text, TextEncoding receivEncoding) {

            //如果字符串为空，则返回
            if (String.IsNullOrEmpty(text)) {
                return "";
            }
            //返回MD5值的字符串表示
            return MD5(Encoding.GetEncoding(receivEncoding.ToString()).GetBytes(text));
        }

        /// <summary>
        /// 编码枚举
        /// </summary>
        public enum TextEncoding : int {
            GBK,
            GB2312
        }
       

        /// <summary>
        /// 128位MD5算法加密Byte数组
        /// </summary>
        /// <param name="data">要加密的Byte数组</param>    
        public static string MD5(byte[] data) {


            try {
                //创建MD5密码服务提供程序
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

                //计算传入的字节数组的哈希值
                byte[] result = md5.ComputeHash(data);

                //释放资源
                md5.Clear();

                //返回MD5值的字符串表示
                return BitConverter.ToString(result).Replace("-", "");
            } catch {

                //LogHelper.WriteTraceLog(TraceLogLevel.Error, ex.Message);
                return "";
            }
        }
        #endregion

        #region Base64加密
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="text">要加密的字符串</param>
        /// <returns></returns>
        public static string EncodeBase64(string text) {
            //如果字符串为空，则返回
            if (String.IsNullOrEmpty(text)) {
                return "";
            }

            try {
                char[] Base64Code = new char[]{'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T',
											'U','V','W','X','Y','Z','a','b','c','d','e','f','g','h','i','j','k','l','m','n',
											'o','p','q','r','s','t','u','v','w','x','y','z','0','1','2','3','4','5','6','7',
											'8','9','+','/','='};
                byte empty = (byte)0;
                ArrayList byteMessage = new ArrayList(Encoding.Default.GetBytes(text));
                StringBuilder outmessage;
                int messageLen = byteMessage.Count;
                int page = messageLen / 3;
                int use = 0;
                if ((use = messageLen % 3) > 0) {
                    for (int i = 0; i < 3 - use; i++)
                        byteMessage.Add(empty);
                    page++;
                }
                outmessage = new System.Text.StringBuilder(page * 4);
                for (int i = 0; i < page; i++) {
                    byte[] instr = new byte[3];
                    instr[0] = (byte)byteMessage[i * 3];
                    instr[1] = (byte)byteMessage[i * 3 + 1];
                    instr[2] = (byte)byteMessage[i * 3 + 2];
                    int[] outstr = new int[4];
                    outstr[0] = instr[0] >> 2;
                    outstr[1] = ((instr[0] & 0x03) << 4) ^ (instr[1] >> 4);
                    if (!instr[1].Equals(empty))
                        outstr[2] = ((instr[1] & 0x0f) << 2) ^ (instr[2] >> 6);
                    else
                        outstr[2] = 64;
                    if (!instr[2].Equals(empty))
                        outstr[3] = (instr[2] & 0x3f);
                    else
                        outstr[3] = 64;
                    outmessage.Append(Base64Code[outstr[0]]);
                    outmessage.Append(Base64Code[outstr[1]]);
                    outmessage.Append(Base64Code[outstr[2]]);
                    outmessage.Append(Base64Code[outstr[3]]);
                }
                return outmessage.ToString();
            } catch (Exception ex) {
                throw ex;
            }
        }
        #endregion

     
        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="text">要解密的字符串</param>
        public static string DecodeBase64(string text) {
            //如果字符串为空，则返回
            if (String.IsNullOrEmpty(text)) {
                return "";
            }
            //将空格替换为加号
            text = text.Replace(" ", "+");

            try {
                if ((text.Length % 4) != 0) {
                    return "包含不正确的BASE64编码";
                }
                if (!Regex.IsMatch(text, "^[A-Z0-9/+=]*$", RegexOptions.IgnoreCase)) {
                    return "包含不正确的BASE64编码";
                }
                string Base64Code = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
                int page = text.Length / 4;
                ArrayList outMessage = new ArrayList(page * 3);
                char[] message = text.ToCharArray();
                for (int i = 0; i < page; i++) {
                    byte[] instr = new byte[4];
                    instr[0] = (byte)Base64Code.IndexOf(message[i * 4]);
                    instr[1] = (byte)Base64Code.IndexOf(message[i * 4 + 1]);
                    instr[2] = (byte)Base64Code.IndexOf(message[i * 4 + 2]);
                    instr[3] = (byte)Base64Code.IndexOf(message[i * 4 + 3]);
                    byte[] outstr = new byte[3];
                    outstr[0] = (byte)((instr[0] << 2) ^ ((instr[1] & 0x30) >> 4));
                    if (instr[2] != 64) {
                        outstr[1] = (byte)((instr[1] << 4) ^ ((instr[2] & 0x3c) >> 2));
                    } else {
                        outstr[2] = 0;
                    }
                    if (instr[3] != 64) {
                        outstr[2] = (byte)((instr[2] << 6) ^ instr[3]);
                    } else {
                        outstr[2] = 0;
                    }
                    outMessage.Add(outstr[0]);
                    if (outstr[1] != 0)
                        outMessage.Add(outstr[1]);
                    if (outstr[2] != 0)
                        outMessage.Add(outstr[2]);
                }
                byte[] outbyte = (byte[])outMessage.ToArray(Type.GetType("System.Byte"));
                return Encoding.Default.GetString(outbyte);
            } catch (Exception ex) {
                throw ex;
            }
        }


        /// <summary>
        /// des 加密
        /// </summary>
        /// <param name="inpuStr"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string DesEncode(string inpuStr, string key, string jsPath) {
            return DesCode(inpuStr, key, "desencode", jsPath);
        }

        /// <summary>
        /// des 解密
        /// </summary>
        /// <param name="inpuStr"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string DesDecode(string inpuStr, string key, string jsPath) {
            try {
                return DesCode(inpuStr,key,"desdecode",jsPath);
            }
            catch {
                return string.Empty;
            }
        }

        /// <summary>
        /// 这个临时处理方案本来不应该在这里
        /// </summary>
        /// <param name="inputStr"></param>
        /// <param name="key"></param>
        /// <param name="codeType"></param>
        /// <returns></returns>
        private static string DesCode(string inputStr, string key, string codeType, string jsPath) {
            MSScriptControl.ScriptControl js = new ScriptControl();
            js.AllowUI = false;
            js.Language = "JScript";
            js.Reset();
            //Server.MapPath("/static/DES.js");
            string jsCode = FileHelper.ReadFile(jsPath, System.Text.Encoding.UTF8);
            js.AddCode(jsCode);
           string result = js.Eval(String.Format("{0}('{1}', '{2}')",
                                                      codeType, inputStr, key));
            
            return result;
        }


   


        #endregion

    }
}
