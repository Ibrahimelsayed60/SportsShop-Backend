using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsShop.API.ControllerParameter;
using SportsShop.Core.Dtos;
using SportsShop.Core.Entities;
using SportsShop.Core.Specifications.Products;
using SportsShop.Service.CQRS.Products.Commands;
using SportsShop.Service.CQRS.Products.Queries;

namespace SportsShop.API.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet("types")]
        public async Task<ResultDto> getTypes()
        {
            var data = await _mediator.Send(new GetTypesOfProductsQuery());
            return data;
        }

        [HttpGet("brands")]
        public Task<ResultDto> getBrands()
        {
            var data = _mediator.Send(new GetBrandsOfProductsQuery());

            return data;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductCreateDto productCreateDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(new AddProductCommand(productCreateDto));

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<ResultDto> UpdateProduct(int id, ProductCreateDto productCreateDto)
        {
            bool isExist = productExists(id);

            if (!isExist)
            {
                return ResultDto.Faliure("Product not found");
            }

            return await _mediator.Send(new UpdateProductCommand(id, productCreateDto));
        }

        [HttpDelete("{id:int}")]
        public async Task<ResultDto> DeleteProduct(int id)
        {
            bool isExist = productExists(id);

            var result = await _mediator.Send(new DeleteProductCommand(id));

            return result;
        }

        private  bool productExists(int id)
        {
            return  _mediator.Send(new CheckProductExistQuery(id)).Result;
        }



    }
}
