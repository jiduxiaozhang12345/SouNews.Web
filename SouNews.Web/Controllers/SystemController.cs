using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Data.Entity;

using EntityFramework.Extensions;
using ShowPin.MvcPaging;

using SouNews.DB;
using SouNews.Model;
using SouNews.Common;

namespace SouNews.Web.Controllers {
    public class SystemController : BaseController {
        private SouNewsDBEntities db = new SouNewsDBEntities();

        #region 用户管理
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Userlist(string name, int? page = 1) {
            int currentPageIndex = page.HasValue ? page.Value : 1;
            var query = db.Users.Where(w => 1 == 1);
            if (!string.IsNullOrEmpty(name)) {
                query = query.Where(w => w.username.Contains(name));
            }
            ViewBag.data = query.OrderByDescending(w => w.id).ToPagedList(currentPageIndex, 20);
            return View(new Users());
        }

        //添加
        public ActionResult UserAdd() {
            return View(new Users());
        }

        /// <summary>
        /// 添加    提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UserAdd(Users user) {
            var data = db.Users.Where(w => w.username == user.username).FirstOrDefault();
            if (data != null) {
                return Json(new { code = 1, message = "用户名已存在！" });
            }
            user.password = SecurityHelper.MD5(user.password);
            user.addtime = DateTime.Now;
            db.Users.Add(user);
            int num = db.SaveChanges();
            return Json(new { code = num, isdelete = false });
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public ActionResult UserEdit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users user = db.Users.Find(id);
            if (user == null) {
                return HttpNotFound();
            }
            return View(user);
        }

        /// <summary>
        /// 修改    提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UserEdit(Users user) {
            var data = db.Users.Where(w => w.username == user.username && w.id != user.id).FirstOrDefault();
            if (data != null) {
                return Json(new { code = 1, message = "用户名已存在！" });
            }
            user.password = SecurityHelper.MD5(user.password);
            int num = db.Users.Where(w => w.id == user.id).Update(w => new Users() {
                username = user.username,
                password = user.password,
                email = user.email
            });
            return Json(new { code = num, isdelete = false });
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public ActionResult UserDel(int id) {
            int num = db.Users.Where(w => w.id == id).Delete();
            return Json(new { code = num, isdelete = true });
        }

        /// <summary>
        /// 设置角色  列表
        /// </summary>
        /// <returns></returns>
        public ActionResult UserSetRole(int userId, string name, int? page = 1) {
            int currentPageIndex = page.HasValue ? page.Value : 1;
            //获取员工已有的角色编号
            var oldroleids = db.UserRole.Where(w => w.userId == userId).Select(w => w.roleId).ToList();
            ViewBag.userroles = oldroleids;
            var rolesdata = db.Role.Where(w => 1 == 1);
            //获取角色列表
            if (!string.IsNullOrEmpty(name)) {
                //查询的角色
                rolesdata = rolesdata.Where(w => w.name.Contains(name));
                //已有的角色
                var oldroles = db.Role.Where(w => oldroleids.Contains(w.id));
                rolesdata = rolesdata.Union(oldroles);
            }
            ViewBag.data = rolesdata.OrderByDescending(s => s.id).ToPagedList(currentPageIndex, db.Role.Count());
            ViewBag.name = name;
            ViewBag.userId = userId;
            return View(new Role());
        }

        /// <summary>
        /// 设置角色  提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UserSetRole(string ids, int userid) {
            var data = db.UserRole.Where(w => w.userId == userid).Delete();
            db.SaveChanges();
            string[] strs = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in strs) {
                var roleid = Convert.ToInt32(item);
                UserRole rp = new UserRole() {
                    roleId = roleid,
                    userId = userid
                };
                db.UserRole.Add(rp);
            }
            int count = db.SaveChanges();
            return Json(new { code = count, isdelete = false });
        }
        #endregion

        #region 权限管理
        /// <summary>
        /// 权限列表
        /// </summary>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult Powerlist(string name, int? page = 1) {
            int currentPageIndex = page.HasValue ? page.Value : 1;
            var query = db.Power.Where(w => 1 == 1);
            if (!string.IsNullOrEmpty(name)) {
                query = query.Where(w => w.name.Contains(name));
            }
            ViewBag.data = query.OrderByDescending(s => s.id).ToPagedList(currentPageIndex, 20);
            ViewBag.name = name;
            return View(new Power());
        }

        /// <summary>
        /// 新建
        /// </summary>
        /// <returns></returns>
        public ActionResult PowerAdd() {
            return View(new Power());
        }

        /// <summary>
        /// 新建    提交
        /// </summary>
        /// <param name="powers"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PowerAdd(Power powers) {
            powers.name = powers.name.Trim();
            powers.path = powers.path.ToLower().Trim();
            db.Power.Add(powers);
            int count = db.SaveChanges();
            return Json(new { code = count, isdelete = false });
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult PowerEdit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Power powers = db.Power.Find(id);
            if (powers == null) {
                return HttpNotFound();
            }
            return View(powers);
        }

        /// <summary>
        /// 编辑    提交
        /// </summary>
        /// <param name="powers"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PowerEdit(Power powers) {
            if (ModelState.IsValid) {
                powers.name = powers.name.Trim();
                powers.path = powers.path.ToLower().Trim();
                db.Entry(powers).State = EntityState.Modified;
                int count = db.SaveChanges();
                return Json(new { code = count, isdelete = false });
            }
            return View(powers);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult PowerDel(int id) {
            int num = db.Power.Where(w => w.id == id).Delete();
            return Json(new { code = num, isdelete = true });
        }
        #endregion

        #region 菜单管理
        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Menulist(string name, int? page = 1) {
            int currentPageIndex = page.HasValue ? page.Value : 1;
            var query = db.Menu.Where(w => 1 == 1);
            if (!string.IsNullOrEmpty(name)) {
                query = query.Where(w => w.name.Contains(name));
            }
            ViewBag.data = query.OrderByDescending(w => w.id).ToPagedList(currentPageIndex, 20);
            return View(new Menu());
        }

        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetMenus() {
            if (IsAdmin(GlobalUser.LoginUser.id)) {
                return Json(db.Menu.ToList());
            }
            else {
                List<Menu> list = new List<Menu>();
                var powerlist = GlobalUser.PowerList.Select(w => w.path).ToList();
                var menulist = db.Menu.ToList();
                foreach (var item in menulist) {
                    if (powerlist.Contains(item.path)) {
                        list.Add(item);
                    }
                }
                return Json(list);
            }
        }

        //添加
        public ActionResult MenuAdd() {
            return View(new Menu());
        }

        /// <summary>
        /// 添加    提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MenuAdd(Menu menu) {
            var data = db.Menu.Where(w => w.name == menu.name).FirstOrDefault();
            if (data != null) {
                return Json(new { code = 1, message = "用户名已存在！" });
            }
            if (!string.IsNullOrEmpty(menu.path)) {
                menu.path = menu.path.Trim().ToLower();
            }
            db.Menu.Add(menu);
            int num = db.SaveChanges();
            return Json(new { code = num, isdelete = false });
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public ActionResult MenuEdit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menu.Find(id);
            if (menu == null) {
                return HttpNotFound();
            }
            return View(menu);
        }

        /// <summary>
        /// 修改    提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult MenuEdit(Menu menu) {
            var data = db.Menu.Where(w => w.name == menu.name && w.id != menu.id).FirstOrDefault();
            if (data != null) {
                return Json(new { code = 1, message = "用户名已存在！" });
            }
            if (!string.IsNullOrEmpty(menu.path)) {
                menu.path = menu.path.Trim().ToLower();
            }
            int num = db.Menu.Where(w => w.id == menu.id).Update(w => new Menu() {
                name = menu.name,
                path = menu.path,
                description = menu.description,
                parentId = menu.parentId,
                position = menu.position,
            });
            return Json(new { code = num, isdelete = false });
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public ActionResult MenuDel(int id) {
            int num = db.Menu.Where(w => w.id == id).Delete();
            return Json(new { code = num, isdelete = true });
        }
        #endregion

        #region 角色管理
        /// <summary>
        /// 角色列表
        /// </summary>
        /// <returns></returns>
        public ActionResult Rolelist(string name, int? page = 1) {
            int currentPageIndex = page.HasValue ? page.Value : 1;
            var query = db.Role.Where(w => 1 == 1);
            if (!string.IsNullOrEmpty(name)) {
                query = query.Where(w => w.name.Contains(name));
            }
            ViewBag.data = query.OrderByDescending(w => w.id).ToPagedList(currentPageIndex, 20);
            return View(new Role());
        }

        //添加
        public ActionResult RoleAdd() {
            return View(new Role());
        }

        /// <summary>
        /// 添加    提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RoleAdd(Role role) {
            var data = db.Role.Where(w => w.name == role.name).FirstOrDefault();
            if (data != null) {
                return Json(new { code = 1, message = "用户名已存在！" });
            }
            db.Role.Add(role);
            int num = db.SaveChanges();
            return Json(new { code = num, isdelete = false });
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public ActionResult RoleEdit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Role role = db.Role.Find(id);
            if (role == null) {
                return HttpNotFound();
            }
            return View(role);
        }

        /// <summary>
        /// 修改    提交
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RoleEdit(Role role) {
            var data = db.Role.Where(w => w.name == role.name && w.id != role.id).FirstOrDefault();
            if (data != null) {
                return Json(new { code = 1, message = "用户名已存在！" });
            }
            int num = db.Role.Where(w => w.id == role.id).Update(w => new Role() {
                name = role.name,
                info = role.info
            });
            return Json(new { code = num, isdelete = false });
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public ActionResult RoleDel(int id) {
            int num = db.Role.Where(w => w.id == id).Delete();
            return Json(new { code = num, isdelete = true });
        }

        /// <summary>
        /// 设置权限   列表
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="name"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult RoleSetPower(int roleId, string name, int? page = 1) {
            int currentPageIndex = page.HasValue ? page.Value : 1;
            var query = db.Power.Where(w=>1==1);
            //获取权限列表
            if (!string.IsNullOrEmpty(name)) {
                query = query.Where(w => w.name == name);
            }
            //获取角色对应的权限编号
            ViewBag.rolepowers = db.RolePower.Where(w => w.roleId == roleId).Select(w => w.powerId).ToList();
            ViewBag.powers = query.OrderByDescending(s => s.id).ToPagedList(currentPageIndex, 300);
            ViewBag.name = name;
            ViewBag.roleId = roleId;
            return View(new Power());
        }

        /// <summary>
        /// 设置权限
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RoleSetPower(string ids, int roleId) {
            var data = db.RolePower.Where(w => w.roleId == roleId).Delete();
            string[] strs = ids.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in strs) {
                var powerid = Convert.ToInt32(item);
                RolePower rp = new RolePower() {
                    powerId = powerid,
                    roleId = roleId
                };
                db.RolePower.Add(rp);
            }
            db.SaveChanges();
            return Json(new { code = 1, isdelete = false });
        }
        #endregion
    }
}
