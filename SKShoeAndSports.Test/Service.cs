using SKShoeAndSports.DataAccess.Data;
using SKShoeAndSports.DataAccess.Repository;
using SKShoeAndSports.Models;
using SKShoeAndSports.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace SKShoeAndSports.Test
{
    public class Service
    {
        public Service(ApplicationDbContext db)
        {
            UnitOfWork = new UnitOfWork(db);

            ProductService = new ProductsService(UnitOfWork, db);
        }

        public UnitOfWork UnitOfWork { get; set; }

        public ProductsService ProductService { get; set; }
    }
}
