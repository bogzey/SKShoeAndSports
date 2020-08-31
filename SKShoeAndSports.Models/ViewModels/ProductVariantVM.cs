using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace SKShoeAndSports.Models.ViewModels
{
    public class ProductVariantVM
    {
        public ProductVariant ProductVariant { get; set; }
        public int ProductId { get; set; }
        public Brand Brand { get; set; }
        public Product Product { get; set; }
        public IEnumerable<SelectListItem> SizeList { get; set; }
        public IEnumerable<SelectListItem> ColourList { get; set; }
        public IEnumerable<SelectListItem> ProductList { get; set; }

    }
}
