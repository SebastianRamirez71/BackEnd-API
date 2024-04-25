using Back_End_TPI_PSS.Data.Models.ProductDTOs;
using Back_End_TPI_PSS.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Back_End_TPI_PSS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService service)
        {
            _productService = service;
        }

        [HttpGet("products")]
        public IActionResult GetProducts()
        {
            return Ok(_productService.GetProducts());
        }

        [HttpPost("products")]
        public IActionResult AddProduct(ProductDto productDto)
        {
            _productService.AddProduct(productDto);
            return Ok();
        }

        [HttpPost("sizes")]
        public IActionResult AddSize([FromBody] string size)
        {
            _productService.AddSize(size);
            return Ok();
        }
        [HttpPost("colours")]
        public IActionResult AddColour([FromBody] string colour)
        {
            _productService.AddColour(colour);
            return Ok();
        }
    }
}
