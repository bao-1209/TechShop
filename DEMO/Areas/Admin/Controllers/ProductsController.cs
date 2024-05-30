using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using DEMO.Models;

namespace DEMO.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private readonly DBContext db;

        public ProductsController()
        {
            db = new DBContext();
        }

        // GET: Admin/Products
        public ActionResult Index(string search = "", string SortColumn = "product_id", string IconClass = "fa-sort-asc")
        {
            // Tìm kiếm
            List<Product> products = db.Products
                .Include(p => p.ProductCategory)
                .Include(p => p.Brand)
                .Where(row => row.product_name.Contains(search))
                .ToList();
            ViewBag.Search = search;

            // Sắp xếp
            ViewBag.SortColumn = SortColumn;
            string iconClassToggle = IconClass == "fa-sort-desc" ? "fa-sort-asc" : "fa-sort-desc";
            ViewBag.IconClassToggle = iconClassToggle;
            switch (SortColumn)
            {
                case "product_id":
                    products = IconClass == "fa-sort-asc" ? products.OrderBy(row => row.product_id).ToList() : products.OrderByDescending(row => row.product_id).ToList();
                    break;
                case "product_name":
                    products = IconClass == "fa-sort-asc" ? products.OrderBy(row => row.product_name).ToList() : products.OrderByDescending(row => row.product_name).ToList();
                    break;
            }
            ViewBag.IconClass = iconClassToggle;
            return View(products);
        }

        // Thêm
        public ActionResult Create()
        {
            ViewBag.Categories = db.ProductCategories.ToList();
            ViewBag.Brands = db.Brands.ToList();
            return View(new Product());
        }

        // Thêm
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges(); // Lưu sản phẩm trước để có ID

                if (uploadImage != null && uploadImage.ContentLength > 0)
                {
                    int id = product.product_id; // Lấy ID của sản phẩm vừa được lưu

                    string extension = Path.GetExtension(uploadImage.FileName);
                    string _FileName = "product" + id.ToString() + "_" + extension;
                    string _path = Path.Combine(Server.MapPath("~/Assets/Admin/image"), _FileName);
                    uploadImage.SaveAs(_path);

                    // Cập nhật đường dẫn ảnh
                    product.product_image = _FileName;
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            ViewBag.Categories = db.ProductCategories.ToList();
            ViewBag.Brands = db.Brands.ToList();
            return View(product);
        }

        // Chỉnh sửa
        public ActionResult Edit(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.Categories = db.ProductCategories.ToList();
            ViewBag.Brands = db.Brands.ToList();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product, HttpPostedFileBase uploadImage)
        {
            if (ModelState.IsValid)
            {
                if (uploadImage != null && uploadImage.ContentLength > 0)
                {
                    // Lưu hình ảnh mới
                    string extension = Path.GetExtension(uploadImage.FileName);
                    string _FileName = "product" + product.product_id.ToString() + "_" + extension;
                    string _path = Path.Combine(Server.MapPath("~/Assets/Admin/image"), _FileName);
                    uploadImage.SaveAs(_path);

                    // Xóa hình ảnh cũ nếu tồn tại
                    if (!string.IsNullOrEmpty(product.product_image))
                    {
                        string oldImagePath = Path.Combine(Server.MapPath("~/Assets/Admin/image"), product.product_image);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Cập nhật đường dẫn ảnh mới
                    product.product_image = _FileName;
                }

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Categories = db.ProductCategories.ToList();
            ViewBag.Brands = db.Brands.ToList();
            return View(product);
        }

        // Xóa
        public ActionResult Delete(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // Xóa - Xác nhận xóa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            // Xóa hình ảnh
            if (!string.IsNullOrEmpty(product.product_image))
            {
                string imagePath = Path.Combine(Server.MapPath("~/Assets/Admin/image"), product.product_image);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
