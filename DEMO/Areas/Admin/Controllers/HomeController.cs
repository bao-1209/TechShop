using System;
using System.Linq;
using System.Web.Mvc;
using DEMO.Models;

namespace DEMO.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Admin/Home
        public ActionResult Index()
        {
            // Truy vấn cơ sở dữ liệu để tính tổng doanh thu từ các đơn hàng
            decimal? totalRevenue = db.Orders
        .Where(o => o.total_price != null) // Lọc ra các đơn hàng có giá trị total_price không null
        .Sum(o => (decimal?)o.total_price); // Sử dụng kiểu dữ liệu decimal? để xử lý giá trị null


            // Gán giá trị tổng doanh thu vào ViewBag để truyền sang view
            ViewBag.TotalRevenue = totalRevenue;

            return View();
        }
    }
}
