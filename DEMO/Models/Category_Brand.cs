namespace DEMO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Category_Brand
    {
        [Key]
        public int categorybrand_id { get; set; }

        public int? category_id { get; set; }

        public int? brand_id { get; set; }

        public virtual Brand Brand { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }
    }
}
