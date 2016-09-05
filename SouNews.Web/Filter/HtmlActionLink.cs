using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Mvc.Ajax;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using SouNews.Common;
using SouNews.Model;
using SouNews.DB;

namespace SouNews.Web {
    public static class HtmlActionLink {

        #region 普通链接扩展方法

        /// <summary>
        /// 无参数
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString InkindActionLink(this HtmlHelper htmlHelper, string username, string linkText, string actionName,
            string controllerName, object htmlAttributes) {
            return htmlHelper.InkindActionLink(username, linkText, actionName, controllerName, null, htmlAttributes);
        }

        /// <summary>
        /// 新 controllerName 页面链接
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="linkText">@不编码</param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString InkindActionLink(this HtmlHelper htmlHelper, string username, string linkText, string actionName,
            string controllerName, object routeValues, object htmlAttributes) {

            if (!CheckPower(actionName, controllerName)) {
                return new MvcHtmlString("");
            }
            string htmlString = htmlHelper.ActionLink(linkText.Trim(), actionName,
             controllerName, routeValues, htmlAttributes).ToHtmlString().ToString()
              .Replace("datatoggle", "data-toggle").Replace("dataurl", "data-url").Replace("datawidth", "data-width").Replace("dataheight", "data-height");
            return new MvcHtmlString(htmlString);
        }

        /// <summary>
        /// 判断权限
        /// </summary>
        /// <returns></returns>
        public static bool CheckPower(string actionName, string controllerName) {
            if (string.IsNullOrEmpty(controllerName)) {
                string redirectOnSuccess = HttpContext.Current.Request.Url.AbsolutePath;
                string[] localPathArr = redirectOnSuccess.Split('/');
                if (localPathArr.Length - 2 > 0) {
                    controllerName = localPathArr[localPathArr.Length - 2];
                }
                else {
                    controllerName = "Home";
                }
            }

            //登录默认就可以拥有的权限
            List<string> powerlist = new List<string> { 
                "account/logout",
                "account/index" 
            };
            VUsers user = HttpContext.Current.Session["userinfo"] as VUsers;
            if (user == null) {
                return false;
            }

            else {
                //pass 登录相关
                if (powerlist.Where(s => s == (controllerName + "/" + actionName)).Any()) {
                    return true;
                }
                List<Power> powerList = user.PowerList;

                if (powerList == null) {
                    return false;
                }
                string roules = (controllerName + "/" + actionName).ToLower().Trim();
                //没有权限直接不反馈任何东西
                //return true;
                return powerList.Where(p => p.path == roules && p.path != null).Any();
            }

        }
        #endregion

        #region ajax 链接

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ajaxHelper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="ajaxOptions"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>

        public static MvcHtmlString InkindActionLink(this AjaxHelper ajaxHelper, string username, string linkText, string actionName,
         string controllerName, AjaxOptions ajaxOptions, object htmlAttributes) {
            return ajaxHelper.InkindActionLink(username, linkText, actionName, controllerName, null, ajaxOptions, htmlAttributes);
        }

        /// <summary>
        /// 最全的重载
        /// </summary>
        /// <param name="ajaxHelper"></param>
        /// <param name="linkText"></param>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="ajaxOptions"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString InkindActionLink(this AjaxHelper ajaxHelper, string username, string linkText, string actionName,
            string controllerName, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes) {
            if (!CheckPower(actionName, controllerName)) {
                return new MvcHtmlString("");
            }
            string htmlString = ajaxHelper.ActionLink(linkText.ToEString().Trim(), actionName, controllerName, routeValues, ajaxOptions, htmlAttributes).ToHtmlString().ToString();

            return new MvcHtmlString(htmlString);
        }

        #endregion

    }
}