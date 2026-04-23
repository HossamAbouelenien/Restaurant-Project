using Microsoft.Extensions.Configuration;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Domain.Enums;
using RMS.Services.PaymobServices;
using RMS.Services.Specifications.PaymentSpec;
using RMS.ServicesAbstraction;
using RMS.ServicesAbstraction.IEmailServices;
using RMS.ServicesAbstraction.IPaymentServices;

public class PaymentService : IPaymentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymobService _paymob;
    private readonly IEmailService _email;
    private readonly string _secret;

    public PaymentService(
        IUnitOfWork uow,
        IPaymobService paymob,
        IEmailService email,
        IConfiguration config)
    {
        _unitOfWork = uow;
        _paymob = paymob;
        _email = email;
        _secret = config["Paymob:SecretKey"]!;
    }

   
    public async Task<string> PayOrderAsync(int orderId, string userId)
    {
        var orderRepo = _unitOfWork.GetRepository<Order>();
        var paymentRepo = _unitOfWork.GetRepository<Payment>();

        var order = await orderRepo.GetByIdAsync(orderId);

        if (order == null)
            throw new Exception("Order not found");

        if (order.UserId != userId)
            throw new Exception("Unauthorized");

        var existingPayment = await paymentRepo.GetByIdAsync(
            new PaymentByOrderIdSpecification(orderId)
        );

        if (existingPayment != null)
        {
            if (existingPayment.PaymentStatus == PaymentStatus.Paid)
                throw new Exception("Order already paid");

            var token = await _paymob.GetPaymentKeyAsync(order.TotalAmount, order.Id);
            return _paymob.BuildIframeUrl(token);
        }

      
        var payment = new Payment
        {
            OrderId = order.Id,
            PaymentMethod = PaymentMethod.Card,
            PaymentStatus = PaymentStatus.Pending,
            PaidAmount = order.TotalAmount
        };

        await paymentRepo.AddAsync(payment);

        order.Status = OrderStatus.AwaitingPayment;

        await _unitOfWork.SaveChangesAsync();

        var newToken = await _paymob.GetPaymentKeyAsync(order.TotalAmount, order.Id);

        return _paymob.BuildIframeUrl(newToken);
    }

   
    public async Task HandleWebhookAsync(PaymobWebhookDto dto)
    {
       
        var valid = PaymobHmacHelper.ValidateHmac(dto, _secret);

        if (!valid)
            return;

        var orderRepo = _unitOfWork.GetRepository<Order>();
        var paymentRepo = _unitOfWork.GetRepository<Payment>();

        if (string.IsNullOrWhiteSpace(dto.Obj.Order) ||
            !int.TryParse(dto.Obj.Order.Split('_')[0], out var orderId))
            return;

        var order = await orderRepo.GetByIdAsync(orderId);

        if (order == null)
            return;

     
        var payment = await paymentRepo.GetByIdAsync(
            new PaymentByOrderIdSpecification(orderId)
        );

        if (payment == null)
            return;

     
        if (payment.PaymentStatus == PaymentStatus.Paid)
            return;

        if (dto.Obj.Success)
        {
            payment.PaymentStatus = PaymentStatus.Paid;
            payment.PaidAt = DateTime.UtcNow;

            order.Status = OrderStatus.Received;

           
            if (!string.IsNullOrEmpty(order.User?.Email))
            {
                await _email.SendEmailAsync(
                    order.User.Email,
                    "Payment Success",
                    $"Order #{order.Id} paid successfully 💳");
            }
        }
        else
        {
            payment.PaymentStatus = PaymentStatus.Failed;
        }

        await _unitOfWork.SaveChangesAsync();
    }
}