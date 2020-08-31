using SKShoeAndSports.DataAccess.Data;
using SKShoeAndSports.DataAccess.Repository.IRepository;
using SKShoeAndSports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKShoeAndSports.DataAccess.Repository
{
    public class SizeRepository : Repository<Size>, ISizeRepository
    {
        private readonly ApplicationDbContext _db;

        public SizeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Size size)
        {
            _db.Update(size);
        }
    }
}
