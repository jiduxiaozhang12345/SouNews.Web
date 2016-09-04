using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Drawing;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Cryptography.X509Certificates;
using System.Drawing.Imaging;
using System.Threading;
using System.Threading.Tasks;

namespace SouNews.Common {
    public class HttpHelper {

        public int timeout = 10000;
        /// <summary>
        /// 获取网页图片
        /// </summary>
        /// <param name="url">图片地址</param>
        /// <param name="method">POST,GET</param>
        /// <param name="cookie">CookieContainer</param>
        /// <returns>采集的图片</returns>
        public Image CollectImage(string url, Method method, ref CookieContainer cookie) {
            Image img = null;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream stream = null;
            Stream outStream = null;
            try {
                ServicePointManager.Expect100Continue = false;
                request = (HttpWebRequest)WebRequest.Create(url);
                request.AllowAutoRedirect = true;
                request.CookieContainer = cookie;
                request.Method = method.ToString();
                response = (HttpWebResponse)request.GetResponse();
                stream = response.GetResponseStream();
                img = Image.FromStream(stream);
            }
            catch (Exception) {
                //  FileHelper.WriteFile(ex.ToString(), DateTime.Now.ToShortDateString() + ".txt", Environment.CurrentDirectory + "\\CollectException\\");
            }
            finally {
                if (response != null) {
                    response.Close();
                }
                if (stream != null) {
                    stream.Dispose();
                }
                if (outStream != null) {
                    outStream.Close();
                }
                if (response != null) {
                    request.Abort();
                }
            }
            return img;
        }


        /// <summary>
        /// 获取网页图片
        /// </summary>
        /// <param name="url">图片地址</param>
        /// <param name="method">POST,GET</param>
        /// <returns>采集的图片</returns>
        public Image CollectImage(string url, Method method) {
            Image img = null;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream stream = null;
            Stream outStream = null;
            try {
                ServicePointManager.Expect100Continue = false;
                request = (HttpWebRequest)WebRequest.Create(url);
                request.AllowAutoRedirect = true;
                request.Method = method.ToString();
                response = (HttpWebResponse)request.GetResponse();
                stream = response.GetResponseStream();
                img = Image.FromStream(stream);
            }
            catch (Exception) {
                //  FileHelper.WriteFile(ex.ToString(), DateTime.Now.ToShortDateString() + ".txt", Environment.CurrentDirectory + "\\CollectException\\");
            }
            finally {
                if (response != null) {
                    response.Close();
                }
                if (stream != null) {
                    stream.Dispose();
                }
                if (outStream != null) {
                    outStream.Close();
                }
                if (response != null) {
                    request.Abort();
                }
            }
            return img;
        }


        /// <summary>
        /// 保存网页采集图片
        /// </summary>
        /// <param name="url">图片地址</param>
        /// <param name="method">POST,GET</param>
        /// <param name="savepath">保存路径</param>
        /// <param name="cookie">CookieContainer</param>
        /// <returns>是否采集成功</returns>
        public bool CollectImageBySave(string url, Method method, string filename, string directory, ref CookieContainer cookie) {
            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }
            string savepath = directory + filename;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream stream = null;
            Stream outStream = null;
            try {

                ServicePointManager.Expect100Continue = false;
                request = (HttpWebRequest)WebRequest.Create(url);
                request.AllowAutoRedirect = true;
                request.CookieContainer = cookie;
                request.Method = method.ToString();
                response = (HttpWebResponse)request.GetResponse();
                stream = response.GetResponseStream();
                if (File.Exists(savepath))
                    File.Delete(savepath);
                byte[] buffer = new byte[1024 * 1024];
                outStream = File.Create(savepath);
                int l;
                do {
                    l = stream.Read(buffer, 0, buffer.Length);
                    if (l > 0)
                        outStream.Write(buffer, 0, l);
                }
                while (l > 0);
                return true;
            }
            catch (Exception) {

                return false;
            }
            finally {

                if (response != null) {
                    response.Close();
                }
                if (stream != null) {
                    stream.Dispose();
                }
                if (outStream != null) {
                    outStream.Close();
                }
                if (request != null) {
                    request.Abort();
                }
            }
        }


        /// <summary>
        /// 保存网页采集图片
        /// </summary>
        /// <param name="url">图片地址</param>
        /// <param name="method">POST,GET</param>
        /// <param name="savepath">保存路径</param>
        /// <param name="cookie">CookieContainer</param>
        /// <returns>是否采集成功</returns>
        public bool CollectImageBySaveBmp(string url, Method method, string filename, string directory, ref CookieContainer cookie) {
            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }
            string savepath = directory + filename;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream stream = null;
            try {
                //ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
                ServicePointManager.Expect100Continue = false;
                request = (HttpWebRequest)WebRequest.Create(url);
                request.AllowAutoRedirect = true;
                request.CookieContainer = cookie;
                request.Method = method.ToString();
                response = (HttpWebResponse)request.GetResponse();
                stream = response.GetResponseStream();
                if (File.Exists(savepath))
                    File.Delete(savepath);
                Bitmap b = new Bitmap(stream);
                b.Save(savepath, ImageFormat.Bmp);
                return true;
            }
            catch (Exception) {
                // FileHelper.WriteFile(ex.ToString(), DateTime.Now.ToShortDateString() + ".txt", Environment.CurrentDirectory + "\\CollectException\\");
                return false;
            }
            finally {

                if (response != null) {
                    response.Close();
                }
                if (stream != null) {
                    stream.Dispose();
                }
                if (request != null) {
                    request.Abort();
                }
            }
        }


        /// <summary>
        /// 保存网页采集图片
        /// </summary>
        /// <param name="url">图片地址</param>
        /// <param name="method">POST,GET</param>
        /// <param name="savepath">保存路径</param>
        /// <returns>是否采集成功</returns>
        public bool CollectImageBySave(string url, Method method, string filename, string directory) {
            if (!Directory.Exists(directory)) {
                Directory.CreateDirectory(directory);
            }
            string savepath = directory + filename;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream stream = null;
            Stream outStream = null;
            try {
                ServicePointManager.Expect100Continue = false;
                request = (HttpWebRequest)WebRequest.Create(url);
                request.AllowAutoRedirect = true;
                request.Method = method.ToString();
                response = (HttpWebResponse)request.GetResponse();
                stream = response.GetResponseStream();
                if (File.Exists(savepath))
                    File.Delete(savepath);
                byte[] buffer = new byte[1024];
                outStream = File.Create(savepath);
                int l;
                do {
                    l = stream.Read(buffer, 0, buffer.Length);
                    if (l > 0)
                        outStream.Write(buffer, 0, l);
                }
                while (l > 0);
                return true;
            }
            catch (Exception) {
                //FileHelper.WriteFile(ex.ToString(), DateTime.Now.ToShortDateString() + ".txt", Environment.CurrentDirectory + "\\CollectException\\");
                return false;
            }
            finally {

                if (response != null) {
                    response.Close();
                }
                if (stream != null) {
                    stream.Dispose();
                }
                if (outStream != null) {
                    outStream.Close();
                }
                if (request != null) {
                    request.Abort();
                }
            }
        }


        /// <summary>
        /// 采集文件流
        /// </summary>
        /// <param name="url">采集地址</param>
        /// <param name="method">POST,GET</param>
        /// <param name="cookie">CookieContainer</param>
        /// <returns>采集的文件流</returns>
        public Stream CollectStream(string url, Method method, ref CookieContainer cookie) {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream stream = null;
            Stream outStream = null;
            try {
                ServicePointManager.Expect100Continue = false;
                request = (HttpWebRequest)WebRequest.Create(url);
                request.AllowAutoRedirect = true;
                request.CookieContainer = cookie;
                request.Method = method.ToString();
                response = (HttpWebResponse)request.GetResponse();
                stream = response.GetResponseStream();
                return stream;
            }
            catch (Exception) {
                //FileHelper.WriteFile(ex.ToString(), DateTime.Now.ToShortDateString() + ".txt", Environment.CurrentDirectory + "\\CollectException\\");
                return null;
            }
            finally {
                if (response != null) {
                    response.Close();
                }
                if (stream != null) {
                    stream.Dispose();
                }
                if (outStream != null) {
                    outStream.Close();
                }
                if (response != null) {
                    request.Abort();
                }
            }
        }


        /// <summary>
        /// 采集文件流
        /// </summary>
        /// <param name="url">采集地址</param>
        /// <param name="method">POST,GET</param>
        /// <returns>采集的文件流</returns>
        public Stream CollectStream(string url, Method method) {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream stream = null;
            Stream outStream = null;
            try {
                ServicePointManager.Expect100Continue = false;
                request = (HttpWebRequest)WebRequest.Create(url);
                request.AllowAutoRedirect = true;
                request.Method = method.ToString();
                response = (HttpWebResponse)request.GetResponse();
                stream = response.GetResponseStream();
                return stream;
            }
            catch (Exception) {
                // FileHelper.WriteFile(ex.ToString(), DateTime.Now.ToShortDateString() + ".txt", Environment.CurrentDirectory + "\\CollectException\\");
                return null;
            }
            finally {
                if (response != null) {
                    response.Close();
                }
                if (stream != null) {
                    stream.Dispose();
                }
                if (outStream != null) {
                    outStream.Close();
                }
                if (response != null) {
                    request.Abort();
                }
            }
        }

        public string proxyIp = "";

        /// <summary>
        /// 采集HTML
        /// </summary>
        /// <param name="url">采集地址</param>
        /// <param name="param">采集参数</param>
        /// <param name="method">POST,GET</param>
        /// <param name="sendencoding">发送数据的编码</param>
        /// <param name="receivencoding">接受数据的编码</param>
        /// <param name="cookie">CookieContainer</param>
        /// <returns>返回采集的数据</returns>
        public string CollectHtml(string url, string param, Method method, HtmlEncoding sendEncoding, HtmlEncoding receivEncoding, ref CookieContainer cookie) {
            return CollectHtml(url, param, method, sendEncoding, receivEncoding, ref  cookie, "");
        }

        public string CollectHtml(string url, string param, Method method, HtmlEncoding
                sendEncoding, HtmlEncoding receivEncoding, ref CookieContainer cookie, string Referer) {

            string html = string.Empty;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream stream = null;
            StreamReader sr = null;
            Stream reqStream = null;
            try {
                // ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
                byte[] bs = Encoding.GetEncoding(sendEncoding.ToString()).GetBytes(param);
                request = (HttpWebRequest)WebRequest.Create(url);
                request.CookieContainer = cookie;
                request.Method = method.ToString();
                request.UserAgent = "Mozilla/5.0 (iPad; U; CPU OS 3_2_2 like Mac OS X; en-us) AppleWebKit/531.21.10 (KHTML, like Gecko) Version/4.0.4 Mobile/7B500 Safari/531.21.10";
                request.ContentType = "application/x-www-form-urlencoded";
                request.KeepAlive = true;
                request.AllowAutoRedirect = true;
                int a = 108 * 256 * 256 + 1 * 256 + 5;
                int b = 145 * 256 * 256 + 110 * 256 + 35;
                int c = new Random().Next(b - a) + a;
                if (this.proxyIp != "") {
                    request.Proxy = new WebProxy(this.proxyIp, 8088);
                }

                request.Accept = "image/jpeg, application/x-ms-application, image/gif, application/xaml+xml, image/pjpeg, application/x-ms-xbap, */*";
                if (Referer != "")
                    request.Referer = Referer;
                request.ContentLength = bs.Length;
                if (bs.Length > 0) {
                    reqStream = request.GetRequestStream();
                    reqStream.Write(bs, 0, bs.Length);
                }
                response = (HttpWebResponse)request.GetResponse();
                stream = response.GetResponseStream();
                sr = new StreamReader(stream, Encoding.GetEncoding(receivEncoding.ToString() == "UTF8" ? "UTF-8" : receivEncoding.ToString()));
                html = sr.ReadToEnd();
            }
            catch {
                html = "timeout";
            }
            finally {

                if (response != null) {
                    response.Close();
                }

                if (stream != null) {
                    stream.Dispose();
                }

                if (sr != null) {
                    sr.Dispose();
                }
                if (reqStream != null) {
                    reqStream.Dispose();
                }
                if (request != null) {
                    request.Abort();
                }
            }
            return html;

        }


        /// <summary>
        /// 采集HTML
        /// </summary>
        /// <param name="url">采集地址</param>
        /// <param name="param">采集参数</param>
        /// <param name="method">POST,GET</param>
        /// <param name="sendencoding">发送数据的编码</param>
        /// <param name="receivencoding">接受数据的编码</param>
        /// <param name="cookie">CookieContainer</param>
        /// <returns>返回采集的数据</returns>
        public string CollectHtml(string url, string param, Method method, HtmlEncoding sendEncoding, HtmlEncoding receivEncoding, string referer, ref CookieContainer cookie) {
            string html = string.Empty;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream stream = null;
            StreamReader sr = null;
            Stream reqStream = null;
            try {
                // ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
                byte[] bs = Encoding.GetEncoding(sendEncoding.ToString() == "UTF8" ? "UTF-8" : sendEncoding.ToString()).GetBytes(param);
                request = (HttpWebRequest)WebRequest.Create(url);
                request.CookieContainer = cookie;
                request.Method = method.ToString();
                request.KeepAlive = true;
                request.AllowAutoRedirect = true;
                request.ContentLength = bs.Length;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0)";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Referer = referer;
                request.Accept = "image/jpeg, application/x-ms-application, image/gif, application/xaml+xml, image/pjpeg, application/x-ms-xbap, */*";
                if (bs.Length > 0) {
                    reqStream = request.GetRequestStream();
                    reqStream.Write(bs, 0, bs.Length);
                }
                response = (HttpWebResponse)request.GetResponse();
                stream = response.GetResponseStream();
                sr = new StreamReader(stream, Encoding.GetEncoding(receivEncoding.ToString() == "UTF8" ? "UTF-8" : receivEncoding.ToString()));
                html = sr.ReadToEnd();
            }
            catch {
                html = "timeout";
            }
            finally {

                if (response != null) {
                    response.Close();
                }

                if (stream != null) {
                    stream.Dispose();
                }

                if (sr != null) {
                    sr.Dispose();
                }
                if (reqStream != null) {
                    reqStream.Dispose();
                }
                if (request != null) {
                    request.Abort();
                }
            }
            return html;
        }


        /// <summary>
        /// 采集HTML
        /// </summary>
        /// <param name="url">采集地址</param>
        /// <param name="param">采集参数</param>
        /// <param name="method">POST,GET</param>
        /// <param name="sendencoding">发送数据的编码</param>
        /// <param name="receivencoding">接受数据的编码</param>
        /// <param name="cookie">CookieContainer</param>
        /// <returns>返回采集的数据</returns>
        public string CollectHtml(string url, string param, Method method, HtmlEncoding sendEncoding, HtmlEncoding receivEncoding) {
            string html = string.Empty;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            Stream stream = null;
            StreamReader sr = null;
            Stream reqStream = null;
            try {
                //ServicePointManager.CertificatePolicy = new AcceptAllCertificatePolicy();
                byte[] bs = Encoding.GetEncoding(sendEncoding.ToString() == "UTF8" ? "UTF-8" : sendEncoding.ToString()).GetBytes(param);
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = method.ToString();
                request.Timeout = timeout;
                request.KeepAlive = true;
                request.ContentLength = bs.Length;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0)";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Accept = "image/jpeg, application/x-ms-application, image/gif, application/xaml+xml, image/pjpeg, application/x-ms-xbap, */*";
                if (bs.Length > 0) {
                    reqStream = request.GetRequestStream();
                    reqStream.Write(bs, 0, bs.Length);
                }
                response = (HttpWebResponse)request.GetResponse();
                stream = response.GetResponseStream();
                sr = new StreamReader(stream, Encoding.GetEncoding(receivEncoding.ToString() == "UTF8" ? "UTF-8" : receivEncoding.ToString()));
                html = sr.ReadToEnd();
            }
            catch (Exception) {
                // FileHelper.WriteFile(ex.ToString(), DateTime.Now.ToShortDateString() + ".log", Environment.CurrentDirectory + "\\POST异常\\");
                html = "timeout";
            }
            finally {

                if (response != null) {
                    response.Close();
                }

                if (stream != null) {
                    stream.Dispose();
                }

                if (sr != null) {
                    sr.Dispose();
                }
                if (reqStream != null) {
                    reqStream.Dispose();
                }
                if (request != null) {
                    request.Abort();
                }
            }
            return html;
        }



        /// <summary>
        /// 获取所有COOKIE
        /// </summary>
        /// <param name="cc">Cookie容器</param>
        /// <returns>返回LIST</returns>
        public List<Cookie> GetAllCookies(CookieContainer cc) {
            List<Cookie> lstCookies = new List<Cookie>();
            Hashtable table = (Hashtable)cc.GetType().InvokeMember("m_domainTable", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.Instance, null, cc, new object[] { });
            foreach (object pathList in table.Values) {
                SortedList lstCookieCol = (SortedList)pathList.GetType().InvokeMember("m_list", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.GetField | System.Reflection.BindingFlags.Instance, null, pathList, new object[] { });
                foreach (CookieCollection colCookies in lstCookieCol.Values)
                    foreach (Cookie c in colCookies) lstCookies.Add(c);
            }

            return lstCookies;
        }


        /// <summary>
        /// 字符串转CookieContainer
        /// </summary>
        /// <param name="cookieheard">cookie字符串</param>
        /// <param name="domain">要获取的域名</param>
        /// <returns>返回COOKIE容器</returns>
        public CookieContainer StrConvertToCookieContainer(string cookieheard, string domain) {
            string[] Cookies = cookieheard.Split(";".ToCharArray());
            CookieCollection coll = new CookieCollection();
            foreach (string cookie in Cookies) {
                int index = cookie.IndexOf("=");
                if (index > 0) {
                    string key = cookie.Substring(0, index);
                    string val = cookie.Remove(0, index + 1);
                    Cookie c = new Cookie();
                    c.Name = key.Trim();
                    c.Value = val.Trim();
                    c.Domain = domain;
                    coll.Add(c);
                }
            }
            CookieContainer gg = new CookieContainer();
            gg.Add(new Uri("http://" + domain), coll);
            return gg;
        }


        /// <summary>
        /// Cookie容器转字符串
        /// </summary>
        /// <param name="cookie">Cookie字符串</param>
        /// <param name="domain">要获取的域名</param>
        /// <returns>转换后的字符串</returns>
        public string CookieContainerConvertToStr(CookieContainer cookie, string domain) {
            return cookie.GetCookieHeader(new Uri(domain));
        }
        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="isSign"></param>
        /// <returns></returns>
        public static string Api(string url ,Method method = Method.POST ,bool isSign = true) {
            //string url = string.Format("http://erp.dianbaobao.com:800/api/products?action=checkgoodsinventory&sign={0}&orderid=27398645", sign);
            
            NameValueCollection nvc = new NameValueCollection();
            var str = Api(url ,nvc ,method ,isSign);
            return str;
        }
        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="nameValueCollection"></param>
        /// <param name="method"></param>
        /// <param name="isSign"></param>
        /// <returns></returns>
        public static string Api(string url ,NameValueCollection nameValueCollection ,Method method = Method.POST ,bool isSign = true) {
            if (nameValueCollection == null) {
                nameValueCollection = new NameValueCollection();
            }
            if (nameValueCollection["appId"].ToEString() == "12232622" || nameValueCollection["topAppKey"].ToEString() == "12232622") {
                return "";
            }

            if (isSign) {
                
                url = String.Format(url ,"");
            }
            
            WebClient wb = new WebClient();
            string msg = string.Empty;
            foreach (var item in nameValueCollection.AllKeys) {
                msg += item + nameValueCollection[item] + ",";
            }
            var date = string.Format("{0}-{1}-{2}" ,DateTime.Now.Year ,DateTime.Now.Month ,DateTime.Now.Day);
            var directory = string.Format(@"{0}\taobaolog\{1}\" ,AppDomain.CurrentDomain.BaseDirectory ,date);
            var fileName = DateTime.Now.Hour.ToEString();
            FileHelper.WriteFile("传出：" + DateTime.Now.ToString() + "," + msg ,fileName + ".txt" ,directory);
            var result = wb.UploadValues(url ,method.ToString() ,nameValueCollection);
            var str = System.Text.Encoding.UTF8.GetString(result);
            FileHelper.WriteFile("返回：" + DateTime.Now.ToString() + "," + str ,fileName + ".txt" ,directory);
            return str;
        }

        /// <summary>
        /// 发送请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="isSign"></param>
        /// <returns></returns>
        public static void ApiAsync(string url ,NameValueCollection nvc = null ,Method method = Method.POST ,bool isSign = true) {
            ThreadPool.QueueUserWorkItem(p => Api(url ,nvc ,method ,isSign));
        }

        /// <summary>
        /// 无条件返回真,SSL访问
        /// </summary>
        internal class AcceptAllCertificatePolicy : ICertificatePolicy {
            public AcceptAllCertificatePolicy() {
            }
            public bool CheckValidationResult(ServicePoint sPoint, X509Certificate cert, WebRequest wRequest, int certProb) {
                return true;
            }
        }

        /// <summary>
        /// 访问方法枚举
        /// </summary>
        public enum Method : int {
            POST,
            GET
        }

        /// <summary>
        /// 编码枚举
        /// </summary>
        public enum HtmlEncoding : int {
            GBK,
            GB2312,
            UTF8
        }

    }
}
