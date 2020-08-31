using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SKShoeAndSports.Models
{
    public class ProductVariant
    {
        [Key]
        public int Id { get; set; }

        // Foreign keys relating to primary key of product model
        [Required]
        public int ProductId { get; set; }
        public int? SizeId { get; set; }
        public int? ColourId { get; set; }
        
        
        [Required]
        public int Quantity { get; set;}
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal DiscountPrice { get; set; }
        public bool isDiscount { get; set; }
        // Navigation property to navigate to product/colour/size classes
       

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        [ForeignKey("ColourId")]
        public Colour Colour { get; set; }
        [ForeignKey("SizeId")]
        public Size Size { get; set; }


    }
}
