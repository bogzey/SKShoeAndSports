using SKShoeAndSports.DataAccess.Data;
using SKShoeAndSports.DataAccess.Repository.IRepository;
using SKShoeAndSports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKShoeAndSports.DataAccess.Repository
{
    public class ProductTypeRepository : Repository<ProductType>, IProductTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ProductType produuctType)
        {
            _db.Update(produuctType);
        }
    }
}
