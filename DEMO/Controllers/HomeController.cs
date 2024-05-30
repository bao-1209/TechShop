using DEMO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DEMO.Controllers
{
    public class HomeController : Controller
    {
		private DBContext db = new DBContext();
		// GET: Home
		public ActionResult Index()
        {
			var products = db.Products.ToList();
			return View(products);
		}
    }
}