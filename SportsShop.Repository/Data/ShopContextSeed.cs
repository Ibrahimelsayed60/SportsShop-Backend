using SportsShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SportsShop.Repository.Data
{
    public class ShopContextSeed
    {

        public static async Task SeedAsync(ShopContext shopContext)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (!shopContext.Products.Any())
            {
                var productsData = await File.ReadAllTextAsync("../SportsShop.Repository/Data/SeedData/products.json");

                var products = JsonSerializer.Deserialize<List<Product>>(productsData);

                if (products == null) return;

                shopContext.Products.AddRange(products);

                await shopContext.SaveChangesAsync();
            }

            if (!shopContext.DeliveryMethods.Any())
            {
                var deliveryData = await File.ReadAllTextAsync("../SportsShop.Repository/Data/SeedData/delivery.json");

                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryData);

                if (deliveryMethods == null) return;

                shopContext.DeliveryMethods.AddRange(deliveryMethods);

                await shopContext.SaveChangesAsync();
            }

        }

    }
}
