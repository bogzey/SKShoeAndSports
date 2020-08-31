﻿using SKShoeAndSports.DataAccess.Data;
using SKShoeAndSports.DataAccess.Repository.IRepository;
using SKShoeAndSports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKShoeAndSports.DataAccess.Repository
{
    public class SubcategoryRepository : Repository<Subcategory>, ISubcategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public SubcategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Subcategory subcategory)
        {
            _db.Update(subcategory);
        }
    }
}
