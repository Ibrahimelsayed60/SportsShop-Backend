using Microsoft.AspNetCore.Http;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsShop.Core.Services.Contract
{
    public interface IPaymentHandle
    {
        public Event ConstructStripeEvent(HttpRequest Request, string json, string _whSecret);
        public Task HandlePaymentIntentSucceeded(PaymentIntent intent);
    }
}
