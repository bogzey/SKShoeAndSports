using SKShoeAndSports.DataAccess.Data;
using SKShoeAndSports.DataAccess.Repository.IRepository;
using SKShoeAndSports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKShoeAndSports.DataAccess.Repository
{
    public class ColourRepository : Repository<Colour>, IColourRepository
    {
        private readonly ApplicationDbContext _db;

        public ColourRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Colour colour)
        {
            _db.Update(colour);
        }
    }
}
