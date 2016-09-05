using SouNews.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SouNews.Web.Controllers
{
    public class MenuController : BaseController
    {
        private SouNewsDBEntities db = new SouNewsDBEntities();

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetMenuByName() {
            return Json(db.Menu.ToList());
        }

    }
}
