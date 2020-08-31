using System;
using System.Collections.Generic;
using System.Text;

namespace SKShoeAndSports.Models.ViewModels
{
    public class BasketVM
    {
        public IEnumerable<Basket> BasketList { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
