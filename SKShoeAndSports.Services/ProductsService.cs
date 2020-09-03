using Microsoft.EntityFrameworkCore;
using SKShoeAndSports.DataAccess.Data;
using SKShoeAndSports.DataAccess.Repository.IRepository;
using SKShoeAndSports.Models;
using SKShoeAndSports.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SKShoeAndSports.Services
{
    public class ProductsService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        

        public ProductsService(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }

        

        public Product GetProductById(int id)
        {
            return _unitOfWork.Product.GetProductById(id);
        }

        public IEnumerable<Product> GetFilteredProducts(string searchQuery)
        {
            var queries = string.IsNullOrEmpty(searchQuery) ? null : 
                Regex.Replace(searchQuery, @"\s+", "").Trim().ToLower().Split(" ");

            // Return search result for product by Product Type/Category/Subcategory/Brand
            
            return GetAllProducts().Where(i => queries.Any(query => (i.Name.ToLower().Contains(query) 
            || (i.Brand.Name.ToLower().Contains(query)
            || (i.Category.Name.ToLower().Contains(query)
            || (i.ProductType.Name.ToLower().Contains(query)
            || (i.Subcategory.Name.ToLower().Contains(query))))))));
        }

     

        public Product AddProduct(Product p)
        {
            var product = new Product
            {
                Brand = p.Brand,
                Category = p.Category,
                Description = p.Description,
                Name = p.Name,
                ImageUrl = p.ImageUrl,
                Subcategory = p.Subcategory,
                Price = p.Price,
                ProductType = p.ProductType,
            };

            _unitOfWork.Product.Add(product);
            _unitOfWork.Save();
            return product;
        }

        public bool DeleteProduct(int id)
        {
            var p = _unitOfWork.Product.Get(id);

            if (p == null)
            {
                return false;
            }

            _unitOfWork.Product.Remove(p);
            _unitOfWork.Save();
            return true;
        }

        public IList<Product> GetAllProducts(string orderby = null)
        {
            var products = _db.Products
                .Include(p => p.ProductVariants)
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Subcategory)
                .Include(p => p.ProductType)
                .AsNoTracking()
                .ToList();

            return products;
        }

        public ProductVariant GetProductVariantById(int id)
        {
            var productVariant = _unitOfWork.ProductVariant.GetFirstOrDefault(i => i.Id == id);

            if (productVariant == null)
            {
                return null;
            }

            return _unitOfWork.ProductVariant.GetProductVariant(productVariant.Id);
        }

       

        public Product UpsertProduct(int id)
        {
            var product = _unitOfWork.Product.GetProductById(id);

            if (product == null)
            {
                _unitOfWork.Product.Add(product);
            }
            else
            {
                _unitOfWork.Product.Update(product);
            }

            _unitOfWork.Save();
            return product;
        }

        public bool DeleteProductVariant(int id)
        {
            var productVariant = _unitOfWork.ProductVariant.GetFirstOrDefault(i => i.Id == id);

            if (productVariant == null)
            {
                return false;
            }
            // Delete product variant and update database 
            _unitOfWork.ProductVariant.Remove(productVariant);
            _unitOfWork.Save();
            return true;
        }

        public IList<ProductVariant> GetAllProductVariants()
        {
            // return all variants of a product
            var productVariants = _db.ProductVariants
                .Include(p => p.Colour)
                .Include(p => p.Size)
                .Include(p => p.Product)
                .AsNoTracking()
                .ToList();

            return productVariants;
        }

        public ProductVariant AddProductVariant(ProductVariant p)
        {
            var product = GetProductById(p.ProductId);
            if (product == null)
            {
                return null;
            }

            // create new product variant
            var productVariant = new ProductVariant
            {
                ProductId = p.ProductId,
                Price = p.Price,
                DiscountPrice = p.DiscountPrice,
                Colour = p.Colour,
                Quantity = p.Quantity,
                Size = p.Size
            };

            _unitOfWork.ProductVariant.Add(productVariant);
            _unitOfWork.Save();
            return productVariant;
        }
        

    }
}
