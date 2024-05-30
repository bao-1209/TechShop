using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DEMO.Models;

namespace DEMO.ViewModels
{
    public class UsersRoleViewModel
    {
        public List<Role> Roles { get; set; }
        public List<User> Users { get; set; }

    }

    public class Category_BrandViewModels
    {
        public List<ProductCategory> ProductCategories { get; set; }
        public List<Brand> Brands { get; set; }

        public List<Category_Brand> Category_Brands { get; set; }
    }







    public class RevenueReportViewModel
    {
        public decimal TotalRevenue { get; set; }
        public List<CategorySales> SalesByCategory { get; set; }
        public List<BrandSales> SalesByBrand { get; set; }
        public List<ProductSales> SalesByProduct { get; set; }
    }

    public class CategorySales
    {
        public string CategoryName { get; set; }
        public int TotalQuantity { get; set; }
    }

    public class BrandSales
    {
        public string BrandName { get; set; }
        public int TotalQuantity { get; set; }
    }

    public class ProductSales
    {
        public string ProductName { get; set; }
        public int TotalQuantity { get; set; }
    }

}
