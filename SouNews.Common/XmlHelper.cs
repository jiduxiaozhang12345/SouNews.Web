using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace SouNews.Common {
    public class XmlHelper {
        /// <summary>
        /// 快递名称与快递编码对应表 key
        /// </summary>
        private const string PosttypevspostcodesCachekey = "POSTTYPEVSPOSTCODESCACHEKEY";

        /// <summary>
        /// 快递名称与快递编码对应表，用在客户端查询物流
        ///  从内存缓存中读取配置。若缓存中不存在，则重新从文件中读取配置，存入缓存
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetPosttypeVsPostCodesDictionary() {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Config\\"+"PosttypeVsPostCodes.xml";
            return GetConfigDictionary(PosttypevspostcodesCachekey, path, XmlHelper.Default) as Dictionary<string, string>;
        }

        /// <summary>
        /// 从内存缓存中读取配置。若缓存中不存在，则重新从文件中读取配置，存入缓存
        /// </summary>
        /// <param name="cacheKey">缓存Key</param>
        /// <param name="path">文件路径</param>
        /// <param name="func">获取xml内容</param>
        /// <returns>配置词典</returns>
        private static object GetConfigDictionary(string cacheKey, string path, Func<string, object> func) {
            object configs = new object();
            ObjectCache cache = MemoryCache.Default;
            if (cache.Contains(cacheKey)) {
                var cacheItem = cache.GetCacheItem(cacheKey);
                if (cacheItem != null) {
                    configs = cacheItem.Value;
                }
            }
            else {
                configs = func(path);
                CacheItemPolicy policy = new CacheItemPolicy { Priority = CacheItemPriority.NotRemovable };
                cache.Set(cacheKey, configs, policy);
                List<string> filePaths = new List<string>() { path };
                HostFileChangeMonitor monitor = new HostFileChangeMonitor(filePaths);
                monitor.NotifyOnChanged((o) => {
                    cache.Remove(cacheKey);
                });
                policy.ChangeMonitors.Add(monitor);
            }
            return configs;
        }

        private static Dictionary<string, string> Default(string path) {
            var configs = new Dictionary<string, string>();
            XDocument doc = XDocument.Load(path);
            var root = doc.Root;
            if (root != null) {
                var elements = root.Elements();
                var query = from element in elements
                            select new { key = element.Attribute("key").Value, value = element.Attribute("value").Value };
                var list = query.ToList();
                configs = list.ToList().ToDictionary(p => p.key, m => m.value);
            }
            return configs;
        }
    }
}