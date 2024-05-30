namespace DEMO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Carts = new HashSet<Cart>();
            Orders = new HashSet<Order>();
        }

        [Key]
        public int user_id { get; set; }

        public int? role_id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên người dùng.")]
        [StringLength(50)]
        public string user_name { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu.")]
        [StringLength(255)]
        public string password { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên hiển thị.")]
        [StringLength(255)]
        public string fullname { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ email.")]
        [StringLength(255)]
        public string email { get; set; }


        [StringLength(20)]
        public string phone { get; set; }

        public string address { get; set; }

        public DateTime? create_at { get; set; }

        public DateTime? update_at { get; set; }

        [Required]
        [StringLength(255)]
        public string password_comfirm { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cart> Carts { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Orders { get; set; }

        public virtual Role Role { get; set; }
    }
}
