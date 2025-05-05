using SportsShop.Core.Entities.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsShop.Core.Dtos.Orders
{
    public class CreateOrderDto
    {
        [Required]
        public string CartId { get; set; } = string.Empty;

        [Required]
        public int DeliveryMethodId { get; set; }

        [Required]
        public ShippingAddress ShippingAddress { get; set; } = null!;

        [Required]
        public PaymentSummary PaymentSummary { get; set; } = null!;
    }
}
