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

    }
}
