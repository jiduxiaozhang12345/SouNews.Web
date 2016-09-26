using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using SouNews.DB;
using SouNews.Model;
using System.Web.SessionState;

namespace SouNews.Web.Controllers {
    [SessionState(SessionStateBehavior.ReadOnly)]
    public class BaseController : Controller {

        #region 初始化
        /// <summary>
        /// 数据库连接
        /// </summary>
        private SouNewsDBEntities db = new SouNewsDBEntities();

        /// <summary>
        /// 当前用户
        /// </summary>
        public VUsers GlobalUser;
        #endregion

        //白名单
        private List<string> WhiteList = new List<string>() {
           "home",
           "account/login",
        };

        /// <summary>
        /// 访问控制
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext) {
            string controller = filterContext.RouteData.Values["controller"].ToString().ToLower();
            string action = filterContext.RouteData.Values["action"].ToString().ToLower();
            //白名单，不需要验证
            if (WhiteList.Any(w => w.ToLower() == controller + "/" + action) || WhiteList.Any(w => w.ToLower() == controller)) {
                return;
            }
            if (Session["userinfo"] == null) {
                filterContext.Result = new RedirectResult("/Account/Login");
            }
            else {
                GlobalUser = Session["userinfo"] as VUsers;
            }
        }

        /// <summary>
        /// 是否管理员
        /// </summary>
        /// <returns></returns>
        public bool IsAdmin(int id) {
            var roles = (from a in db.Role
                         join b in db.UserRole on a.id equals b.roleId
                         where b.userId == id
                         select a.name).ToList();
            if (roles.Contains("管理员")) {
                return true;
            }
            else {
                return false;
            }
        }
    }
}
