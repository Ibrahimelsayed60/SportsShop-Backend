﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsShop.Core.Entities.Order
{
    public class Order : BaseEntity
    {
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public required string BuyerEmail { get; set; }
        public ShippingAddress ShippingAddress { get; set; } = null!;
        public DeliveryMethod DeliveryMethod { get; set; } = null!;
        public PaymentSummary PaymentSummary { get; set; } = null!;
        public List<OrderItem> OrderItems { get; set; } = [];
        public decimal Subtotal { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public required string PaymentIntentId { get; set; }

        public decimal GetTotal()
        {
            return Subtotal + DeliveryMethod.Price;
        }

    }
}
