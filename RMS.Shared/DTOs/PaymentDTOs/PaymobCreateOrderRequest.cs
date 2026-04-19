using RMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMS.Shared.DTOs.PaymentDTOs
{
    public class PaymobCreateOrderRequest
    {
        public bool DeliveryNeeded { get; set; }
        public int AmountCents { get; set; }
        public string? MerchantOrderId { get; set; }
        public List<PaymobItem> Items { get; set; } = [];
        public PaymobShippingData? ShippingData { get; set; }
    }
    public class PaymobItem
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int AmountCents { get; set; }
        public int Quantity { get; set; } = 1;
    }
    public class PaymobShippingData
    {
        public string? Name { get; set; }
      
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Note { get; set; }
        public string? spacialMark { get; set; }
        public string? Street { get; set; }
        public string? BuildingNumber { get; set; }
        public string? City { get; set; }
       
      

        public PaymobShippingData() { }

        /// <summary>Construct shipping data from an Order (customer + address).</summary>
        public PaymobShippingData(Order order)
        {
            Email = order.User?.Email;
            PhoneNumber = order.User?.PhoneNumber;
            Name = order.User?.Name?.Split(' ').FirstOrDefault() ?? string.Empty;
            BuildingNumber = order.User?.Addresses.Select(a=>a.BuildingNumber).FirstOrDefault().ToString();
           spacialMark= order.User?.Addresses.Select(a=>a.SpecialMark).FirstOrDefault()?.ToString();
            Street = (order.User?.Addresses.Select(a => a.Street).FirstOrDefault().ToString()) ;
            Note = order.User?.Addresses.Select(a=>a.Note).FirstOrDefault()?.ToString();
            City = order.User?.Addresses.Select(a=>a.City).FirstOrDefault();
            
        }

        /// <summary>Construct shipping data from a Worker.</summary>
        //public PaymobShippingData(Worker worker)
        //{
        //    Email = worker.Email;
        //    PhoneNumber = worker.PhoneNumber;
        //    FirstName = worker.FullName?.Split(' ').FirstOrDefault() ?? string.Empty;
        //    LastName = worker.FullName?.Split(' ').LastOrDefault() ?? string.Empty;
        //    City = "Cairo";
        //    State = "Cairo";
        //    Apartment = "N/A";
        //    Floor = "N/A";
        //    Street = "N/A";
        //    Building = "N/A";
        //}
    }
}
