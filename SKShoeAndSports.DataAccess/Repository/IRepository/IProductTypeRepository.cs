using SKShoeAndSports.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SKShoeAndSports.DataAccess.Repository.IRepository
{
    public interface IProductTypeRepository : IRepository<ProductType>
    {
        void Update(ProductType productType);
    }
}
