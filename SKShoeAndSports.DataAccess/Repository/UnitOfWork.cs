using SKShoeAndSports.DataAccess.Data;
using SKShoeAndSports.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace SKShoeAndSports.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Product = new ProductRepository(_db);
            Brand = new BrandRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            Staff = new StaffRepository(_db);
            Basket = new BasketRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
            OrderDetails = new OrderDetailsRepository(_db);
            Size = new SizeRepository(_db);
            Subcategory = new SubcategoryRepository(_db);
            Colour = new ColourRepository(_db);
            Category = new CategoryRepository(_db);
            ProductVariant = new ProductVariantRepository(_db);
            ProductType = new ProductTypeRepository(_db);
        }
        public ICategoryRepository Category { get; private set; }
        public IProductVariantRepository ProductVariant { get; private set; }
        public IProductTypeRepository ProductType { get;  private set; }
        public IColourRepository Colour { get; private set; }
        public ISubcategoryRepository Subcategory { get; private set; }
        public ISizeRepository Size { get; private set; }
        public IStaffRepository Staff { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public IProductRepository Product { get; private set; }
        public IBrandRepository Brand { get; private set; }
        public IBasketRepository Basket { get; private set; }
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailsRepository OrderDetails { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
