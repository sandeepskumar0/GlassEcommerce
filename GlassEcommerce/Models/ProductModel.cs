using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GlassEcommerce.Models
{
    public class ProductModel
    {
        public List<Category> cat { get; set; }
        public List<Product> pro { get; set; }
    }
}