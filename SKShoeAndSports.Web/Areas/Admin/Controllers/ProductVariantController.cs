using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NToastNotify;
using SKShoeAndSports.DataAccess.Repository.IRepository;
using SKShoeAndSports.Models;
using SKShoeAndSports.Models.ViewModels;
using SKShoeAndSports.Services.Interface;
using SKShoeAndSports.Utility;

namespace SKShoeAndSports.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Admin_Role + "," + SD.Staff_Role)]
    public class ProductVariantController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductService _productService;
        private readonly IToastNotification _toastNotification;

        public ProductVariantController(IUnitOfWork unitOfWork, IProductService productService,
             IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
            _productService = productService;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        /*public IActionResult CreateProductVariant(int id)
        {
            var p = _productService.GetProductById(id);
            if (p == null)
            {
                // Alert
                Alert("Product does not exisit", AlertType.danger);
                return NotFound();
            }
            var variantViewModel = new ProductVariantVM { ProductId == id };
        }*/

        public IActionResult Upsert(int? id)
        {
            ProductVariantVM productVariantVM = new ProductVariantVM()
            {
                ProductVariant = new ProductVariant(),
                ProductList = _unitOfWork.Product.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()

                }),
                SizeList = _unitOfWork.Size.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()

                }),
                ColourList = _unitOfWork.Colour.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                
            };

            
            

            if (id == null)
            {
                // create
                return View(productVariantVM);
            }

            // edit
            productVariantVM.ProductVariant = _unitOfWork.ProductVariant.Get(id.GetValueOrDefault());
            if (productVariantVM.ProductVariant == null)
            {
                // Alert
                _toastNotification.AddAlertToastMessage("Product variant does not exist");
                return NotFound();
            }
            return View(productVariantVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVariantVM productVariantVM)
        {
            if (ModelState.IsValid)
            {
                if (productVariantVM.ProductVariant.Id == 0)
                {
                    // Alert 
                    _toastNotification.AddSuccessToastMessage("Product variant successfully created");
                    _unitOfWork.ProductVariant.Add(productVariantVM.ProductVariant);
                }
                else
                {
                    _toastNotification.AddSuccessToastMessage("Product variant successfully updated");
                    _unitOfWork.ProductVariant.Update(productVariantVM.ProductVariant);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                productVariantVM.ProductList = _unitOfWork.Product.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                productVariantVM.SizeList = _unitOfWork.Size.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                productVariantVM.ColourList = _unitOfWork.Colour.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });

                if (productVariantVM.ProductVariant.Id != 0)
                {
                    productVariantVM.ProductVariant = _unitOfWork.ProductVariant.Get(productVariantVM.ProductVariant.Id);
                }
            }
            return View(productVariantVM);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _productService.GetAllProductVariants();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.ProductVariant.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.ProductVariant.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }

        #endregion
    }
}
