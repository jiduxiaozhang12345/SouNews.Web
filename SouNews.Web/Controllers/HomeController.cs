using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SouNews.Web.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult News() {
            return View();
        }
        public ActionResult Sports() {
            return View();
        }
        public ActionResult Entertainment() {
            return View();
        }
        public ActionResult Technology() {
            return View();
        }
        public ActionResult Tourism() {
            return View();
        }
        public ActionResult Education() {
            return View();
        }
    }
}
