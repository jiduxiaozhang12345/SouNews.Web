using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SouNews.Model {
    /// <summary>
    /// 全局配置文件
    /// </summary>
    public class GlobalConfig {
        public static Dictionary<string, string> ArticleType = new Dictionary<string, string>() {
        {"新闻","新闻"},
        {"体育","体育"},
        {"娱乐","娱乐"},
        {"科技","科技"},
        {"旅游","旅游"},
        {"教育","教育"},
        };
    }
}
