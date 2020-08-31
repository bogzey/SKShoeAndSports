using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SKShoeAndSports.DataAccess.Data;
using SKShoeAndSports.DataAccess.Repository.IRepository;
using SKShoeAndSports.Models;
using SKShoeAndSports.Utility;

namespace SKShoeAndSports.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Admin_Role)]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        /*public IActionResult Delete(int id)
        {
            var user = _db.ApplicationUsers.Find(id);
        }*/
       

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var userList = _db.ApplicationUsers.Include(u => u.Staff).ToList();
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();

            foreach (var user in userList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
                /*if (user.Staff == null)
                {
                    user.Staff = new Staff()
                    {
                        Name = ""
                    };
                }*/
            }

            return Json(new { data = userList });
        }

        /*[HttpPost]
        public IActionResult LockUnlock([FromBody] string id)
        {
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            if(objFromDb == null)
            {
                return Json(new { success = false, message = "Error while Locking/Unlocking" });
            }
            if(objFromDb.LockoutEnd != null && objFromDb.LockoutEnd > DateTime.Now)
            {
                // User is currently locked, unlock
                objFromDb.LockoutEnd = DateTime.Now.AddYears(1);
            }
            _db.SaveChanges();
            return Json(new { success = true, message = "Operation successful" });
        }*/
        [HttpDelete]
        public IActionResult Delete([FromBody] string id)
        {
            try
            {
                var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
                if (objFromDb == null)
                {
                    return Json(new { success = false, message = "Error while deleting" });
                }
                _db.ApplicationUsers.Remove(objFromDb);
                _db.SaveChanges();
            } catch (Exception e)
            {
                
            }
                return Json(new { success = true, message = "Delete successful" });
        }

        

        #endregion
    }
}
