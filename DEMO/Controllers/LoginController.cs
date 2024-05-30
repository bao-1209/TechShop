using DEMO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace DEMO.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User users)
        {
            using (DBContext db = new DBContext())
            {
                var userInDb = db.Users.FirstOrDefault(u => u.user_name == users.user_name && u.password == users.password);
                if (userInDb != null)
                {
                    Session["User"] = userInDb;
                    Session["UserName"] = userInDb.user_name;
                    if (userInDb.role_id == 1) // Kiểm tra vai trò của người dùng
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" }); // Chuyển hướng người dùng đến trang Admin
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home"); // Chuyển hướng người dùng đến trang Home
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            return View(users);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User users)
        {
            if (ModelState.IsValid)
            {
                using (DBContext db = new DBContext())
                {
                    // Kiểm tra xem tên người dùng hoặc email đã tồn tại chưa
                    var existingUser = db.Users.FirstOrDefault(u => u.user_name == users.user_name);
                    var existingEmail = db.Users.FirstOrDefault(u => u.email == users.email);
                    if (existingUser != null)
                    {
                        ModelState.AddModelError("", "Username already exists");
                        return View(users);
                    }
                    if (existingEmail != null)
                    {
                        ModelState.AddModelError("", "Email already exists");
                        return View(users);
                    }

                    // Đặt mặc định role_id là 2
                    users.role_id = 2;

                    db.Users.Add(users);
                    db.SaveChanges();
                    return RedirectToAction("Login");
                }
            }
            return View(users);
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
