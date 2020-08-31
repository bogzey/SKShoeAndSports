using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SKShoeAndSports.Models
{
    public class Category
    {
        public int Id { get; set; }
        [MaxLength(30)]
        [Required]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
