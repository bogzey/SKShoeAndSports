using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SKShoeAndSports.Models
{
    public class Brand
    {
        public int Id { get; set; }
        [Display(Name = "Brand Name")]
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public ICollection<ProductVariant> ProductVariants { get; set; }
    }
}
