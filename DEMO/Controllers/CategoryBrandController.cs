using DEMO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DEMO.Controllers
{
    public class CategoryBrandController : Controller
    {
        // GET: CategoryBrand
        private DBContext db = new DBContext();
        public ActionResult Category_Brand(int categoryBrand)
		{
            var products = db.Category_Brand.Where(p => p.categorybrand_id == categoryBrand).ToList();
			return View();
        }
    }
}