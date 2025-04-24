using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SportsShop.API.ControllerParameter;

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

        //[HttpPost]
        //public 



    }
}
