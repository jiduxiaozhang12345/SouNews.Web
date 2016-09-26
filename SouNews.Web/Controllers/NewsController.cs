using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Web.SessionState;

using ShowPin.MvcPaging;
using EntityFramework.Extensions;

using SouNews.DB;
using SouNews.Model;

namespace SouNews.Web.Controllers {
    [SessionState(SessionStateBehavior.Required)]
    public class NewsController : BaseController {
        private SouNewsDBEntities db = new SouNewsDBEntities();

        #region 文章管理
        /// <summary>
        /// 文章列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Articlelist(string name, int? page = 1) {
            int currentPageIndex = page.HasValue ? page.Value : 1;
            var query = db.Article.Where(w => 1 == 1);
            if (!string.IsNullOrEmpty(name)) {
                query = query.Where(w => w.title.Contains(name));
            }
            ViewBag.data = query.OrderByDescending(w => w.id).ToPagedList(currentPageIndex, 20);
            ViewBag.name = name;
            return View(new Article());
        }

        //添加
        public ActionResult ArticleAdd() {
            ViewBag.type = GlobalConfig.ArticleType.Select(w => new SelectListItem() {  Value = w.Key, Text = w.Key}).ToList();
            return View(new Article());
        }

        /// <summary>
        /// 添加    提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ArticleAdd(Article news) {
            var data = db.Article.Where(w => w.title == news.title).FirstOrDefault();
            if (data != null) {
                return Json(new { code = 1, message = "此标题已存在！" });
            }
            news.state = 1;
            news.addtime = DateTime.Now;
            db.Article.Add(news);
            int num = db.SaveChanges();
            return Json(new { code = num, isdelete = false });
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public ActionResult ArticleEdit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article news = db.Article.Find(id);
            if (news == null) {
                return HttpNotFound();
            }
            ViewBag.type = GlobalConfig.ArticleType.Select(w => new SelectListItem() { Value = w.Key, Text = w.Key }).ToList();
            return View(news);
        }

        /// <summary>
        /// 修改    提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ArticleEdit(Article news) {
            var data = db.Article.Where(w => w.title == news.title && w.id != news.id).FirstOrDefault();
            if (data != null) {
                return Json(new { code = 1, message = "此标题已存在！" });
            }
            int num = db.Article.Where(w => w.id == news.id).Update(w => new Article() {
                title = news.title,
                contents = news.contents,
                type = news.type,
                state = news.state,
                addtime = DateTime.Now
            });
            return Json(new { code = num, isdelete = false });
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public ActionResult ArticleDel(int id) {
            int num = db.Article.Where(w => w.id == id).Delete();
            return Json(new { code = num, isdelete = true });
        }
        #endregion

        #region 模块管理
        /// <summary>
        /// 模块列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Modulelist(string name, int? page = 1) {
            int currentPageIndex = page.HasValue ? page.Value : 1;
            var query = db.Module.Where(w => 1 == 1);
            if (!string.IsNullOrEmpty(name)) {
                query = query.Where(w => w.name.Contains(name));
            }
            ViewBag.data = query.OrderByDescending(w => w.id).ToPagedList(currentPageIndex, 20);
            ViewBag.name = name;
            return View(new Module());
        }

        //添加
        public ActionResult ModuleAdd() {
            ViewBag.type = GlobalConfig.ArticleType.Select(w => new SelectListItem() { Value = w.Key, Text = w.Key }).ToList();
            return View(new Module());
        }

        /// <summary>
        /// 添加    提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ModuleAdd(Module module) {
            var data = db.Module.Where(w => w.name == module.name).FirstOrDefault();
            if (data != null) {
                return Json(new { code = 1, message = "此名称已存在！" });
            }
            module.addtime = DateTime.Now;
            db.Module.Add(module);
            int num = db.SaveChanges();
            return Json(new { code = num, isdelete = false });
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public ActionResult ModuleEdit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module news = db.Module.Find(id);
            if (news == null) {
                return HttpNotFound();
            }
            ViewBag.type = GlobalConfig.ArticleType.Select(w => new SelectListItem() { Value = w.Key, Text = w.Key }).ToList();
            return View(news);
        }

        /// <summary>
        /// 修改    提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ModuleEdit(Module module) {
            var data = db.Module.Where(w => w.name == module.name && w.id != module.id).FirstOrDefault();
            if (data != null) {
                return Json(new { code = 1, message = "此名称已存在！" });
            }
            int num = db.Module.Where(w => w.id == module.id).Update(w => new Module() {
                name = module.name,
                type = module.type,
                addtime = DateTime.Now
            });
            return Json(new { code = num, isdelete = false });
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public ActionResult ModuleDel(int id) {
            int num = db.Module.Where(w => w.id == id).Delete();
            return Json(new { code = num, isdelete = true });
        }
        #endregion
    }
}