using SKShoeAndSports.DataAccess.Data;
using SKShoeAndSports.DataAccess.Repository.IRepository;
using SKShoeAndSports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKShoeAndSports.DataAccess.Repository
{
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        private readonly ApplicationDbContext _db;

        public BrandRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Brand brand)
        {
            var objFromDb = _db.Brands.FirstOrDefault(s => s.Id == brand.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = brand.Name;
            }
        }
    }
}
