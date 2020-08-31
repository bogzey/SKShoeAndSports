using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SKShoeAndSports.Models
{
    public class Colour
    {
        public int Id { get; set; }
        [MaxLength(30)]
        [Required]
        public string Name { get; set; }

        // Navigation properties - 1:N
        public ICollection<ProductVariant> ProductVariants { get; set; }
    }
}
