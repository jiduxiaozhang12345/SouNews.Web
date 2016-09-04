using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SouNews.Web.Controllers
{
    public class BaseController : Controller
    {

        //白名单
        private List<string> WriteList = new List<string>() {


        };

        protected void OnActionExecuting(ActionExecutingContext filterContext) {
            string controller = filterContext.RouteData.Values["controller"].ToString();
            string action = filterContext.RouteData.Values["action"].ToString();
            if (!WriteList.Contains(controller + "/" + action)) {
                filterContext.Result = new RedirectResult("/AHome/Login");
            }
        }

    }
}
