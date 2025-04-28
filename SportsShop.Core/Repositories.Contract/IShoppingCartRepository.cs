using SportsShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsShop.Core.Repositories.Contract
{
    public interface IShoppingCartRepository
    {

        Task<ShoppingCart?> GetCartAsync(string key);

        Task<ShoppingCart?> SetCartAsync(ShoppingCart cart);

        Task<bool> DeleteCartAsync(string cartId);


    }
}
