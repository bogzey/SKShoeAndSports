using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SKShoeAndSports.DataAccess.Repository.IRepository;
using SKShoeAndSports.Models;
using SKShoeAndSports.Models.ViewModels;
using SKShoeAndSports.Utility;
using Stripe;

namespace SKShoeAndSports.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public OrderDetailsVM OrderVM { get; set; }

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        [NoDirectAccess] // Prevents accessing through manipulation of url
        public IActionResult Details(int id)
        {
            OrderVM = new OrderDetailsVM()
            {
                OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(i => i.Id == id,
                includeProperties: "ApplicationUser"),
                OrderDetails = _unitOfWork.OrderDetails.GetAll(o => o.OrderId == id, includeProperties: "ProductVariant,ProductVariant.Product,ProductVariant.Product.Brand")

            };

            return View(OrderVM);
        }

        [Authorize(Roles = SD.Admin_Role + "," + SD.Staff_Role)]
        public IActionResult StartProcessing(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(i => i.Id == id);

            orderHeader.OrderStatus = SD.StatusInProcess;
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = SD.Admin_Role + "," + SD.Staff_Role)]
        public IActionResult ShipOrder()
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(i => i.Id == OrderVM.OrderHeader.Id);
            orderHeader.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            orderHeader.PostalCarrier = OrderVM.OrderHeader.PostalCarrier;
            orderHeader.OrderStatus = SD.StatusShipped;
            orderHeader.ShippingDate = DateTime.Now;
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = SD.Admin_Role + "," + SD.Staff_Role)]
        public IActionResult CancelOrder(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(i => i.Id == id);

            OrderVM = new OrderDetailsVM()
            {
                OrderDetails = _unitOfWork.OrderDetails.GetAll(i => i.OrderId == id),
                
            };

            

            /*foreach (var product in OrderVM.OrderDetails)
            {
                OrderVM.ProductVariants = _unitOfWork.ProductVariant.GetAll(i => i.Id == product.ProductVariantId);

                product.ProductVariant.Quantity += product.Quantity;
            }*/
            

            


            if (orderHeader.PaymentStatus == SD.StatusApproved)
            {
                var options = new RefundCreateOptions
                {
                    Amount = Convert.ToInt32(orderHeader.OrderTotal * 100),
                    Reason = RefundReasons.RequestedByCustomer,
                    Charge = orderHeader.TransactionId
                };

                var service = new RefundService();
                Refund refund = service.Create(options);

                orderHeader.OrderStatus = SD.StatusRefunded;

                orderHeader.PaymentStatus = SD.StatusRefunded;

                foreach (var product in OrderVM.OrderDetails)
                {
                    // Update product variant quanitity after return
                    OrderVM.ProductVariants = _unitOfWork.ProductVariant.GetAll(i => i.Id == product.ProductVariantId);
                    product.ProductVariant.Quantity += product.Quantity;
                }

            }
            else
            {
                orderHeader.OrderStatus = SD.StatusCancelled;
                orderHeader.PaymentStatus = SD.StatusCancelled;

                foreach (var product in OrderVM.OrderDetails)
                {
                    // Update product variant quanitity after return
                    OrderVM.ProductVariants = _unitOfWork.ProductVariant.GetAll(i => i.Id == product.ProductVariantId);
                    product.ProductVariant.Quantity += product.Quantity;
                }

            }

            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetOrderList(string status)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            IEnumerable<OrderHeader> orderHeaderList;

            if (User.IsInRole(SD.Admin_Role) || User.IsInRole(SD.Staff_Role))
            {
                // All customers orders
                orderHeaderList = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");
            }
            else
            {
                // Orders related to customer
                orderHeaderList = _unitOfWork.OrderHeader.GetAll(i => i.ApplicationUserId == claim.Value,
                                                                    includeProperties: "ApplicationUser");
            }

            // Filter Orders

            switch (status)
            {
                case "inprocess":
                    orderHeaderList = orderHeaderList.Where(i => i.PaymentStatus == SD.StatusApproved ||
                                                                i.OrderStatus == SD.StatusInProcess || 
                                                                i.OrderStatus == SD.StatusPending);
                    break;
                case "completed":
                    orderHeaderList = orderHeaderList.Where(i => i.OrderStatus == SD.StatusShipped);
                    break;
                case "rejected":
                    orderHeaderList = orderHeaderList.Where(i => i.PaymentStatus == SD.StatusCancelled ||
                                                                i.OrderStatus == SD.StatusRefunded ||
                                                                i.OrderStatus == SD.PaymentStatusRejected);
                    break;
                default:
                    break;

            }

            return Json( new {data = orderHeaderList});
        }

        #endregion
    }
}
