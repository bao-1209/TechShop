using DEMO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DEMO.Controllers
{
    public class ProductController : Controller
    {
		// GET: Product
		private DBContext db = new DBContext();
		public ActionResult Index(string searchString)
		{
			var products = from p in db.Products
						   select p;

			if (!String.IsNullOrEmpty(searchString))
			{
				products = products.Where(s => s.product_name.Contains(searchString));
			}

			return View(products.ToList());
		}
		public ActionResult ByCategory(int categoryId)
		{
			var products = db.Products.Where(p => p.category_id == categoryId).ToList();
			return View(products);
		}

		public ActionResult Details(int? id)
		{
			var product = db.Products.Where(n => n.product_id == id).SingleOrDefault();
			return PartialView(product);
		}
		public ActionResult ByBrands(int? BrandId)
		{
			var products = db.Products.Where(p => p.brand_id == BrandId).ToList();
			return View(products);
		}

	}
}