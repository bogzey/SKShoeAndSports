using SKShoeAndSports.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SKShoeAndSports.DataAccess.Repository.IRepository
{
    public interface ISubcategoryRepository : IRepository<Subcategory>
    {
        void Update(Subcategory subcategory);
    }
}
