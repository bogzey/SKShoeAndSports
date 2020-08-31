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
    public class BrandController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IToastNotification _toastNotification;

        public BrandController(IUnitOfWork unitOfWork, IToastNotification toastNotification)
        {
            _unitOfWork = unitOfWork;
            _toastNotification = toastNotification;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Brand brand = new Brand();

            if (id == null)
            {
                // create
                return View(brand);
            }

            // edit
            brand = _unitOfWork.Brand.Get(id.GetValueOrDefault());
            if (brand == null)
            {
                // Alert
                _toastNotification.AddAlertToastMessage("This brand does not exist");
                return NotFound();
            }
            return View(brand);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Brand brand)
        {
            if (ModelState.IsValid)
            {
                if (brand.Id == 0)
                {
                    _toastNotification.AddSuccessToastMessage($"{brand.Name} was successfully created");
                    _unitOfWork.Brand.Add(brand);
                }
                else
                {
                    _toastNotification.AddSuccessToastMessage($"{brand.Name} was successfully updated");
                    _unitOfWork.Brand.Update(brand);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Brand.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Brand.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Brand.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }

        #endregion
    }
}
