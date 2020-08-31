using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Moq;
using SKShoeAndSports.DataAccess.Data;
using SKShoeAndSports.DataAccess.Repository.IRepository;
using SKShoeAndSports.Models;
using SKShoeAndSports.Services;
using SKShoeAndSports.Services.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SKShoeAndSports.Test
{
    public class ProductServiceTest
    {
        [Fact]
        public void ProductService_Test_GetProductById()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ProductService_Test_GetProductById")
                .Options;

            var productEntity = new Product() { Id = 1 , Name = "Product 1", Price = 100m };

            using (var context = new ApplicationDbContext(options))
            {
                context.Products.Add(productEntity);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.NotNull(service.ProductService.GetProductById(productEntity.Id));
            }
        }

        [Fact]
        public void ProductService_Test_GetAllProducts()
        {
            // arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ProductService_Test_GetAllProduct")
                .Options;

            var productEntities = new List<Product>
            {
                new Product() { Id = 1, Name = "Product 1", Price = 100, DiscountPrice = 200 },
                new Product() { Id = 2, Name = "Product 2", Price = 100, DiscountPrice = 400 },
                new Product() { Id = 3, Name = "Product 3", Price = 100, DiscountPrice = 400 }
            };

            using (var context = new ApplicationDbContext(options))
            {
                foreach (var product in productEntities)
                    context.Products.Add(product);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var service = new Service(context);
                // assert
                Assert.Equal(productEntities.Count, service.ProductService.GetAllProducts().Count);
            }
        }
    }
}
