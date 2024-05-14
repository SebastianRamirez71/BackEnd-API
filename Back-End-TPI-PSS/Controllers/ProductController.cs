using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Data.Models.ProductDTOs;
using Back_End_TPI_PSS.Services.Interfaces;
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
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] string? order, string? genre)
        {
            var products = await _productService.GetProducts(order, genre);
            return Ok(products);
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

        [HttpGet("allProducts")]
        public IActionResult GetAllProducts()
        {
            var productsToReturn = _productService.GetAllProducts();
            return Ok(productsToReturn);
        }

        // Chequear xD
        [HttpPut("products/status/{id}")]
        public IActionResult ChangeProductStatus(int id)
        {
            _productService.ChangeProductStatus(id);
            return Ok($"Se cambió el estado del producto");
        }

        [HttpPut("products/{id}")]
        public IActionResult AddProduct(int id)
        {
            _productService.AddProduct(id);
            return Ok($"Se ha dado de alta al producto con el ID : {id}");
        }

        [HttpDelete("products/{id}")]
        public IActionResult DeleteProduct(int id)
        {
            _productService.DeleteProduct(id);
            return Ok($"Se ha dado de baja al producto con el ID : {id}");
        }

        [HttpPut("colours/{id}")]
        public IActionResult AddColor(int id)
        {
            _productService.AddColor(id);
            return Ok($"Se ha dado de alta al color con el ID : {id}");
        }

        [HttpDelete("colours/{id}")]
        public IActionResult DeleteColor(int id)
        {
            _productService.DeleteColor(id);
            return Ok($"Se ha dado de baja al color con el ID : {id}");
        }

        [HttpPut("sizes/{id}")]
        public IActionResult AddSize(int id)
        {
            _productService.AddSize(id);
            return Ok($"Se ha dado de alta al tamaño con el ID : {id}");
        }

        [HttpDelete("sizes/{id}")]
        public IActionResult DeleteSize(int id)
        {
            _productService.DeleteSize(id);
            return Ok($"Se ha dado de baja al tamaño con el ID : {id}");
        }

        [HttpPut("categories/{id}")]
        public IActionResult AddCategory(int id)
        {
            _productService.AddCategory(id);
            return Ok($"Se ha dado de alta a la categoría con el ID : {id}");
        }

        [HttpDelete("categories/{id}")]
        public IActionResult DeleteCategory(int id)
        {
            _productService.DeleteCategory(id);
            return Ok($"Se ha dado de baja a la categoría con el ID : {id}");
        }

    }
}
