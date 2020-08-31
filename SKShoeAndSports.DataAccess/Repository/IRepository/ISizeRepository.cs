using SKShoeAndSports.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SKShoeAndSports.DataAccess.Repository.IRepository
{
    public interface ISizeRepository : IRepository<Size>
    {
        void Update(Size size);
    }
}
