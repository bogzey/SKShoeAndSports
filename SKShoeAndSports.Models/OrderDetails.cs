using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SKShoeAndSports.Models
{
    public class OrderDetails
    {
        [Key]
        public int Id { get; set; }

        // Navigation properties 1:1 

        [Required]
        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public OrderHeader OrderHeader { get; set; }
        
        [Required]
        public int ProductVariantId { get; set; }
        [ForeignKey("ProductVariantId")]
        public ProductVariant ProductVariant { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
    }
}
