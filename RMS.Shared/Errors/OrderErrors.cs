using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.Errors
{
   

    public static class OrderErrors
    {
        public const string OrderNotFound = "Order not found.";
        public const string AmountMustBePositive = "Total Amount must be greater than 0.";
        public const string OrderAlreadyCompleted = "Order already completed.";
        public const string OrderIdRequired = "Order ID is required.";
        public const string InvalidOrderIdFormat = "Order ID format is invalid.";
        public const string OrderFlagged = "Order is flagged.";
        public const string OrderAlreadyApproved = "Order is already approved.";
        public const string WaitingWorkerAcceptance = "Order is awaiting worker acceptance.";
        public const string UnauthorizedAccess = "Unauthorized access to the order.";
        public const string InvalidOrderStatus = "Invalid order status.";
        public const string OrderCanceled = "Order has been canceled.";
        public const string PaymentFailed = "Payment has failed.";
        public const string PendingValidation = "Order is pending validation.";
        public const string AmountExceedsLimit = "Total Amount exceeds the allowed limit (10000).";
        public const string ScheduledDateInPast = "Scheduled date must be in the future.";
        public const string WaitOtpCodeRequired = "Wait OTP code is required for this order.";
        public const string InvalidOtpCode = "Invalid OTP code provided.";
        public const string OtpCodeRequired = "OTP code is required for this operation.";
        public const string OrderNotCompleted = "Order is not completed yet.";
        public const string OrderPaymentAlreadyProcessed = "Order payment has already been processed.";
    }
}
