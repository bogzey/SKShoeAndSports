using System;
using System.Collections.Generic;
using System.Text;

namespace SKShoeAndSports.Models.ViewModels
{
    public class OrderDetailsVM
    {
        public OrderHeader OrderHeader { get; set; }

        public IEnumerable<OrderDetails> OrderDetails { get; set; }

        public IEnumerable<ProductVariant> ProductVariants { get; set; }
    }
}
