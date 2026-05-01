using AutoMapper;
using Microsoft.Extensions.Configuration;
using RMS.Domain.Contracts;
using RMS.Domain.Entities;
using RMS.Domain.Enums;
using RMS.Services.Exceptions;
using RMS.Services.Services.PaymobServices;
using RMS.Services.Specifications.PaymentSpec;
using RMS.ServicesAbstraction.IServices.IEmailServices;
using RMS.ServicesAbstraction.IServices.IPaymentServices;
using RMS.ServicesAbstraction.IServices.IPaymobServices;
using RMS.Shared;
using RMS.Shared.DTOs.PaymentsDTOS;
using RMS.Shared.QueryParams;

public class PaymentService : IPaymentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPaymobService _paymob;
    private readonly IEmailService _email;
    private readonly string _secret;
    private readonly IMapper _mapper;

    public PaymentService(
        IUnitOfWork uow,
        IPaymobService paymob,
        IEmailService email,
        IConfiguration config,
        IMapper mapper)
    {
        _unitOfWork = uow;
        _paymob = paymob;
        _email = email;
        _secret = config["Paymob:SecretKey"]!;
        _mapper = mapper;
    }

   


    public async Task<string> PayOrderAsync(int orderId, string userId)
    {
        var orderRepo = _unitOfWork.GetRepository<Order>();
        var paymentRepo = _unitOfWork.GetRepository<Payment>();

        var order = await orderRepo.GetByIdAsync(orderId);

        if (order == null)
            throw new OrderNotFoundException(orderId);

        if (order.UserId != userId)
            throw new UnauthorizedOrderException();

        var existingPayment = await paymentRepo.GetByIdAsync(
            new PaymentByOrderIdSpecification(orderId)
        );

        if (existingPayment != null)
        {
            if (existingPayment.PaymentStatus == PaymentStatus.Paid)
                throw new OrderAlreadyPaidException(orderId);

            var payment = new Payment
            {
                OrderId = order.Id,
                PaymentMethod = PaymentMethod.Card,
                PaymentStatus = PaymentStatus.Paid,
                PaidAmount = order.TotalAmount
            };

            await paymentRepo.AddAsync(payment);

            order.Status = OrderStatus.Received;



            await _unitOfWork.SaveChangesAsync();


            var token = await _paymob.GetPaymentKeyAsync(order.TotalAmount, order.Id);
            return _paymob.BuildIframeUrl(token);
        }

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


    public async Task ConfirmCashPaymentAsync(int orderId , decimal paidAmount)
    {
        var orderRepo = _unitOfWork.GetRepository<Order>();
        var paymentRepo = _unitOfWork.GetRepository<Payment>();

        var order = await orderRepo.GetByIdAsync(orderId);
        if (order == null)
            throw new OrderNotFoundException(orderId);

        var payment = await paymentRepo.GetByIdAsync(
            new PaymentByOrderIdSpecification(orderId)
        );

        if (payment == null)
            throw new PaymentNotFoundException(orderId);

        if (payment.PaymentStatus == PaymentStatus.Paid)
            throw new OrderAlreadyPaidException(orderId);

        payment.PaidAmount = paidAmount;
        payment.PaymentMethod = PaymentMethod.Cash;

        payment.PaymentStatus = PaymentStatus.Paid;
        payment.PaidAt = DateTime.Now;

        await _unitOfWork.SaveChangesAsync();
    }






    public async Task<PaginatedResult<PaymentDto>> GetAllAsync(PaymentQueryParams queryParams)
    {
        var repo = _unitOfWork.GetRepository<Payment>();

        var spec = new PaymentSpecifications(queryParams);
        var countSpec = new PaymentCountSpecifications(queryParams);

        var payments = await repo.GetAllAsync(spec);

        var count = await repo.CountAsync(countSpec);

        var data = _mapper.Map<List<PaymentDto>>(payments);

        return new PaginatedResult<PaymentDto>(queryParams.PageIndex,queryParams.PageSize,count,data);
      
    }

    public async Task<List<PaymentDto>> GetAllWithoutPaginationAsync()
    {
        var today = DateTime.Today;
        var tomorrow = today.AddDays(1);


        var repo = _unitOfWork.GetRepository<Payment>();

        var payments = await repo.GetAllAsync();

        return _mapper.Map<List<PaymentDto>>(payments);
    }
}





















