using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SKShoeAndSports.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SKShoeAndSports.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Colour>().HasData(
                    new Colour { Id = 1, Name = "Red" },
                    new Colour { Id = 2, Name = "Blue" },
                    new Colour { Id = 3, Name = "Green" },
                    new Colour { Id = 4, Name = "Yellow" },
                    new Colour { Id = 5, Name = "Purple" },
                    new Colour { Id = 6, Name = "Black" },
                    new Colour { Id = 7, Name = "White" },
                    new Colour { Id = 8, Name = "Grey" },
                    new Colour { Id = 9, Name = "Orange" },
                    new Colour { Id = 10, Name = "Brown" },
                    new Colour { Id = 11, Name = "Grey" }
            );

            builder.Entity<Category>().HasData(
                    new Category { Id = 1, Name = "Men" },
                    new Category { Id = 2, Name = "Women" },
                    new Category { Id = 3, Name = "Children" },
                    new Category { Id = 4, Name = "Schoolwear" }
            );

            builder.Entity<Brand>().HasData(
                    new Brand { Id = 1, Name = "Adidas" },
                    new Brand { Id = 2, Name = "Nike" },
                    new Brand { Id = 3, Name = "Skechers" },
                    new Brand { Id = 4, Name = "Wrangler" },
                    new Brand { Id = 5, Name = "Puma" },
                    new Brand { Id = 6, Name = "Umbro" },
                    new Brand { Id = 7, Name = "Canterbury" },
                    new Brand { Id = 8, Name = "Hype" },
                    new Brand { Id = 9, Name = "Aasics" },
                    new Brand { Id = 10, Name = "Susst" },
                    new Brand { Id = 11, Name = "Oaktrack" },
                    new Brand { Id = 12, Name = "Outrage" },
                    new Brand { Id = 13, Name = "Remonte" },
                    new Brand { Id = 14, Name = "Marco Tozzi" },
                    new Brand { Id = 15, Name = "Lunar Sandals" },
                    new Brand { Id = 16, Name = "Reiker" },
                    new Brand { Id = 17, Name = "Sponge" },
                    new Brand { Id = 18, Name = "Zanni" }
                );

            builder.Entity<Subcategory>().HasData(
                    new Subcategory { Id = 1, Name = "Accessories" },
                    new Subcategory { Id = 2, Name = "Skirts" },
                    new Subcategory { Id = 3, Name = "Shirts" },
                    new Subcategory { Id = 4, Name = "Tops" },
                    new Subcategory { Id = 5, Name = "Footwear" },
                    new Subcategory { Id = 6, Name = "Knitwear" },
                    new Subcategory { Id = 7, Name = "Outerwear" },
                    new Subcategory { Id = 8, Name = "Sports" }
                );

            builder.Entity<ProductType>().HasData(
                    new ProductType { Id = 1, Name = "Football Boots" },
                    new ProductType { Id = 2, Name = "Sandals" },
                    new ProductType { Id = 3, Name = "Heels" },
                    new ProductType { Id = 4, Name = "Trainers" },
                    new ProductType { Id = 5, Name = "Ballerinas" },
                    new ProductType { Id = 6, Name = "Slippers" },
                    new ProductType { Id = 7, Name = "Brogues" },
                    new ProductType { Id = 8, Name = "Coat" },
                    new ProductType { Id = 9, Name = "Joggers" },
                    new ProductType { Id = 10, Name = "Jeans" },
                    new ProductType { Id = 11, Name = "Jumpers" },
                    new ProductType { Id = 12, Name = "Polos" },
                    new ProductType { Id = 13, Name = "T-Shirts" },
                    new ProductType { Id = 14, Name = "Shirts" },
                    new ProductType { Id = 16, Name = "Schoolbag" },
                    new ProductType { Id = 17, Name = "Jackets" },
                    new ProductType { Id = 18, Name = "Hoodies" },
                    new ProductType { Id = 19, Name = "Vests" },
                    new ProductType { Id = 20, Name = "Tracksuits" }
                );

            builder.Entity<Size>().HasData(
                    new Size { Id = 1, Name = "XS" },
                    new Size { Id = 2, Name = "S" },
                    new Size { Id = 3, Name = "M" },
                    new Size { Id = 4, Name = "XL" },
                    new Size { Id = 5, Name = "L" },
                    new Size { Id = 6, Name = "XXL" },
                    new Size { Id = 7, Name = "1" },
                    new Size { Id = 8, Name = "2" },
                    new Size { Id = 9, Name = "3" },
                    new Size { Id = 10, Name = "4" },
                    new Size { Id = 11, Name = "5" },
                    new Size { Id = 12, Name = "6" },
                    new Size { Id = 13, Name = "7" },
                    new Size { Id = 14, Name = "8" },
                    new Size { Id = 15, Name = "9" },
                    new Size { Id = 16, Name = "10" },
                    new Size { Id = 17, Name = "11" },
                    new Size { Id = 18, Name = "12" },
                    new Size { Id = 19, Name = "13" },
                    new Size { Id = 20, Name = "14" },
                    new Size { Id = 21, Name = "15" },
                    new Size { Id = 22, Name = "16" },
                    new Size { Id = 23, Name = "17" },
                    new Size { Id = 24, Name = "18" },
                    new Size { Id = 25, Name = "19" },
                    new Size { Id = 26, Name = "20" },
                    new Size { Id = 27, Name = "21" },
                    new Size { Id = 28, Name = "22" },
                    new Size { Id = 29, Name = "23" },
                    new Size { Id = 30, Name = "24" }
                );

            builder.Entity<ProductVariant>().HasData(
                    new ProductVariant { Id = 1, Price = 50, ProductId = 1, ColourId = 2, Quantity = 12, SizeId = 10 },
                    new ProductVariant { Id = 2, Price = 50, ProductId = 2, ColourId = 2, Quantity = 20, SizeId = 11 },
                    new ProductVariant { Id = 3, Price = 50, ProductId = 3, Quantity = 15, ColourId = 3, SizeId = 12 },
                    new ProductVariant { Id = 4, Price = 50, ProductId = 4, ColourId = 2, SizeId = 10, Quantity = 10 },
                    new ProductVariant { Id = 5, Price = 50, ProductId = 5, ColourId = 1, SizeId = 11, Quantity = 11 },
                    new ProductVariant { Id = 6, Price = 50, ProductId = 6, ColourId = 4, SizeId = 10, Quantity = 12 },
                    new ProductVariant { Id = 7, Price = 50, ProductId = 7, ColourId = 3, SizeId = 11, Quantity = 13 },
                    new ProductVariant { Id = 8, Price = 50, ProductId = 8, ColourId = 5, SizeId = 12, Quantity = 14 },
                    new ProductVariant { Id = 9, Price = 50, ProductId = 9, ColourId = 5, SizeId = 13, Quantity = 15 },
                    new ProductVariant { Id = 10, Price = 50, ProductId = 10, ColourId = 4, SizeId = 10, Quantity = 16 },
                    new ProductVariant { Id = 11, Price = 50, ProductId = 11, ColourId = 3, SizeId = 11, Quantity = 11 },
                    new ProductVariant { Id = 12, Price = 50, ProductId = 12, ColourId = 5, SizeId = 12, Quantity = 12 },
                    new ProductVariant { Id = 13, Price = 50, ProductId = 13, ColourId = 5, SizeId = 13, Quantity = 13 },
                    new ProductVariant { Id = 14, Price = 50, ProductId = 13, ColourId = 4, SizeId = 14, Quantity = 14 },
                    new ProductVariant { Id = 15, Price = 50, ProductId = 13, ColourId = 3, SizeId = 15, Quantity = 15 },
                    new ProductVariant { Id = 16, Price = 50, ProductId = 13, ColourId = 5, SizeId = 16, Quantity = 15 }

                );

            builder.Entity<Product>().HasData(
                    new Product { Id = 1, Name = "Predator", Description = "Suitable for astroturf, 3G", Price = 60, ImageUrl = @"/images/products/predator.jpg", CategoryId = 1, SubcategoryId = 5, ProductTypeId = 1, BrandId = 1 },
                    new Product { Id = 2, Name = "Joga", Description = "Suitable for astroturf, 3G", Price = 60, ImageUrl = @"/images/products/joga.jpg", CategoryId = 1, SubcategoryId = 5, ProductTypeId = 1, BrandId = 1 },
                    new Product { Id = 3, Name = "Bootcut", Description = "Bootcut Jeans", Price = 60, ImageUrl = @"/images/products/bootcut.jpg", CategoryId = 1, SubcategoryId = 7, ProductTypeId = 14, BrandId = 13 },
                    new Product { Id = 4, Name = "Boots", Description = "Perfect for outdoors", Price = 60, ImageUrl = @"/images/products/boots.jpg", CategoryId = 1, SubcategoryId = 5, ProductTypeId = 9, BrandId = 12 },
                    new Product { Id = 5, Name = "Comics", Description = "Comic strip football boots", Price = 60, ImageUrl = @"/images/products/comics.jpg", CategoryId = 1, SubcategoryId = 5, ProductTypeId = 1, BrandId = 6 },
                    new Product { Id = 6, Name = "Loafers", Description = "Comfortable streamlined fit", Price = 60, ImageUrl = @"/images/products/loafers.jpg", CategoryId = 1, SubcategoryId = 5, ProductTypeId = 9, BrandId = 11 },
                    new Product { Id = 7, Name = "Slip-Ons", Description = "Slip on trainers", Price = 60, ImageUrl = @"/images/products/slipons.jpg", CategoryId = 2, SubcategoryId = 5, ProductTypeId = 6, BrandId = 17 },
                    new Product { Id = 8, Name = "Zipps", Description = "Zippped Trainers", Price = 60, ImageUrl = @"/images/products/zipps.jpg", CategoryId = 2, SubcategoryId = 5, ProductTypeId = 5, BrandId = 17 },
                    new Product { Id = 9, Name = "Silvers", Description = "Silver zipped trainers", Price = 60, ImageUrl = @"/images/products/silvers2.jpg", CategoryId = 2, SubcategoryId = 5, ProductTypeId = 5, BrandId = 17 },
                    new Product { Id = 10, Name = "Snake Eater", Description = "Snake patterned heels with strap", Price = 60, ImageUrl = @"/images/products/snake.jpg", CategoryId = 2, SubcategoryId = 5, ProductTypeId = 4, BrandId = 17 },
                    new Product { Id = 11, Name = "Gel Saviour", Description = "Trainers with a comfortable gel sole", Price = 60, ImageUrl = @"/images/products/gel.jpg", CategoryId = 2, SubcategoryId = 5, ProductTypeId = 5, BrandId = 10 },
                    new Product { Id = 12, Name = "Platform Push", Description = "Trainers with a platform sole", Price = 60, ImageUrl = @"/images/products/platform.jpg", CategoryId = 2, SubcategoryId = 5, ProductTypeId = 5, BrandId = 17 },
                    new Product { Id = 13, Name = "Slippy Slip", Description = "Comfortable ergonomic slip-ons", Price = 60, ImageUrl = @"/images/products/slippy.jpg", CategoryId = 2, SubcategoryId = 5, ProductTypeId = 5, BrandId = 18 }

                );

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Colour> Colours { get; set; }

        
    }
}
