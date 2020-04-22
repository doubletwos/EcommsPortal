using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcommsPortal.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; } 

        public decimal Price { get; set; }

        public int ProductTypeId { get; set; }

        public ProductType ProductType { get; set; }

        public int ProductCategoryId { get; set; }

        public ProductCategory ProductCategory { get; set; }

        public int ProductSizeId { get; set; }

        public ProductSize ProductSizes { get; set; }

        public virtual ICollection<File> Files { get; set; }




    }
}