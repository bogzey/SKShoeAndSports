using SKShoeAndSports.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SKShoeAndSports.Services.Interface
{
    public interface IProductService
    {
        Product UpsertProduct(int id);

        bool DeleteProduct(int id);

        bool DeleteProductVariant(int id);

        IList<ProductVariant> GetAllProductVariants();

        ProductVariant GetProductVariantById(int id);

        ProductVariant AddProductVariant(ProductVariant pv);

        IList<Product> GetAllProducts(string orderby = null);

        IEnumerable<Product> GetFilteredProducts(string searchQuery);

        Product AddProduct(Product p);

        Product GetProductById(int id);


    }
}
