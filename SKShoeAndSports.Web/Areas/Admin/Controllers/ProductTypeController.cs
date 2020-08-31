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
    public class ProductTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IToastNotification _toastNotification;

        public ProductTypeController(IUnitOfWork unitOfWork, IToastNotification toastNotification)
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
            ProductType ProductType = new ProductType();

            if (id == null)
            {
                // create
                return View(ProductType);
            }

            // edit
            ProductType = _unitOfWork.ProductType.Get(id.GetValueOrDefault());
            if (ProductType == null)
            {
                // Alert
                _toastNotification.AddAlertToastMessage("Product Type was not found");
                return NotFound();
            }
            return View(ProductType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductType productType)
        {
            if (ModelState.IsValid)
            {
                if (productType.Id == 0)
                {
                    // Alert 
                    _toastNotification.AddSuccessToastMessage($"{productType.Name} was successfully created");
                    _unitOfWork.ProductType.Add(productType);
                }
                else
                {
                    _toastNotification.AddSuccessToastMessage($"{productType.Name} was successfully updated");
                    _unitOfWork.ProductType.Update(productType);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(productType);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.ProductType.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.ProductType.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.ProductType.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }

        #endregion
    }
}
