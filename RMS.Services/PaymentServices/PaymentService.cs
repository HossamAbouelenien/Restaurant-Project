using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Domain.Enums;
using RMS.Services.PaymobServices;
using RMS.ServicesAbstraction;
using RMS.ServicesAbstraction.IEmailServices;
using RMS.ServicesAbstraction.IPaymentServices;

public class PaymentService : IPaymentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymobService _paymob;
    private readonly IEmailService _email;

    public PaymentService(IUnitOfWork uow, IPaymobService paymob, IEmailService email)
    {
        _unitOfWork = uow;
        _paymob = paymob;
        _email = email;
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

        if (order.Payment != null && order.Payment.PaymentStatus == PaymentStatus.Paid)
            throw new Exception("Order already paid");

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

        var token = await _paymob.GetPaymentKeyAsync(order.TotalAmount, order.Id);

        return _paymob.BuildIframeUrl(token);
    }

    public async Task HandleWebhookAsync(PaymobWebhookDto dto)
    {
        var valid = PaymobHmacHelper.ValidateHmac(dto, _paymobSecret());

        if (!valid)
            return;

        var orderRepo = _unitOfWork.GetRepository<Order>();

        if (!int.TryParse(dto.Obj.Order.Split('_')[0], out var orderId))
            return;

        var order = await orderRepo.GetByIdAsync(orderId);

        if (order == null || order.Payment == null)
            return;

        if (dto.Obj.Success)
        {
            order.Payment.PaymentStatus = PaymentStatus.Paid;
            order.Payment.PaidAt = DateTime.UtcNow;
            order.Status = OrderStatus.Received;

            await _email.SendEmailAsync(
                order.User!.Email!,
                "Payment Success",
                $"Order #{order.Id} paid successfully 💳");
        }
        else
        {
            order.Payment.PaymentStatus = PaymentStatus.Failed;
        }

        await _unitOfWork.SaveChangesAsync();
    }

    private string _paymobSecret()
    {
        // inject from config in real case
        return "YOUR_SECRET_KEY";
    }
}