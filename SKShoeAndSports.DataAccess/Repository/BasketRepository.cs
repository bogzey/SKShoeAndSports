using Microsoft.EntityFrameworkCore;
using SKShoeAndSports.DataAccess.Data;
using SKShoeAndSports.DataAccess.Repository.IRepository;
using SKShoeAndSports.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKShoeAndSports.DataAccess.Repository
{
    public class BasketRepository : Repository<Basket>, IBasketRepository
    {
        private readonly ApplicationDbContext _db;

        public BasketRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Basket basket)
        {
            _db.Update(basket);
        }

        /*public Basket GetAllBasket(int id)
        {
            return _db.Baskets
                .Include(b => b.ProductVariant.Product).ThenInclude(b => b.Name)


        }*/
    }
}
