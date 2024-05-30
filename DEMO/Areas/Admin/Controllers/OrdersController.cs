using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using DEMO.Models;

namespace DEMO.Areas.Admin.Controllers
{
    public class OrdersController : Controller
    {
        // GET: Admin/Orders
        public ActionResult Index()
        {
            using (DBContext db = new DBContext())
            {
                var orders = db.Orders.Include(o => o.User).ToList();
                return View(orders);
            }
        }

        public ActionResult Detail(int id)
        {
            using (DBContext db = new DBContext())
            {
                Order order = db.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderDetails.Select(od => od.Product))
                    .FirstOrDefault(o => o.order_id == id);

                if (order == null)
                {
                    return HttpNotFound();
                }

                foreach (var detail in order.OrderDetails)
                {
                    if (detail.Product != null && detail.detail_quantity.HasValue)
                    {
                        detail.unit_price = detail.Product.product_price * detail.detail_quantity;
                    }
                }

                return View(order);
            }
        }

        public ActionResult UpdateStatus(int id)
        {
            using (DBContext db = new DBContext())
            {
                Order order = db.Orders.Find(id);
                return View(order);
            }
        }

        [HttpPost]
        public ActionResult UpdateStatus(Order editedOrder)
        {
            using (DBContext db = new DBContext())
            {
                Order order = db.Orders.Find(editedOrder.order_id);

                if (order == null)
                {
                    return HttpNotFound();
                }

                // Cập nhật trạng thái đơn hàng
                order.order_status = editedOrder.order_status;

                // Nếu trạng thái là "đã giao hàng", cập nhật tồn kho
                if (order.order_status == "đã giao hàng")
                {
                    foreach (var detail in order.OrderDetails)
                    {
                        if (detail.Product != null && detail.detail_quantity.HasValue)
                        {
                            // Lấy sản phẩm từ CSDL
                            Product product = db.Products.Find(detail.product_id);
                            if (product != null)
                            {
                                // Trừ số lượng đã bán khỏi tồn kho
                                product.product_quantity -= detail.detail_quantity.Value;
                            }
                        }
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}
