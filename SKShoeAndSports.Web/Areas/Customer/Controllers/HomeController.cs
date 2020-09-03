using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NToastNotify;
using SKShoeAndSports.DataAccess.Repository.IRepository;
using SKShoeAndSports.Models;
using SKShoeAndSports.Models.ViewModels;
using SKShoeAndSports.Services.Interface;
using SKShoeAndSports.Utility;
using SKShoeAndSports.Web.Areas.Admin.Controllers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;

namespace SKShoeAndSports.Web.Controllers
{
    [Area("Customer")]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductService _productService;
        private readonly IToastNotification _toastNotification;

        [BindProperty]
        public ProductVM ProductVM { get; set; }
        public CategoryVM CategoryVM { get; set; }

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, IProductService productService, IToastNotification toastNotification)
        {
            _productService = productService;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _toastNotification = toastNotification;

        }

        public IActionResult Index()
        {
            ProductVM = new ProductVM()
            {
                ProductList = _unitOfWork.Product.GetAll(includeProperties: "Brand,Category,Subcategory,ProductType")
            };

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            
            // Check if user is logged in and retrieve basket
            if(claim != null)
            {
                var count = _unitOfWork.Basket.GetAll(i => i.ApplicationUserId == claim.Value)
                    .ToList().Count();

                HttpContext.Session.SetInt32(SD.SessionBasket, count);
            }

            return View(ProductVM);
        }

        public IActionResult CategoryList(string categoryName)
        {
            var categories = _unitOfWork.Category.GetAll();

            bool categoryExist = categories.Any(c => c.Name == categoryName);

            if (!categoryExist)
            {
                // Alert 
                _toastNotification.AddWarningToastMessage($"{categoryName} is not available");
                return RedirectToAction(nameof(Index));
            }

            var category = _unitOfWork.Category.GetFirstOrDefault(i => i.Name == categoryName);

            if (category == null)
            {
                _toastNotification.AddWarningToastMessage($"{categoryName} is not available");
                return RedirectToAction(nameof(Index));
            }

            var products = _unitOfWork.Product.GetAll(includeProperties: "Brand,Category,ProductType,Subcategory");

            bool productExist = products.Any(i => i.Category == category);

            if (!productExist)
            {
                _toastNotification.AddWarningToastMessage($"No stock currently available for: {categoryName}");
                return RedirectToAction(nameof(Index));
            }

            var productList = products.Where(x => x.Category == category);

            ViewBag.CurrentCategory = category.Name;

            ProductVM = new ProductVM()
            {
                ProductList = productList
            };

            return View(ProductVM);


        }

        public IActionResult BrandList(string brandName)
        {
            var brands = _unitOfWork.Brand.GetAll();

            bool brandExist = brands.Any(c => c.Name == brandName);

            if (!brandExist)
            {
                _toastNotification.AddWarningToastMessage($"{brandName} is not available");
                return RedirectToAction(nameof(Index));
            }

            var brand = _unitOfWork.Brand.GetFirstOrDefault(i => i.Name == brandName);

            if (brand == null)
            {
                _toastNotification.AddWarningToastMessage($"{brandName} is not available");
                return RedirectToAction(nameof(Index));
            }

            var products = _unitOfWork.Product.GetAll(includeProperties: "Brand,Category,ProductType,Subcategory");

            bool productExist = products.Any(i => i.Brand == brand);

            if (!productExist)
            {
                _toastNotification.AddWarningToastMessage($"No stock currently available for: {brandName}");
                return RedirectToAction(nameof(Index));
            }

            var productList = products.Where(x => x.Brand == brand);

            ProductVM = new ProductVM()
            {
                ProductList = productList
            };

            ViewBag.CurrentBrand = brand.Name;

            return View(ProductVM);
        }

        public IActionResult SubcategoryList(string subcategoryName)
        {
            var subcategories = _unitOfWork.Subcategory.GetAll();

            bool subcategoryExist = subcategories.Any(c => c.Name == subcategoryName);

            if (!subcategoryExist)
            {
                _toastNotification.AddWarningToastMessage($"{subcategoryName} is not available");
                return RedirectToAction(nameof(Index));
            }

            var subcategory = _unitOfWork.Subcategory.GetFirstOrDefault(i => i.Name == subcategoryName);

            if (subcategory == null)
            {
                _toastNotification.AddWarningToastMessage($"{subcategoryName} is not available");
                return RedirectToAction(nameof(Index));
            }

            var products = _unitOfWork.Product.GetAll(includeProperties: "Brand,Category,ProductType,Subcategory");

            bool productExist = products.Any(i => i.Subcategory == subcategory);

            if (!productExist)
            {
                _toastNotification.AddWarningToastMessage($"No stock currently available for: {subcategoryName}");
                return RedirectToAction(nameof(Index));
            }

            var productList = products.Where(x => x.Subcategory == subcategory);

            ProductVM = new ProductVM()
            {
                ProductList = productList
            };

            ViewBag.CurrentSubcategory = subcategory.Name;

            return View(ProductVM);
        }

        public IActionResult ProductTypeList(string productTypeName)
        {
            var productTypes = _unitOfWork.ProductType.GetAll();

            bool productTypeExist = productTypes.Any(c => c.Name == productTypeName);

            if (!productTypeExist)
            {
                _toastNotification.AddWarningToastMessage($"{productTypeName} is not available");
                return RedirectToAction(nameof(Index));
            }

            var productType = _unitOfWork.ProductType.GetFirstOrDefault(i => i.Name == productTypeName);

            if (productType == null)
            {
                _toastNotification.AddWarningToastMessage($"{productTypeName} is not available");
                return RedirectToAction(nameof(Index));
            }

            var products = _unitOfWork.Product.GetAll(includeProperties: "Brand,Category,ProductType,Subcategory");

            bool productExist = products.Any(i => i.ProductType == productType);

            if (!productExist)
            {
                _toastNotification.AddWarningToastMessage($"No stock currently available for: {productTypeName}");
                return RedirectToAction(nameof(Index));
            }

            var productList = products.Where(x => x.ProductType == productType);

            ProductVM = new ProductVM()
            {
                ProductList = productList
            };

            ViewBag.CurrentProductType = productType.Name;

            return View(ProductVM);
        }

        public IActionResult Search(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery) || string.IsNullOrEmpty(searchQuery))
            {
                _toastNotification.AddWarningToastMessage("Please enter details in search box");
                return RedirectToAction(nameof(Index));
            }

            var searchedProducts = _productService.GetFilteredProducts(searchQuery);

            if (searchedProducts == null)
            {
                _toastNotification.AddWarningToastMessage($"No stock currently available for: {searchedProducts}");
                return RedirectToAction(nameof(Index));
            }

            IEnumerable<Product> productList = searchedProducts;

            ProductVM = new ProductVM()
            {
                ProductList = productList
            };

            return View(ProductVM);
        }

        public IActionResult ProductDetails(int id)
        {
            var product = _productService.GetProductById(id);

            ProductVM = new ProductVM()
            {
                Product = product
            };
            
            if (product == null)
            {
                // Alert
                _toastNotification.AddWarningToastMessage("Product not available");
                return NotFound();
            }
                        
            return View(ProductVM);
        }

        public IActionResult ProductVariantDetails(int id)
        {
            var variant = _productService.GetProductVariantById(id);

            if(variant == null)
            {
                // Alert
                _toastNotification.AddWarningToastMessage("Product not available");
                return NotFound();
            }

            Basket basketObj = new Basket()
            {
                ProductVariant = variant,
                ProductVariantId = variant.Id
            };

            return View(basketObj);
        }
       

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult ProductVariantDetails(Basket basketObj)
        {
            basketObj.Id = 0;
            if (ModelState.IsValid)
            {
                // add to cart
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                basketObj.ApplicationUserId = claim.Value;

                // Retrieve basket associated with user 
                Basket basketFromDb = _unitOfWork.Basket.GetFirstOrDefault(
                    i => i.ApplicationUserId == basketObj.ApplicationUserId && i.ProductVariantId == basketObj.ProductVariantId
                    , includeProperties: "ProductVariant");

                // Retrieve product variant and populate it with related product attributes
                var productVariant = _unitOfWork.ProductVariant.GetFirstOrDefault(i => i.Id == basketObj.ProductVariantId
                , includeProperties: "Product,Product.Brand,Product.Category,Product.Subcategory,Product.ProductType,Size,Colour");

                basketObj.ProductVariant = _unitOfWork.ProductVariant.Get(productVariant.Id);

                // Check if basket exists
                if (basketFromDb == null)
                {
                    // Check if basket quantity is not greater than stock available
                    if (basketObj.Quantity <= productVariant.Quantity) { 
                    // Record does not exist in database for that product for user
                    _unitOfWork.Basket.Add(basketObj);
                    
                    // Decrease product variant's quanitity when items are added to basket
                    productVariant.Quantity -= basketObj.Quantity;
                    }
                    // Ensure that a product is not oversold
                    else if (basketObj.Quantity > productVariant.Quantity)
                    {
                        _toastNotification.AddWarningToastMessage($"Max quantity available is: {productVariant.Quantity}");
                        return View(basketObj);
                    }
                }
                else
                {
                    // Check if basket quantity is not greater than stock available
                    if (basketObj.Quantity <= productVariant.Quantity) { 
                    basketFromDb.Quantity += basketObj.Quantity;
                    productVariant.Quantity -= basketObj.Quantity;
                    _unitOfWork.ProductVariant.Update(productVariant);
                    _unitOfWork.Basket.Update(basketFromDb);
                    }
                    // Ensure that a product is not oversold
                    else
                    {
                        _toastNotification.AddWarningToastMessage($"Max quantity available is: {productVariant.Quantity}");
                        return View(basketObj);
                    }

                }
                _unitOfWork.Save();

                // Number of products in cart
                var count = _unitOfWork.Basket.GetAll(i => i.ApplicationUserId == basketObj.ApplicationUserId)
                    .ToList().Count();

                // Set count for basket
                HttpContext.Session.SetInt32(SD.SessionBasket, count);
                

                return RedirectToAction(nameof(Index));
            }

            else
            {
                var product = _unitOfWork.ProductVariant.GetFirstOrDefault(i => i.Id == basketObj.ProductVariantId, includeProperties: "Product");
                Basket basket = new Basket()
                {
                    ProductVariant = product,
                    ProductVariantId = product.Id
                };
                return View(basket);
            }

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
