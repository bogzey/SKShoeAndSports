using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SKShoeAndSports.DataAccess.Repository.IRepository;
using SKShoeAndSports.Models;
using SKShoeAndSports.Utility;

namespace SKShoeAndSports.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Admin_Role + "," + SD.Staff_Role)]
    public class StaffController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public StaffController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Staff staff = new Staff();

            if (id == null)
            {
                // create
                return View(staff);
            }

            // edit
            staff = _unitOfWork.Staff.Get(id.GetValueOrDefault());
            if (staff == null)
            {
                return NotFound();
            }
            return View(staff);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Staff staff)
        {
            if (ModelState.IsValid)
            {
                if (staff.Id == 0)
                {
                    _unitOfWork.Staff.Add(staff);

                }
                else
                {
                    _unitOfWork.Staff.Update(staff);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(staff);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Staff.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Staff.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Staff.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }

        #endregion
    }
}
