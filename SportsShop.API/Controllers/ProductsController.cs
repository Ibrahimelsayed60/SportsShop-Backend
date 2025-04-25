using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsShop.API.ControllerParameter;
using SportsShop.Core.Dtos;
using SportsShop.Core.Entities;
using SportsShop.Core.Specifications.Products;
using SportsShop.Service.CQRS.Products.Queries;

namespace SportsShop.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductsController : BaseController
    {
        private readonly ControllerParameters _controllerParameters;

        public ProductsController(ControllerParameters controllerParameters ):base(controllerParameters) 
        {
            _controllerParameters = controllerParameters;
        }

        [HttpGet]
        public async Task<ResultDto> GetProducts([FromQuery] ProductSpecParams productSpecParams)
        {

            var resultDto = await _mediator.Send(new GetAllProductsQuery(productSpecParams));

            return resultDto;

        }

        [HttpGet("{id:int}")]
        public Task<ResultDto> GetProduct(int id)
        {
            var resultDto = _mediator.Send(new GetProductByIdQuery(id));

            return resultDto;
        }

        //[HttpPost]
        //public Task<Product> CreateProduct(Product product)
        //{

        //}

        //[HttpPut("{id:int}")]
        //public Task UpdateProduct(int id, Product product)
        //{

        //}

        //[HttpDelete("{id:int}")]
        //public Task DeleteProduct(int id)
        //{

        //}

        //private bool productExists(int id)
        //{

        //}



    }
}
