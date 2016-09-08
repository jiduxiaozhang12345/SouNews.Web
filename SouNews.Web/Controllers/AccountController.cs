using SouNews.Common;
using SouNews.DB;
using SouNews.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SouNews.Web.Controllers {
    public class AccountController : Controller {
        public ActionResult Index() {
            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Login() {
            return View();
        }

        /// <summary>
        /// 登录    提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(string username, string password) {
            SouNewsDBEntities db = new SouNewsDBEntities();
            string pwd = string.Empty;
            if (password.Length == 32) {
                pwd = password;
            }
            else {
                pwd = SecurityHelper.MD5(password);
            }
            VUsers vuser = new VUsers();
            var user = db.Users.Where(w => w.username == username && w.password == pwd).FirstOrDefault();
            if (user != null) {
                List<Power> powerlist = null;
                //获取权限列表
                var roleIds = db.UserRole.Where(w => w.userId == user.id).Select(w => w.roleId).ToList();
                var roles = string.Join(",", db.Role.Where(w => roleIds.Contains(w.id)).Select(w => w.name).ToList());
                if (roles.Contains("管理员")) {
                    powerlist = db.Power.ToList();
                }
                else {
                    powerlist = (from a in db.Power
                                 join b in db.RolePower on a.id equals b.powerId
                                 where roleIds.Contains(b.roleId)
                                 select a).ToList();
                }
                vuser.LoginUser = user;
                vuser.Roles = roles;
                vuser.PowerList = powerlist;
                Session["userinfo"] = vuser;
                return Content("{'message':'ok','t':'" + pwd + "'}");
            }
            return Content("{'message':'no','t':''}");
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginOut() {
            Session["userinfo"] = null;
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}
