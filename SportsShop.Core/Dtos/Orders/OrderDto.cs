﻿using SportsShop.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsShop.Core.Dtos.Orders
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public required string BuyerEmail { get; set; }
        public required ShippingAddress ShippingAddress { get; set; }
        public required string DeliveryMethod { get; set; }
        public decimal ShippingPrice { get; set; }
        public required PaymentSummary PaymentSummary { get; set; }
        public required List<OrderItemDto> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public required string Status { get; set; }
        public decimal Total { get; set; }
        public required string PaymentIntentId { get; set; }

    }
}
