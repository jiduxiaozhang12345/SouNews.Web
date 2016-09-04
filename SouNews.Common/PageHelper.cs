using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SouNews.Common
{
    public class PageHelper
    {
        /// <summary>
        /// 获取页码的起始位置和结束位置，并将该范围的数字返回
        /// </summary>
        /// <param name="pageindex">当前页码</param>
        /// <param name="pagesize">每页显示的条数</param>
        /// <param name="pagecount">总页数</param>
        /// <returns></returns>
        public static string GetPage(int pageindex,int pagesize,int pagecount)
        {
            int start = pageindex - pagesize;
            int end = start + 2 * pagesize - 1;
            if (start<1)
            {
                start = 1;
                end = start + 2 * pagesize - 1 > pagecount ? pagecount : start + 2 * pagesize - 1;
            }
            if (end>pagecount)
            {
                end = pagecount;
                //从后往前显示pagesize个数字
                start = pagecount - 2 * pagesize + 1 < 1 ? 1 : pagecount - 2 * pagesize + 1;
            }
            StringBuilder sb=new StringBuilder();
            //上一页
            if (pageindex>1)
            {
                sb.Append("<a href='News.aspx?pageindex=" + (pageindex-1) + "'>上一页</a>");
            }
            for (int i = start; i <= end; i++)
            {
                sb.Append("<a href='News.aspx?pageindex=" + i + "'>" + i + "</a>");
            }
            //下一页
            if (pageindex <pagecount)
            {
                sb.Append("<a href='News.aspx?pageindex=" + (pageindex + 1) + "'>下一页</a>");
            }
            return sb.ToString();
        }
    }
}
