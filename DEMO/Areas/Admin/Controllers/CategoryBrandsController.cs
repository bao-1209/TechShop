using DEMO.Models;
using DEMO.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DEMO.Areas.Admin.Controllers
{
    public class CategoryBrandsController : Controller
    {
        private readonly DBContext db = new DBContext();
        // GET: Admin/CategoryBrands
        public ActionResult Index()
        {
            List<ProductCategory> productCategories = db.ProductCategories.ToList();
            List<Brand> brands = db.Brands.ToList();
            List<Category_Brand> category_Brands = db.Category_Brand.ToList();

            var viewModel = new Category_BrandViewModels
            {
                ProductCategories = productCategories,
                Brands = brands,
                Category_Brands = category_Brands,
            };

            return View(viewModel);
        }


        //Thêm danh mục sản phẩm
        public ActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCategory(ProductCategory createcategory)
        {
            DBContext db = new DBContext();
            db.ProductCategories.Add(createcategory);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //Thêm thương hiệu
        public ActionResult CreateBrands()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateBrands(Brand createbrands)
        {
            DBContext db = new DBContext();
            db.Brands.Add(createbrands);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //Thêm phân loại
        public ActionResult CreateCategoryBrand()
        {
            ViewBag.Categories = db.ProductCategories.ToList();
            ViewBag.Brands = db.Brands.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult CreateCategoryBrand(Category_Brand createcategorybrand)
        {
            DBContext db = new DBContext();
            db.Category_Brand.Add(createcategorybrand);
            db.SaveChanges();
            ViewBag.Categories = db.ProductCategories.ToList();
            ViewBag.Brands = db.Brands.ToList();
            return RedirectToAction("Index");
        }




        // Sua danh muc
        public ActionResult EditCategory(int id)
        {
            DBContext db = new DBContext();
            ProductCategory category = db.ProductCategories.Where(row => row.category_id == id).FirstOrDefault();
            return View(category);
        }

        [HttpPost]
        public ActionResult EditCategory(ProductCategory editcategory)
        {
            DBContext db = new DBContext();
            ProductCategory category = db.ProductCategories.Where(row => row.category_id == editcategory.category_id).FirstOrDefault();

            //cap nhat
            category.category_name = editcategory.category_name;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //Chinh sua thuong hieu
        public ActionResult EditBrands(int id)
        {
            Brand brand = db.Brands.Where(row => row.brand_id == id).FirstOrDefault();
            return View(brand);
        }

        [HttpPost]
        public ActionResult EditBrands(Brand editbrand)
        {
            Brand brand = db.Brands.Where(row => row.brand_id == editbrand.brand_id).FirstOrDefault();

            brand.brand_name = editbrand.brand_name;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // Chỉnh sửa category brands
        public ActionResult EditCategoryBrand(int id)
        {
            Category_Brand category_Brand = db.Category_Brand.Where(row => row.categorybrand_id == id).FirstOrDefault();
            if (category_Brand == null)
            {
                return HttpNotFound();
            }
            ViewBag.Categories = db.ProductCategories.ToList();
            ViewBag.Brands = db.Brands.ToList();
            return View(category_Brand);
        }


        [HttpPost]
        public ActionResult EditCategoryBrand(Category_Brand editcategorybrands)
        {
            Category_Brand category_Brand = db.Category_Brand.Where(row => row.categorybrand_id == editcategorybrands.categorybrand_id).FirstOrDefault();

            ViewBag.Categories = db.ProductCategories.ToList();
            ViewBag.Brands = db.Brands.ToList();
            db.SaveChanges();
            return RedirectToAction("Index");
        }




        // Xóa danh mục
        public ActionResult DeleteCategory(int id)
        {
            ProductCategory category = db.ProductCategories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // Xóa - Xác nhận xóa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCategoryConfirmed(int id)
        {
            ProductCategory category = db.ProductCategories.Find(id);
            if (category != null)
            {
                db.ProductCategories.Remove(category);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // Xóa thương hiệu
        public ActionResult DeleteBrands(int id)
        {
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // Xóa - Xác nhận xóa
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteBrandsConfirmed(int id)
        {
            Brand brand = db.Brands.Find(id);
            if (brand != null)
            {
                db.Brands.Remove(brand);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}