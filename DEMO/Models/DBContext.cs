using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace DEMO.Models
{
	public partial class DBContext : DbContext
	{
		public DBContext()
			: base("name=TechShopDB")
		{
		}

		public virtual DbSet<Brand> Brands { get; set; }
		public virtual DbSet<CartItem> CartItems { get; set; }
		public virtual DbSet<Cart> Carts { get; set; }
		public virtual DbSet<Category_Brand> Category_Brand { get; set; }
		public virtual DbSet<OrderDetail> OrderDetails { get; set; }
		public virtual DbSet<Order> Orders { get; set; }
		public virtual DbSet<ProductCategory> ProductCategories { get; set; }
		public virtual DbSet<Product> Products { get; set; }
		public virtual DbSet<Role> Roles { get; set; }
		public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
		public virtual DbSet<User> Users { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Cart>()
				.HasMany(e => e.CartItems)
				.WithRequired(e => e.Cart)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<OrderDetail>()
				.Property(e => e.unit_price)
				.HasPrecision(10, 2);

			modelBuilder.Entity<Order>()
				.Property(e => e.total_price)
				.HasPrecision(10, 2);

			modelBuilder.Entity<Product>()
				.Property(e => e.product_price)
				.HasPrecision(10, 2);

			modelBuilder.Entity<Product>()
				.Property(e => e.product_image)
				.IsUnicode(false);

			modelBuilder.Entity<Product>()
				.HasMany(e => e.CartItems)
				.WithRequired(e => e.Product)
				.WillCascadeOnDelete(false);

			modelBuilder.Entity<Role>()
				.Property(e => e.role_name)
				.IsUnicode(false);

			modelBuilder.Entity<User>()
				.Property(e => e.user_name)
				.IsUnicode(false);

			modelBuilder.Entity<User>()
				.Property(e => e.password)
				.IsUnicode(false);

			modelBuilder.Entity<User>()
				.Property(e => e.email)
				.IsUnicode(false);

			modelBuilder.Entity<User>()
				.Property(e => e.phone)
				.IsUnicode(false);

			modelBuilder.Entity<User>()
				.Property(e => e.password_comfirm)
				.IsUnicode(false);

			modelBuilder.Entity<User>()
				.HasMany(e => e.Carts)
				.WithRequired(e => e.User)
				.HasForeignKey(e => e.UserID)
				.WillCascadeOnDelete(false);
		}
	}
}
