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
    public class ColourController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IToastNotification _toastNotification;

        public ColourController(IUnitOfWork unitOfWork, IToastNotification toastNotification)
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
            Colour colour = new Colour();

            if (id == null)
            {
                // create
                return View(colour);
            }

            // edit
            colour = _unitOfWork.Colour.Get(id.GetValueOrDefault());
            if (colour == null)
            {
                // Alert
                _toastNotification.AddAlertToastMessage("Colour was not found");
                return NotFound();
            }
            return View(colour);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Colour colour)
        {
            if (ModelState.IsValid)
            {
                if (colour.Id == 0)
                {
                    _toastNotification.AddSuccessToastMessage("Colour was succcessfully created");
                    _unitOfWork.Colour.Add(colour);
                }
                else
                {
                    _toastNotification.AddSuccessToastMessage("Colour was succcessfully updated");
                    _unitOfWork.Colour.Update(colour);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(colour);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Colour.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Colour.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Colour.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }

        #endregion
    }
}
