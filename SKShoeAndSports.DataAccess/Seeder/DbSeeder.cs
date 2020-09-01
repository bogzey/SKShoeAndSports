using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SKShoeAndSports.DataAccess.Data;
using SKShoeAndSports.Models;
using SKShoeAndSports.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SKShoeAndSports.DataAccess.Seeder
{
    public class DbSeeder : IDbSeeder
    {
        private readonly ApplicationDbContext _db;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public DbSeeder(ApplicationDbContext db, UserManager<IdentityUser> userManager,
                                                RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public void Initaliser()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception e)
            {

            }

            if (_db.Roles.Any(i => i.Name == SD.Admin_Role)) return;

            _roleManager.CreateAsync(new IdentityRole(SD.Admin_Role)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Customer_Role)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(SD.Staff_Role)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@hotmail.co.uk",
                Email = "admin@hotmail.co.uk",
                EmailConfirmed = true,
                Name = "Joshua Bogle",
            }, "Password123?").GetAwaiter().GetResult();

            ApplicationUser user = _db.ApplicationUsers.Where(u => u.Email == "admin@hotmail.co.uk").FirstOrDefault();

            _userManager.AddToRoleAsync(user, SD.Admin_Role).GetAwaiter().GetResult();
        }
    }
}
