using SouNews.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SouNews.Web.Controllers
{
    public class AccountController : Controller
    {
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
        public ActionResult Login(VUsers user) {
            return View();
        }

    }
}
