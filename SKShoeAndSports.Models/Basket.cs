using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SKShoeAndSports.Models
{
    public class Basket
    {
        public Basket()
        {
            Quantity = 1;
        }

        [Key]
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }
        public ProductVariant ProductVariant { get; set; }
        // Foreign Keys
        public int ProductVariantId { get; set; }
        [ForeignKey("ProductVariantId")]
        [Range(1,1000, ErrorMessage = "Please enter a value between 1 and 1000")]
        public int Quantity { get; set; }
        [NotMapped]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }


    }
}
