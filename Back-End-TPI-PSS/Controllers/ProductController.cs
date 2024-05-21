using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Data.Models.ProductDTOs;
using Back_End_TPI_PSS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Back_End_TPI_PSS.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService service)
        {
            _productService = service;
        }

        [HttpGet("products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] string? priceOrder, string? genre)
        {
            return Ok(await _productService.GetProducts(priceOrder, genre));
        }
        [HttpGet("allproducts")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return Ok( await _productService.GetAllProducts());
        }

        [HttpPost("products")]
        public IActionResult AddProduct(ProductDto productDto)
        {
            try
            {
                if (_productService.AddProduct(productDto))
                {
                    return Ok("Producto agregado");
                }
                return BadRequest("Error al agregar el producto.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("colours")]
        public IActionResult AddColour([FromBody] ColourDto colourDto)
        {
            if (_productService.AddColour(colourDto))
            {
                return Ok("Color agregado");
            }
            return BadRequest("Error al agregar el Color.");
        }

        [HttpPost("sizes")]
        public IActionResult AddSize([FromBody] SizeDto sizeDto)
        {
            if (_productService.AddSize(sizeDto))
            {
                return Ok("Talle agregado");
            }
            return BadRequest("Error al agregar el Talle.");
        }

        [HttpPost("categories")]
        public IActionResult AddCategories([FromBody] CategoryDto categoryDto)
        {
            if (_productService.AddCategory(categoryDto))
            {
                return Ok("Categoría agregado");
            }
            return BadRequest("Error al agregar la Categoría.");
        }

        [HttpPut("products/edit/{id}")]
        public IActionResult EditProductById(int id, ProductToEditDto productToEditDto)
        {
            if (_productService.EditProductById(id, productToEditDto))
            {
                return Ok($"Se ha editado el producto con el ID: {id}");
            }
            return BadRequest($"No se ha podido editar el producto con el ID: {id}");
        }

        [HttpGet("colours")]
        public IActionResult GetColours()
        {
            var coloursToReturn = _productService.GetColours();
            return Ok(coloursToReturn);
        }

        [HttpGet("sizes")]
        public IActionResult GetSizes()
        {
            var sizesToReturn = _productService.GetSizes();
            return Ok(sizesToReturn);
        }

        [HttpGet("categories")]
        public IActionResult GetCategories()
        {
            var categoriesToReturn = _productService.GetCategories();
            return Ok(categoriesToReturn);
        }

        [HttpPut("products/{id}")]
        public IActionResult ChangeProductStatus(int id)
        {
            _productService.ChangeProductStatus(id);
            return Ok($"Se cambió el estado del producto");
        }

        [HttpPut("colours/{id}")]
        public IActionResult ChangeColourStatus(int id)
        {
            _productService.ChangeColourStatus(id);
            return Ok($"Se cambió el estado del color con ID {id}");
        }

        [HttpPut("sizes/{id}")]
        public IActionResult ChangeSizeStatus(int id)
        {
            _productService.ChangeSizeStatus(id);
            return Ok($"Se cambió el estado del tamaño con ID {id}");
        }

        [HttpPut("categories/{id}")]
        public IActionResult ChangeCategoryStatus(int id)
        {
            _productService.ChangeCategoryStatus(id);
            return Ok($"Se cambió el estado de la categoría con ID {id}");
        }
    }
}
