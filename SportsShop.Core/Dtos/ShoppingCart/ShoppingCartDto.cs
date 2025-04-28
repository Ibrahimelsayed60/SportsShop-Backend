using SportsShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsShop.Core.Dtos.ShoppingCart
{
    public class ShoppingCartDto
    {

        public required string Id { get; set; }

        public List<CartItem> Items { get; set; } = [];

    }
}
