using Microsoft.EntityFrameworkCore;
using SKShoeAndSports.DataAccess.Data;
using SKShoeAndSports.DataAccess.Repository.IRepository;
using SKShoeAndSports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKShoeAndSports.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product product)
        {
            var productsFromDb = _db.Products.FirstOrDefault(s => s.Id == product.Id);

            if (productsFromDb != null)
            {
                if (product.ImageUrl != null)
                {
                    productsFromDb.ImageUrl = product.ImageUrl;
                }

                productsFromDb.CategoryId = product.CategoryId;
                productsFromDb.Quantity = product.Quantity;
                productsFromDb.BrandId = product.BrandId;
                productsFromDb.Price = product.Price;
                productsFromDb.ProductTypeId = product.ProductTypeId;
                productsFromDb.SubcategoryId = product.SubcategoryId;
                productsFromDb.Description = product.Description;
                productsFromDb.Name = product.Name;
            }
        }

        

        

        public IList<Product> GetAllProducts()
        {
            return _db.Products
                .Include(p => p.ProductVariants)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Subcategory)
                .Include(p => p.ProductType)
                .AsNoTracking()
                .ToList();
        }

        public Product GetProductById(int id)
        {
            return _db.Products
                .Include(p => p.ProductVariants).ThenInclude(p => p.Colour)
                .Include(p => p.ProductVariants).ThenInclude(p => p.Size)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Subcategory)
                .Include(p => p.ProductType)
                .AsNoTracking()
                .FirstOrDefault(i => i.Id == id);
        }

        



        
    }
}