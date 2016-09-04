using System;
using System.Drawing;
using System.Text;
using System.Collections.Generic;
using System.IO;

namespace SouNews.Common {
    public class FileHelper {
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="_path">文件路径</param>
        /// <returns></returns>
        public static void FileDelete(string _path) {
            if (FileExists(_path))
                File.Delete(_path);
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <param name="_path"></param>
        /// <returns></returns>
        public static string ReadFile(string _path,Encoding encoding) {
            try {
                return File.ReadAllText(_path,encoding);
            }
            catch { return null; }
        }

        /// <summary>
        /// 返回文件是否存在
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>是否存在</returns>
        public static bool FileExists(string filename) {
            return File.Exists(filename);
        }

        /// <summary>
        /// 文件写入共享锁
        /// </summary>
        protected static object lockobj = new object();
        /// <summary>
        /// 文件写入,如果文件不存在则创建,如何文件存在则追加
        /// </summary>
        /// <param name="log">记录的信息</param>
        /// <param name="filename">文件名</param>
        /// <param name="directory">文件夹路径</param>
        public static void WriteFile(string log,string filename,string directory) {
            lock (lockobj) {
                try {
                    string path = directory + filename;
                    StreamWriter sr;
                    if (!Directory.Exists(directory)) {
                        Directory.CreateDirectory(directory);
                    }
                    //追加日志
                    if (File.Exists(path)) {
                        sr = File.AppendText(path);
                    }
                    //创建日志
                    else {
                        sr = File.CreateText(path);
                    }
                    sr.WriteLine(log);
                    sr.Close();
                }
                catch (Exception) {
                    
                }
            }
        }


        /// <summary>
        /// 替换文件
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="filename">文件名</param>
        /// <param name="directory">文件夹路径</param>
        public static void ReplaceFile(string text,string filename,string directory) {
            lock (lockobj) {
                string path = directory + filename;
                StreamWriter sr;
                if (!Directory.Exists(directory)) {
                    Directory.CreateDirectory(directory);
                }
                sr = File.CreateText(path);
                sr.WriteLine(text);
                sr.Close();
            }
        }

        /// <summary>
        /// base64字符转化成为图片
        /// </summary>
        /// <param name="inputStr"></param>
        /// <param name="txtFileName"></param>
        /// <returns></returns>
        public static bool base64ToFile(string inputStr,string txtFileName) {

            try {
                byte[] arr = Convert.FromBase64String(inputStr);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);

                bmp.Save(txtFileName,System.Drawing.Imaging.ImageFormat.Jpeg);

                ms.Close();

                return true;
            }
            catch {
                return false;
            }
        }

        /// <summary>
        /// 上传图片的存放目录
        /// </summary>
        /// <param name="directory"> 总目录</param>
        /// <param name="token"> </param>
        public static string SetImgUrl(string directory,string token) {

            string imgName = SecurityHelper.MD5(token + DateTime.Now.ToString("YYYYMMDDHIS") + StringHelper.MakeRndNum(6));
            string imgPath = directory + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString()
                             + "/" + DateTime.Now.Day.ToString() + "/";
            if (!Directory.Exists(imgPath)) {
                Directory.CreateDirectory(imgPath);
            }
            return imgPath + imgName + ".jpg";
        }

    }
}
