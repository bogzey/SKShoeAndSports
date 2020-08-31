using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using SKShoeAndSports.DataAccess.Repository.IRepository;
using SKShoeAndSports.Models;
using SKShoeAndSports.Utility;

namespace SKShoeAndSports.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Admin_Role + "," + SD.Staff_Role)]
    public class SizeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IToastNotification _toastNotifcation;

        public SizeController(IUnitOfWork unitOfWork, IToastNotification toastNotification)
        {
            _unitOfWork = unitOfWork;
            _toastNotifcation = toastNotification;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Size size = new Size();

            if (id == null)
            {
                // create
                return View(size);
            }

            // edit
            size = _unitOfWork.Size.Get(id.GetValueOrDefault());
            if (size == null)
            {
                // Alert
                _toastNotifcation.AddAlertToastMessage("Size does not exist");
                return NotFound();
            }
            return View(size);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Size size)
        {
            if (ModelState.IsValid)
            {
                if (size.Id == 0)
                {
                    // Alert
                    _toastNotifcation.AddSuccessToastMessage($"{size.Name} was added successfully");
                    _unitOfWork.Size.Add(size);
                }
                else
                {
                    // Alert
                    _toastNotifcation.AddSuccessToastMessage($"{size.Name} was updated successfully");
                    _unitOfWork.Size.Update(size);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(size);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Size.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Size.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Size.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }

        #endregion
    }
}
