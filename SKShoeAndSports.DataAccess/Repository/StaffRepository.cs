using SKShoeAndSports.DataAccess.Data;
using SKShoeAndSports.DataAccess.Repository.IRepository;
using SKShoeAndSports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKShoeAndSports.DataAccess.Repository
{
    public class StaffRepository : Repository<Staff>, IStaffRepository
    {
        private readonly ApplicationDbContext _db;

        public StaffRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Staff staff)
        {
            _db.Update(staff);
            _db.SaveChanges();
        }
    }
}
