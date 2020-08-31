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
    public class SubcategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IToastNotification _toastNotification;
        public SubcategoryController(IUnitOfWork unitOfWork, IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Subcategory subcategory = new Subcategory();

            if (id == null)
            {
                // create
                return View(subcategory);
            }

            // edit
            subcategory = _unitOfWork.Subcategory.Get(id.GetValueOrDefault());
            if (subcategory == null)
            {
                // Alert 
                _toastNotification.AddAlertToastMessage("This subcategory does not exist");
                return NotFound();
            }
            return View(subcategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Subcategory subcategory)
        {
            if (ModelState.IsValid)
            {
                if (subcategory.Id == 0)
                {
                    // Alert
                    _toastNotification.AddSuccessToastMessage($"{subcategory.Name} was successfully created");
                    _unitOfWork.Subcategory.Add(subcategory);
                }
                else
                {
                    _toastNotification.AddSuccessToastMessage($"{subcategory.Name} was successfully updated");
                    _unitOfWork.Subcategory.Update(subcategory);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(subcategory);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Subcategory.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Subcategory.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Subcategory.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }

        #endregion
    }
}
