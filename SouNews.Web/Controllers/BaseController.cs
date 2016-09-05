using SouNews.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SouNews.Web.Controllers {
    public class BaseController : Controller {

        //用户
        public VUsers GlobalUser;
        //白名单
        private List<string> WriteList = new List<string>() {

        };

        /// <summary>
        /// 访问控制
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            string controller = filterContext.RouteData.Values["controller"].ToString().ToLower();
            string action = filterContext.RouteData.Values["action"].ToString().ToLower();
            if (!WriteList.Contains(controller + "/" + action) && controller != "home" && Session["userinfo"] != null) {
                filterContext.Result = new RedirectResult("/Account/Login");
            }
            else {
                GlobalUser = Session["userinfo"] as VUsers;
            }
        }

    }
}
