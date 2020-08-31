using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SKShoeAndSports.Models
{
    public class Size
    {
        public int Id { get; set; }
        [MaxLength(10)]
        [Required]
        public string Name { get; set; }

        public ICollection<ProductVariant> ProductVariants { get; set; } 
    }
}
