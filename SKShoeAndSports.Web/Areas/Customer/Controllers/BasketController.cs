using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using NToastNotify;
using SKShoeAndSports.DataAccess.Repository.IRepository;
using SKShoeAndSports.Models;
using SKShoeAndSports.Models.ViewModels;
using SKShoeAndSports.Utility;
using Stripe;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace SKShoeAndSports.Web.Areas.Admin.Controllers
{
    [Area("Customer")]
    public class BasketController : Controller
    {
        private readonly IEmailSender _emailSender;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IToastNotification _toastNotification;
        private TwilioOptions _twilioOptions;

        [BindProperty]
        public BasketVM BasketVM { get; set; }

        public BasketController(IEmailSender emailSender, UserManager<IdentityUser> userManager, 
            IUnitOfWork unitOfWork ,IOptions<TwilioOptions> twilioOptions, 
            IToastNotification toastNotification)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _userManager = userManager;
            _twilioOptions = twilioOptions.Value;
            _toastNotification = toastNotification;
        }

        public IActionResult Index()
        {
            // Retrieve userID of logged in user
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);


            BasketVM = new BasketVM()
            {
                OrderHeader = new OrderHeader(),
                // Retrieve all items and add to cart
                BasketList = _unitOfWork.Basket.GetAll(i => i.ApplicationUserId == claim.Value,
                includeProperties: "ProductVariant,ProductVariant.Product,ProductVariant.Product.Brand")
            };
            BasketVM.OrderHeader.OrderTotal = 0;

            // Populate application user
            BasketVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser
                .GetFirstOrDefault(i => i.Id == claim.Value);

            foreach (var items in BasketVM.BasketList)
            {
                items.Price = items.ProductVariant.Price;
                BasketVM.OrderHeader.OrderTotal += (items.Price * items.Quantity);

                // Minimise the description for the basket view
                if (items.ProductVariant.Product.Description.Length > 100)
                {
                    items.ProductVariant.Product.Description = items.ProductVariant.Product.Description.Substring(0, 99) + "...";
                }
            }
            return View(BasketVM);
        }
        
        [HttpPost]
        [ActionName("Index")]
        public async Task<IActionResult> IndexPost()
        {
            // Retrieve userID of logged in user
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            //
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            // Populate application user
            var user = _unitOfWork.ApplicationUser.GetFirstOrDefault(i => i.Id == claim.Value);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Verification email is empty.");
            }

            // To confirm email 


            /*var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = user.Id, code = code,},
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");*/

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Plus(int id)
        {
            var basket = _unitOfWork.Basket.GetFirstOrDefault
                (b => b.Id == id, includeProperties: "ProductVariant");

            var productVariant = _unitOfWork.ProductVariant.Get(basket.ProductVariantId);
            if (basket.ProductVariant.Quantity != 0)
            {
                basket.Quantity += 1;
                basket.ProductVariant.Quantity -= 1;
            }
            else
            {
                _toastNotification.AddAlertToastMessage("No stock left");
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int id)
        {
            var basket = _unitOfWork.Basket.GetFirstOrDefault
                (b => b.Id == id, includeProperties: "ProductVariant");

            var productvariant = _unitOfWork.ProductVariant.Get(basket.ProductVariantId);
            var originalCount = productvariant.Quantity;

            

            if (basket.Quantity == 1)
            {
                var total = _unitOfWork.Basket.GetAll(i => i.ApplicationUserId == basket.ApplicationUserId).ToList().Count;
                basket.ProductVariant.Quantity += 1;
                _unitOfWork.Basket.Remove(basket);
                _unitOfWork.Save();
                HttpContext.Session.SetInt32(SD.SessionBasket, total - 1);
               
            }
            else if (basket.Quantity > 0)
            {
                
                basket.Quantity -= 1;
                basket.ProductVariant.Quantity += 1;
                _unitOfWork.Save();
            }
            
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int id)
        {
            var basket = _unitOfWork.Basket.GetFirstOrDefault
                (b => b.Id == id, includeProperties: "ProductVariant");

            var productVariant = _unitOfWork.ProductVariant.Get(basket.ProductVariantId);
            var orgianalCount = basket.Quantity;

                var total = _unitOfWork.Basket.GetAll(i => i.ApplicationUserId == basket.ApplicationUserId).ToList().Count;
                _unitOfWork.Basket.Remove(basket);
                productVariant.Quantity += basket.Quantity;
                _unitOfWork.Save();

                // Remove basket once all basket items are removed
                HttpContext.Session.SetInt32(SD.SessionBasket, total - 1);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            BasketVM = new BasketVM()
            {
                OrderHeader = new Models.OrderHeader(),
                BasketList = _unitOfWork.Basket.GetAll(i => i.ApplicationUserId == claim.Value,
                includeProperties: "ProductVariant,ProductVariant.Product,ProductVariant.Product.Brand")
            };

            // Retrieve userID of logged in user and assign it to orderheader
            BasketVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser
                .GetFirstOrDefault(i => i.Id == claim.Value);

            foreach (var items in BasketVM.BasketList)
            {
                items.Price = items.ProductVariant.Price;
                BasketVM.OrderHeader.OrderTotal += (items.Price * items.Quantity);
            }

            // Calculate VAT into order total
            var vat = SD.CalculateVat(BasketVM.OrderHeader.OrderTotal);

            BasketVM.OrderHeader.OrderTotal += vat;

            // Populate order header with user details
            BasketVM.OrderHeader.Name = BasketVM.OrderHeader.ApplicationUser.Name;
            BasketVM.OrderHeader.StreetAddress = BasketVM.OrderHeader.ApplicationUser.StreetAddress;
            BasketVM.OrderHeader.PhoneNumber = BasketVM.OrderHeader.ApplicationUser.PhoneNumber;
            BasketVM.OrderHeader.City = BasketVM.OrderHeader.ApplicationUser.City;
            BasketVM.OrderHeader.County = BasketVM.OrderHeader.ApplicationUser.County;
            BasketVM.OrderHeader.PostalCode = BasketVM.OrderHeader.ApplicationUser.Postcode;

            return View(BasketVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPost(string stripeToken)
        {
            // Identitfy user associated with basket
            var claimsIdentity = (ClaimsIdentity)User.Identity;

            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            BasketVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(i => i.Id == claim.Value);

            // Retrieve product details including product name and brand name for order information
            BasketVM.BasketList = _unitOfWork.Basket.GetAll(i => i.ApplicationUserId == claim.Value,
                                                            includeProperties: "ProductVariant,ProductVariant.Product,ProductVariant.Product.Brand");

            BasketVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            BasketVM.OrderHeader.OrderStatus = SD.StatusPending;
            // Assign user to order
            BasketVM.OrderHeader.ApplicationUserId = claim.Value;
            BasketVM.OrderHeader.OrderDate = DateTime.Now;

            _unitOfWork.OrderHeader.Add(BasketVM.OrderHeader);
            _unitOfWork.Save();

            foreach (var item in BasketVM.BasketList)
            {
                item.Price = item.ProductVariant.Price;
                OrderDetails orderDetails = new OrderDetails()
                {
                    ProductVariantId = item.ProductVariantId,
                    OrderId = BasketVM.OrderHeader.Id,
                    Price = item.Price,
                    Quantity = item.Quantity
                };
                BasketVM.OrderHeader.OrderTotal += orderDetails.Quantity * orderDetails.Price;
                _unitOfWork.OrderDetails.Add(orderDetails);
            }
            // Remove all items from basket
            _unitOfWork.Basket.RemoveRange(BasketVM.BasketList);
            _unitOfWork.Save();
            // Remove basket from session
            HttpContext.Session.SetInt32(SD.SessionBasket, 0);

            if(stripeToken == null)
            {

            }
            else
            {
                // Process Payment
                var options = new ChargeCreateOptions
                {
                    Amount = Convert.ToInt32(BasketVM.OrderHeader.OrderTotal * 100),
                    Currency = "gbp",
                    Description = "Order ID : " + BasketVM.OrderHeader.Id,
                    Source = stripeToken
                };

                var service = new ChargeService();
                Charge charge = service.Create(options);

                if(charge.Id == null)
                {
                    BasketVM.OrderHeader.PaymentStatus = SD.PaymentStatusRejected;
                }
                else
                {
                    BasketVM.OrderHeader.TransactionId = charge.Id;
                }
                // Payment Approval
                if(charge.Status.ToLower() == "succeeded")
                {
                    BasketVM.OrderHeader.PaymentStatus = SD.PaymentStatusApproved;
                    BasketVM.OrderHeader.OrderStatus = SD.StatusApproved;
                    BasketVM.OrderHeader.PaymentDate = DateTime.Now;
                }


                _unitOfWork.Save();
            }

            return RedirectToAction("OrderConfirmation", "Basket", new { id = BasketVM.OrderHeader.Id });
        }

        public IActionResult OrderConfirmation(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(u => u.Id == id);

            // Phone notification for order
            TwilioClient.Init(_twilioOptions.AccountSid, _twilioOptions.AuthToken);

            try
            {
                var message = MessageResource.Create(
                    body: "Order placed on S&K's Shoe and Sports. Your Order ID: " + id,
                    from: new Twilio.Types.PhoneNumber(_twilioOptions.PhoneNumber),
                    to: new Twilio.Types.PhoneNumber(orderHeader.PhoneNumber)
                    );
            }
            catch (Exception ex)
            {

            }

            return View(id);
            }
        }
    }
    
    