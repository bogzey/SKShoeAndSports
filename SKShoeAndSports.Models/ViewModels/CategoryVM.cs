using System;
using System.Collections.Generic;
using System.Text;

namespace SKShoeAndSports.Models.ViewModels
{
    public class CategoryVM
    {
        public IEnumerable<Category> Categories { get; set; }
        public Paging Paging { get; set; }
    }
}
