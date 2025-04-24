using Mapster;
using SportsShop.Core.Entities;
using SportsShop.Service.CQRS.Products.Commands;

namespace SportsShop.API.Helpers
{
    public static class MappingProfile
    {

        public static void RegisterMappings()
        {

            #region Product
            TypeAdapterConfig<ProductCreateDto, Product>.NewConfig(); 
            #endregion
        }

    }
}
