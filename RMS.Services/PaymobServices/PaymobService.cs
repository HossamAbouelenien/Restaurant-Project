using Ardalis.GuardClauses;
using ErrorOr;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Domain.Enums;
using RMS.Services.Specifications.OrderSpec;
using RMS.ServicesAbstraction;
using RMS.ServicesAbstraction.IEmailServices;
using RMS.Shared.DTOs.PaymentDTOs;
using RMS.Shared.Enums;
using RMS.Shared.Errors;
using RMS.Shared.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace RMS.Services.Paymob
{
    // PaymentService.cs
    

    public class PaymobService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymobService _paymobService;
        private readonly IEmailService _emailService;
        //private readonly ILogger<PaymentService> _logger;
        private readonly string _iFrameUrl;
        private readonly int _integrationId;

        public PaymobService(
            IUnitOfWork unitOfWork,
            IPaymobService paymobService,
            IEmailService emailService,
            //ILogger<PaymentService> logger,
            IOptions<PaymobSettings> options)
        {
            var settings = Guard.Against.Null(options).Value;
            _integrationId = settings.IntegrationId;
            Guard.Against.NegativeOrZero(_integrationId);
            _iFrameUrl = Guard.Against.NullOrEmpty(settings.EndPoints!.IFrameUrl);
            _unitOfWork = Guard.Against.Null(unitOfWork);
            _paymobService = Guard.Against.Null(paymobService);
            _emailService = Guard.Against.Null(emailService);
            //_logger = Guard.Against.Null(logger);
        }

        //public async Task<ErrorOr<PayOrderWithCardResponse>> PayOrderWithCardAsync(
        //    string customerId, int orderId, CancellationToken cancellationToken = default)
        //{
        //    // Validation
        //    if (string.IsNullOrWhiteSpace(customerId) || !AccountErrors.IsValidId(customerId))
        //        return Error.Validation(description: AccountErrors.UserIdInvalidFormat);

        //    if (orderId <= 0)
        //        return Error.Validation(description: OrderErrors.InvalidOrderIdFormat);

        //    var spec = new OrderDetailsSpecification(
        //         orderId,
        //     OrderIncludes.User | OrderIncludes.Payment
        //);


        //    var order = await _unitOfWork.GetRepository<Order>()
        //        .GetByIdAsync(spec);

            //var validationResult = OrderHelpers.ValidateOrder(
            //    order,
            //    _logger,
            //    expectedCustomerId: customerId,
            //    options: OrderValidationOptions.ValidateCustomerAccess |
            //             OrderValidationOptions.RequireNotFlagged |
            //             OrderValidationOptions.RequireNotCompleted |
            //             OrderValidationOptions.RequireNotAwaitingWorkerAcceptance |
            //             OrderValidationOptions.RequireNotCanceled);

            //if (validationResult.IsError)
            //    return validationResult.Errors;

        //    var payment = new Payment
        //    {
        //        OrderId = order.Id,
        //        PaymentMethod = PaymentMethod.Card,
        //        PaymentStatus = PaymentStatus.Pending,
        //        PaidAmount = order.TotalAmount
        //    };

        //    order.Payment.PaymentMethod = PaymentMethod.Card;
        //    order.Payment.PaymentStatus = PaymentStatus.Pending;
        //    order.Payment = payment;
        //    order.Status = OrderStatus.AwaitingPayment;
        //    order.UpdatedAt = DateTime.UtcNow;

        //    await _unitOfWork.SaveChangesAsync();

        //    var token = await _paymobService.GetPaymentKeyAsync(
        //        GetPaymobOrderRequest(order),
        //        GetPaymobPaymentRequest(order),
        //        order.Id);

        //    _logger.LogInformation("Payment initiated for order {OrderId}", order.Id);

        //    return new PayOrderWithCardResponse(
        //        $"{_iFrameUrl}?payment_token={token}",
        //        "Payment initiated successfully, please complete the payment");
        //}

        //public async Task<ErrorOr<PayOrderWithCardResponse>> PayPlatformFeeAsync(
        //    string workerId, CancellationToken cancellationToken = default)
        //{
        //    if (string.IsNullOrWhiteSpace(workerId) || !AccountErrors.IsValidId(workerId))
        //        return Error.Validation(description: AccountErrors.UserIdInvalidFormat);

        //    var worker = await _unitOfWork.Repository<Worker>()
        //        .FindAsync(w => w.Id.Equals(workerId), cancellationToken);

        //    if (worker is null)
        //        return Error.NotFound(description: "Worker not found");

        //    if (worker.PlatformFeeBalance <= 0)
        //        return Error.Validation(description: "No platform fee to pay");

        //    var newWallet = worker.WalletBalance - worker.PlatformFeeBalance;

        //    if (newWallet >= 0)
        //    {
        //        await _unitOfWork.Repository<Worker>()
        //            .ExecuteUpdateAsync(
        //                w => w.Id.Equals(workerId),
        //                setter => setter
        //                    .SetProperty(w => w.WalletBalance, newWallet)
        //                    .SetProperty(w => w.PlatformFeeBalance, 0),
        //                cancellationToken);

        //        EnqueuePlatformFeePaidEmail(worker);

        //        return new PayOrderWithCardResponse(
        //            string.Empty,
        //            "Platform fee paid successfully, no payment required.");
        //    }

        //    await _unitOfWork.Repository<Worker>()
        //        .ExecuteUpdateAsync(
        //            w => w.Id.Equals(workerId),
        //            setter => setter
        //                .SetProperty(w => w.WalletBalance, 0)
        //                .SetProperty(w => w.PlatformFeeBalance, -newWallet),
        //            cancellationToken);

        //    worker.WalletBalance = 0;
        //    worker.PlatformFeeBalance = -newWallet;

        //    var token = await _paymobServices.GetPaymentKeyAsync(
        //        GetPlatformFeeOrderRequest(worker),
        //        GetPlatformFeePaymentRequest(worker),
        //        worker.Id,
        //        bill: BillType.WorkerFee);

        //    _logger.LogInformation("Generated platform fee iframe for worker {WorkerId}", workerId);

        //    return new PayOrderWithCardResponse(
        //        $"{_iFrameUrl}?payment_token={token}",
        //        "Payment initiated successfully, please complete the payment");
        //}

        // ── Private helpers ──────────────────────────────────────────────

        //private static PaymobCreateOrderRequest GetPaymobOrderRequest(Order order) =>
        //    new()
        //    {
        //        DeliveryNeeded = false,
        //        AmountCents = (int)(order.TotalAmount * 100),
        //        MerchantOrderId = order.Id,
        //        Items =
        //        [
        //            new PaymobItem
        //        {
        //            Name = $"Service Payment for Order: {order.Id}",
        //            Description = "Online payment for service",
        //            AmountCents = (int)(order.TotalAmount * 100)
        //        }
        //        ],
        //        ShippingData = new PaymobShippingData(order)
        //    };

        //private PaymobPaymentRequest GetPaymobPaymentRequest(Order order) =>
        //    new()
        //    {
        //        AmountCents = (int)(order.TotalAmount * 100),
        //        Expiration = 3600,
        //        BillingData = new PaymobBillingData
        //        {
        //            Apartment = order.Address!.FlatNumber.ToString(),
        //            Floor = order.Address!.FloorNumber.ToString(),
        //            Street = order.Address!.AddressLine + " " + order.Address!.AddressNote,
        //            Building = order.Address!.HomeNumber.ToString(),
        //            City = order.Address!.City,
        //            State = order.Address!.City,
        //            Email = order.Customer!.Email!,
        //            FirstName = order.Customer!.FullName!.Split(' ').FirstOrDefault() ?? string.Empty,
        //            LastName = order.Customer!.FullName!.Split(' ').LastOrDefault() ?? string.Empty,
        //            PhoneNumber = order.Customer!.PhoneNumber ?? string.Empty,
        //            PostalCode = "12345",
        //            Country = "EG",
        //            ShippingMethod = "PKG"
        //        },
        //        Currency = "EGP",
        //        IntegrationId = _integrationId
        //    };

        //private static PaymobCreateOrderRequest GetPlatformFeeOrderRequest(Worker worker) =>
        //    new()
        //    {
        //        DeliveryNeeded = false,
        //        AmountCents = (int)(worker.PlatformFeeBalance * 100),
        //        MerchantOrderId = worker.Id,
        //        Items =
        //        [
        //            new PaymobItem
        //        {
        //            Name = $"Platform Fee for Worker: {worker.Id}",
        //            Description = "Online payment for service",
        //            AmountCents = (int)(worker.PlatformFeeBalance * 100)
        //        }
        //        ],
        //        ShippingData = new PaymobShippingData(worker)
        //    };

        //private PaymobPaymentRequest GetPlatformFeePaymentRequest(Worker worker) =>
        //    new()
        //    {
        //        AmountCents = (int)(worker.PlatformFeeBalance * 100),
        //        Expiration = 3600,
        //        BillingData = new PaymobBillingData(worker),
        //        Currency = "EGP",
        //        IntegrationId = _integrationId
        //    };

        //private void EnqueuePlatformFeePaidEmail(Worker worker)
        //{
        //    var email = new Email(
        //        To: [worker.Email!],
        //        Title: "Platform Fee Paid",
        //        Subject: "Your platform fee has been fully paid",
        //        Body: $"Hello {worker.FullName},\n\nYour outstanding platform fee balance is now zero.\n\nThank you!\nThe Swipe Team",
        //        TemplateType: EmailTemplateType.Default);

        //    BackgroundJob.Enqueue(() => _emailService.SendEmailAsync(email, null, null));
        //       }
        //public async Task<string> GetPaymentKeyAsync(
        // PaymobCreateOrderRequest paymobCreateOrderRequest,
        // PaymobPaymentRequest paymobPaymentRequest,
        // string cleaningOrderId,
        // BillType bill = BillType.Order)
        //{
        //    var authToken = await GetAuthTokenAsync();
        //    if (string.IsNullOrEmpty(authToken))
        //    {
        //        _logger.LogError("Authentication token is null or empty.");
        //        return string.Empty;
        //    }

        //    var orderRequest = paymobCreateOrderRequest;
        //    orderRequest.AuthToken = authToken;
        //    orderRequest.MerchantOrderId =
        //        $"{cleaningOrderId}_{bill.ToString()}_{DateTime.UtcNow:yyyyMMddHHmmssfff}"; //$"ORD-{Guid.NewGuid()}-orderId-{cleaningOrderId}";

        //    var paymobOrderId = await CreateOrderAsync(orderRequest);
        //    if (!int.TryParse(paymobOrderId, out var orderIdInt))
        //    {
        //        _logger.LogError("Invalid order id from Paymob: {Raw}", paymobOrderId);
        //        return string.Empty;
        //    }
        //    //var paymobOrderId = await CreateOrderAsync(orderRequest);

        //    var paymentRequest = paymobPaymentRequest;
        //    paymentRequest.AuthToken = authToken;
        //    paymentRequest.OrderId = orderIdInt;

        //    _httpClient.DefaultRequestHeaders.Authorization =
        //        new AuthenticationHeaderValue("Bearer", authToken);

        //    var content = new StringContent(JsonSerializer.Serialize(paymentRequest), Encoding.UTF8,
        //        "application/json");

        //    var response = await _httpClient.PostAsync(_paymobSettings.EndPoints!.GetPaymentKeyEndPoint, content);
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        var errorMessage = await response.Content.ReadAsStringAsync();
        //        _logger.LogError("Failed to get payment key: {StatusCode} - {ErrorMessage}", response.StatusCode,
        //            errorMessage);
        //        return string.Empty;
        //    }

            //response.EnsureSuccessStatusCode();

            //var responseJson = await response.Content.ReadAsStringAsync();
            //using var doc = JsonDocument.Parse(responseJson);

            //return doc.RootElement.GetProperty("token")
            //    .GetString() ?? string.Empty;
        //}
    }

}
