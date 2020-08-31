using SKShoeAndSports.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SKShoeAndSports.DataAccess.Repository.IRepository
{
    public interface IProductVariantRepository : IRepository<ProductVariant>
    {
        void Update(ProductVariant productVariant);

        IList<ProductVariant> GetAllProductVariants();

        ProductVariant GetProductVariant(int id);
    }
}
