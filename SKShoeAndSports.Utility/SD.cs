using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace SKShoeAndSports.Utility
{
    public static class SD
    {
        public const string Customer_Role = "Customer";
        public const string Admin_Role = "Admin";
        public const string Staff_Role = "Staff";

        public const string SessionBasket = "Basket Session";

        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusInProcess = "Processing";
        public const string StatusShipped = "Shipped";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";

        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusRejected = "Rejected";


        public static decimal ComputeDiscountedPrice(this decimal originalPrice, decimal percentDiscount)
        {
            // enforce preconditions
            if (originalPrice < 0m) throw new ArgumentOutOfRangeException("originalPrice", "a price can't be negative!");
            if (percentDiscount < 0m) throw new ArgumentOutOfRangeException("percentDiscount", "a discount can't be negative!");
            if (percentDiscount > 100m) throw new ArgumentOutOfRangeException("percentDiscount", "a discount can't exceed 100%");

            decimal markdown = Math.Round(originalPrice * (percentDiscount / 100m), 2, MidpointRounding.ToEven);
            decimal discountedPrice = originalPrice - markdown;

            return discountedPrice;
        }

        public static decimal CalculateVat(decimal value)
        {
            return (value / 100) * 20;
        }


    }
}
