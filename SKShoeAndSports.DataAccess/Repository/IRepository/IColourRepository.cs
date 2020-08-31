using SKShoeAndSports.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SKShoeAndSports.DataAccess.Repository.IRepository
{
    public interface IColourRepository : IRepository<Colour>
    {
        void Update(Colour colour);
    }
}
