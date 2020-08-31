using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
    public class ProductController : BaseController
    {
        private readonly IToastNotification _toastNotification;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IProductService _productService;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment, 
            IProductService productService, IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
            _productService = productService;
            
        }
        public IActionResult Index(string orderby)
        {
            ViewBag.ProductIdParam = string.IsNullOrEmpty(orderby) ? "id_desc" : "";
            ViewBag.CategoryParam = orderby == "category" ? "category_desc" : "category";
            ViewBag.ProductTypeParam = orderby == "productType" ? "productType_desc" : "productType";
            ViewBag.SubcategoryParam = orderby == "subcategory" ? "subcategory_desc" : "subcategory";
            var products = _productService.GetAllProducts();

            switch (orderby)
            {
                case "id_desc":
                    products = products.OrderByDescending(p => p.Id).ToList();
                    break;
                case "category":
                    products = products.OrderBy(p => p.Category.Name).ToList();
                    break;
                case "category_desc":
                    products = products.OrderByDescending(p => p.Category.Name).ToList();
                    break;
                case "subcategory":
                    products = products.OrderBy(p => p.Subcategory.Name).ToList();
                    break;
                case "subcategory_desc":
                    products = products.OrderByDescending(p => p.Subcategory.Name).ToList();
                    break;
                case "productType":
                    products = products.OrderBy(p => p.ProductType.Name).ToList();
                    break;
                case "productType_desc":
                    products = products.OrderByDescending(p => p.ProductType.Name).ToList();
                    break;
                default:
                    products = products.OrderBy(p => p.Id).ToList();
                    break;
            }

            return View(products);
        }

        public IActionResult Details(int id)
        {
            var product = _productService.GetProductById(id);
            var productVariant = _productService.GetProductVariantById(product.Id);

            if (product == null)
            {
                _toastNotification.AddAlertToastMessage("Product was not found");
                return NotFound();
            }
            return View(product);
        }

        [HttpGet]
        public IActionResult DeleteProductVariant(int id)
        {
            // load the variant
            var variant = _productService.GetProductVariantById(id);

            if(variant == null)
            {
                // Alert if variant was not found
                _toastNotification.AddAlertToastMessage("This variant does not exist");
                return NotFound();
            }

            // have variant passed to the view to delete
            return View(variant);
        }

        [HttpPost]
        public IActionResult ConfirmDeleteProductVariant(int id)
        {
            // Get variant's related product id
            var variant = _productService.GetProductVariantById(id);
            var product = _productService.GetProductById(variant.Id);

            // Delete variant
            _productService.DeleteProductVariant(id);
            _toastNotification.AddSuccessToastMessage("Product variant was deleted successfully");

            return RedirectToAction("Details", "Product", product);
        }

        public IActionResult Delete(int id)
        {
            // retrieve product
            var product = _productService.GetProductById(id);

            if (product == null)
            {
                _toastNotification.AddAlertToastMessage("Product does not exist");
            }

            // have product passed to view to delete
            return View(product);
        }

        public IActionResult ConfirmDelete(int id)
        {
            // have product deleted
            _productService.DeleteProduct(id);

            // have user redirected to product index
            _toastNotification.AddAlertToastMessage("Product was deleted successfully");
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                BrandList = _unitOfWork.Brand.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                SubcategoryList = _unitOfWork.Subcategory.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                ProductTypeList = _unitOfWork.ProductType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                })
            };

            if (id == null)
            {
                // create
                return View(productVM);
            }

            // edit
            productVM.Product = _unitOfWork.Product.Get(id.GetValueOrDefault());
            if (productVM.Product == null)
            {
                _toastNotification.AddAlertToastMessage("Product does not exist");
                return NotFound();
            }
            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\products");
                    var extenstion = Path.GetExtension(files[0].FileName);

                    if (productVM.Product.ImageUrl != null)
                    {
                        // this is an edit and need to remove old image
                        var imagePath = Path.Combine(webRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }
                    using (var filesStreams = new FileStream(Path.Combine(uploads, fileName + extenstion), FileMode.Create))
                    {
                        files[0].CopyTo(filesStreams);
                    }
                    productVM.Product.ImageUrl = @"\images\products\" + fileName + extenstion;

                }
                else
                {
                    // update when you do not change the image
                    if (productVM.Product.Id != 0)
                    {
                        Product objFromDb = _unitOfWork.Product.Get(productVM.Product.Id);
                        productVM.Product.ImageUrl = objFromDb.ImageUrl;
                    }
                }

                if (productVM.Product.Id == 0)
                {
                    _toastNotification.AddSuccessToastMessage("Product was successfully created");
                    _unitOfWork.Product.Add(productVM.Product);

                }
                else
                {
                    _toastNotification.AddSuccessToastMessage("Product was successfully updated");
                    _unitOfWork.Product.Update(productVM.Product);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                productVM.BrandList = _unitOfWork.Brand.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                productVM.SubcategoryList = _unitOfWork.Subcategory.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                productVM.ProductTypeList = _unitOfWork.ProductType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                if (productVM.Product.Id != 0)
                {
                    productVM.Product = _unitOfWork.Product.Get(productVM.Product.Id);
                }
            }

            return View(productVM);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            /*var allObj = _unitOfWork.Product.GetAll(includeProperties: "Brand,Category,Subcategory,ProductType");*/
            var allObj = _unitOfWork.Product.GetAllProducts();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult DeleteProduct(int id)
        {
            var objFromDb = _unitOfWork.Product.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Product.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete successful" });
        }
        #endregion

    }
}
