using SportsShop.Core.Entities;
using SportsShop.Core.Repositories.Contract;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SportsShop.Repository
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {

        private readonly IDatabase _database;

        public ShoppingCartRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task<bool> DeleteCartAsync(string cartId)
        {
            return await _database.KeyDeleteAsync(cartId);
        }

        public async Task<ShoppingCart?> GetCartAsync(string key)
        {
            var cart = await _database.StringGetAsync(key);
            return cart.IsNullOrEmpty ? null : JsonSerializer.Deserialize<ShoppingCart>(cart!); 
        }

        public async Task<ShoppingCart?> SetCartAsync(ShoppingCart cart)
        {
            var createdOrUpdated = await _database.StringSetAsync(cart.Id, JsonSerializer.Serialize(cart), TimeSpan.FromDays(30));
            if (createdOrUpdated is false) return null;
            return await GetCartAsync(cart.Id);
        }
    }
}
