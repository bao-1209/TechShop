using DEMO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using System.Web.Mvc;

namespace DEMO.Controllers
{
	public class ShoppingCartController : Controller
	{
		DBContext db = new DBContext();
		// GET: ShoppingCart
		public List<CartItem> GetCartItems()
		{
			List<CartItem> lstCartItems = Session["CartItems"] as List<CartItem>;
			if (lstCartItems == null)
			{
				lstCartItems = new List<CartItem>();
				Session["CartItems"] = lstCartItems;
			}
			return lstCartItems;
		}
		public ActionResult AddCartItems(int ProductID, int Quantity)
		{
			DBContext db = new DBContext();
			Product product = db.Products.Find(ProductID);
			if (product == null)
			{
				return HttpNotFound();
			}
			CartItem cartItem = new CartItem();
			cartItem.product_id = ProductID;
			cartItem.Product = product;
			cartItem.Quantity = Quantity;
			List<CartItem> lstCartItems = GetCartItems();
			CartItem item = lstCartItems.Find(p => p.product_id == ProductID);
			if (item == null)
			{
				lstCartItems.Add(cartItem);
			}
			else
			{
				item.Quantity += Quantity;
			}
			if (lstCartItems.Count == 0)
			{
				
				//return View("EmptyCart");
				
				return Json(new { message = "Giỏ hàng không có sản phẩm." });
			}

			return RedirectToAction("ShowCart") as ActionResult;
		}
		private int TotalQuantity()
		{
			int? totalQuantity = 0;
			List<CartItem> lstCartItems = GetCartItems();
			foreach (var item in lstCartItems)
			{
				totalQuantity += item.Quantity;
			}
			return (int)totalQuantity;
		}
		private double TotalPrice()
		{
			double totalPrice = 0;
			List<CartItem> lstCartItems = Session["CartItems"] as List<CartItem>;
			if (lstCartItems != null)
			{
				totalPrice = (double)lstCartItems.Sum(p => p.Quantity * p.Product.product_price);
			}
			return totalPrice;
		}
		public ActionResult ShowCart()
		{
			List<CartItem> lstCartItems = GetCartItems();
			if (lstCartItems.Count == 0)
			{
				return RedirectToAction("Index", "Home");
			}
			ViewBag.TotalQuantity = TotalQuantity();
			ViewBag.TotalPrice = TotalPrice();
			return View(lstCartItems);
		}
		public ActionResult RemoveCart(int ProductID)
		{
			List<CartItem> lstCartItems = GetCartItems();
			CartItem item = lstCartItems.SingleOrDefault(p => p.product_id == ProductID);
			if (item != null)
			{
				lstCartItems.RemoveAll(n => n.product_id == ProductID);
				return RedirectToAction("ShowCart");
			}
			if(lstCartItems.Count == 0)
			{
				return RedirectToAction("Index", "Home");
			}
			return RedirectToAction("ShowCart");
		}
		public ActionResult UpdateCart(int ProductID, FormCollection form)
		{
			List<CartItem> lstCartItems = GetCartItems();
			CartItem item = lstCartItems.Find(p => p.product_id == ProductID);
			if (item != null)
			{
				item.Quantity = int.Parse(form["Quantity"].ToString());
			}
			return RedirectToAction("ShowCart");
		}
		[HttpGet]
		public ActionResult Order()
		{
			if (Session["User"] == null || Session["User"].ToString() == "")
			{
				return RedirectToAction("Login", "Login");
			}
			if (Session["CartItems"] == null)
			{
				return RedirectToAction("Index", "Home");
			}
			List<CartItem> lstCart = GetCartItems();
			ViewBag.Total = TotalQuantity();
			ViewBag.TotalPrice = TotalPrice();

			return View(lstCart);
		}


		public ActionResult OrderProduct(FormCollection collection)
		{
			Order order = new Order();
			User user = (User)Session["User"];
			List<CartItem> lstCartItems = GetCartItems();
			order.user_id = user.user_id;
			order.order_date = DateTime.Now;
			var deliverydate = string.Format("{0:MM/dd/yyyy}", collection["order_status"]);
			order.order_status = "chưa hoàn thành";
			order.shipping_address = user.address;
			order.total_price = (decimal)TotalPrice();
			db.Orders.Add(order);
			db.SaveChanges();
			foreach (var item in lstCartItems)
			{
				OrderDetail orderDetail = new OrderDetail();
				orderDetail.product_id = item.product_id;
				orderDetail.order_id = order.order_id;
				orderDetail.unit_price = (decimal)TotalPrice();
				orderDetail.detail_quantity = item.Quantity;
				db.OrderDetails.Add(orderDetail);
			}
			db.SaveChanges();
			Session["CartItems"] = null;
			return RedirectToAction("ComfirmOrder", "ShoppingCart");
		}
		public ActionResult ComfirmOrder()
		{
			return View();
		}

	}
}