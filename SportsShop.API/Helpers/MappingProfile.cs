using AutoMapper;
using SportsShop.Core.Dtos.Products;
using SportsShop.Core.Entities;
using SportsShop.Service.CQRS.Products.Commands;

namespace SportsShop.API.Helpers
{
    public class MappingProfiles:Profile
    {
        public MappingProfiles()
        {
            #region Product
            CreateMap<Product, ProductDto>().ReverseMap();

            CreateMap<Product, ProductCreateDto>().ReverseMap(); 
            #endregion
        }
        

    }
}
