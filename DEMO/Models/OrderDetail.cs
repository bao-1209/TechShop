namespace DEMO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderDetail
    {
        [Key]
        public int detail_id { get; set; }

        public int? order_id { get; set; }

        public int? product_id { get; set; }

        public int? detail_quantity { get; set; }

        public decimal? unit_price { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }
}
