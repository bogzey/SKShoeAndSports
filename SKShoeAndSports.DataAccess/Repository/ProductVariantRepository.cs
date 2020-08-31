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
    public class ProductVariantRepository : Repository<ProductVariant>, IProductVariantRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductVariantRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ProductVariant productVariant)
        {
            _db.Update(productVariant);
        }

        public IList<ProductVariant> GetAllProductVariants()
        {
            return _db.ProductVariants
                .Include(p => p.Product).ThenInclude(p => p.Id)
                .Include(p => p.Colour).ThenInclude(p => p.Id)
                .Include(p => p.Size).ThenInclude(p => p.Id)
                .AsNoTracking()
                .ToList();
        }

        public ProductVariant GetProductVariant(int id)
        {
            return _db.ProductVariants
                .Include(p => p.Product).ThenInclude(p => p.Brand)
                .Include(p => p.Product).ThenInclude(p => p.Category)
                .Include(p => p.Product).ThenInclude(p => p.ProductType)
                .Include(p => p.Product).ThenInclude(p => p.Subcategory)
                .Include(p => p.Colour)
                .Include(p => p.Size)
                .AsNoTracking()
                .FirstOrDefault(i => i.Id == id);
        }
    }
}
