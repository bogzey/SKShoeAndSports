using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace SKShoeAndSports.Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }
        public IEnumerable<Product> ProductList { get; set; }
        public int Quantity { get; set; }
        public ProductVariant ProductVariant { get; set; }
        public IEnumerable<SelectListItem> BrandList { get; set; }

        public IEnumerable<SelectListItem> CategoryList { get; set; }

        public IEnumerable<SelectListItem> SubcategoryList { get; set; }

        public IEnumerable<SelectListItem> ProductTypeList { get; set; }

        public string SearchQuery { get; set; }

        public Paging Paging { get; set; }
    }
}
