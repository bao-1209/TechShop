using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Web.Mvc;
using DEMO.Models;
using DEMO.ViewModels;

namespace DEMO.Areas.Admin.Controllers
{
    public class Users_RoleController : Controller
    {
        private readonly DBContext db = new DBContext();

        // GET: Admin/Users_Role
        public ActionResult Index(string search ="")
        {
            // Tìm kiếm
            List<User> users = db.Users
        .Where(row => row.user_name.Contains(search) || row.phone.Contains(search))
        .ToList();
            ViewBag.Search = search;

            List<Role> roles = db.Roles.ToList();

            var viewModel = new UsersRoleViewModel
            {
                Roles = roles,
                Users = users
            };

            return View(viewModel);
        }

        // Thêm vai trò
        public ActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRole(Role createrole)
        {
            if (ModelState.IsValid)
            {
                db.Roles.Add(createrole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(createrole);
        }

        // Thêm người dùng
        public ActionResult CreateUsers()
        {
            return View(new User());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUsers(User createusers)
        {
            if (ModelState.IsValid)
            {
                if (db.Users.Any(u => u.user_name == createusers.user_name))
                {
                    ModelState.AddModelError("user_name", "Tên người dùng đã tồn tại.");
                    ViewBag.Role = db.Roles.ToList();
                    return View(createusers);
                }

                createusers.role_id = 2; // Giả sử '2' là role ID của user
                createusers.create_at = DateTime.Now;
                db.Users.Add(createusers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(createusers);
        }

        // Chỉnh sửa vai trò
        public ActionResult EditRole(int id)
        {
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRole(Role editrole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(editrole).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(editrole);
        }

        // Phân quyền người dùng
        public ActionResult EditUsers(int id)
        {
            ViewBag.Role = db.Roles.ToList();
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUsers(int user_id, int role_id)
        {
            User user = db.Users.Find(user_id);
            if (user == null)
            {
                return HttpNotFound();
            }
            user.role_id = role_id;

            // Cập nhật thời gian chỉnh sửa tự động
            user.update_at = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Role = db.Roles.ToList();
            return View(user);
        }



        // Xóa vai trò
        public ActionResult DeleteRole(int id)
        {
            Role role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        [HttpPost, ActionName("DeleteRole")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleConfirmed(int id)
        {
            Role role = db.Roles.Find(id);
            if (role != null)
            {
                db.Roles.Remove(role);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // Xóa người dùng
        public ActionResult DeleteUsers(int id)
        {
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // Xóa - Xác nhận xóa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUsersConfirmed(int id)
        {
            User user = db.Users.Find(id);
            if (user != null)
            {
                db.Users.Remove(user);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
