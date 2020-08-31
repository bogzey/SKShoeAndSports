using SKShoeAndSports.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SKShoeAndSports.DataAccess.Repository.IRepository
{
    public interface IStaffRepository : IRepository<Staff>
    {
        void Update(Staff staff);
    }
}
