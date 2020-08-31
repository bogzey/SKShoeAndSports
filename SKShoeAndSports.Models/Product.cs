using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SKShoeAndSports.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        
        public string Description { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }
        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal DiscountPrice { get; set; }
        public bool IsDiscount { get; set; }
        public string ImageUrl { get; set; }

        // Foreign Keys
        public int BrandId { get; set; }
        
        public int ProductTypeId { get; set; }
        public int SubcategoryId { get; set; }
        public int CategoryId { get; set; }

        // Navigation Properties
        [ForeignKey("BrandId")]
        public Brand Brand { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        [ForeignKey("SubcategoryId")]
        public Subcategory Subcategory { get; set; }
        [ForeignKey("ProductTypeId")]
        public ProductType ProductType { get; set; }

        // Navigation properties 1:N
        public ICollection<ProductVariant> ProductVariants { get; set; }
    }
}
