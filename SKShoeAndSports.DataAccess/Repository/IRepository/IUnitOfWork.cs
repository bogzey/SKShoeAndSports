using System;
using System.Collections.Generic;
using System.Text;

namespace SKShoeAndSports.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ISubcategoryRepository Subcategory { get; }
        IProductVariantRepository ProductVariant { get; }
        ISizeRepository Size { get; }
        ICategoryRepository Category { get; }
        IProductTypeRepository ProductType { get; }
        IColourRepository Colour { get;}
        IStaffRepository Staff { get; }
        IProductRepository Product { get; }
        IBrandRepository Brand { get; }
        IBasketRepository Basket { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IOrderDetailsRepository OrderDetails { get; }
        IApplicationUserRepository ApplicationUser { get; }
        void Save();
    }
}
