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
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts([FromQuery] string? priceOrder, string? size, string? colour, string? genre, string? category, string? dateOrder)
        {
            try
            {
                return Ok(await _productService.GetProducts(priceOrder, size, genre, category, colour, dateOrder));

            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("allproducts")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            try
            {
                return Ok(await _productService.GetAllProducts());
            }
            catch (ArgumentException ex)
            {

                return BadRequest(ex.Message);
            }

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
        public IActionResult EditProductById(int id, ProductToEditDto producttoeditdto)
        {
            if (_productService.EditProductById(id, producttoeditdto))
            {
                return Ok($"se ha editado el producto con el id: {id}");
            }
            return BadRequest($"no se ha podido editar el producto con el id: {id}");
        }

        #region GetVariants
        [HttpGet("colours")]
        public IActionResult GetColours()
        {
            return Ok(_productService.GetActiveEntities<Colour>());
        }

        [HttpGet("sizes")]
        public IActionResult GetSizes()
        {
            return Ok(_productService.GetActiveEntities<Size>());
        }

        [HttpGet("categories")]
        public IActionResult GetCategories()
        {
            return Ok(_productService.GetActiveEntities<Category>());
        }

        [HttpGet("mappedColours")]
        public IActionResult GetColoursMapped()
        {
            return Ok(_productService.GetMappedVariants<Colour, ReturnColourDto>());
        }
        [HttpGet("mappedSizes")]
        public IActionResult GetSizesMapped()
        {
            return Ok(_productService.GetMappedVariants<Size, ReturnSizeDto>());
        }
        [HttpGet("mappedCategories")]
        public IActionResult GetCategoriesMapped()
        {
            return Ok(_productService.GetMappedVariants<Category, ReturnCategoryDto>());
        }

        #endregion

        #region PutVariants
        [HttpPut("products/{id}")]
        public IActionResult ChangeProductStatus(int id)
        {
            _productService.ChangeEntityStatus<Product>(id);
            return Ok($"Se cambió el estado del producto");
        }

        [HttpPut("colours/{id}")]
        public IActionResult ChangeColourStatus(int id)
        {
            _productService.ChangeEntityStatus<Colour>(id);
            return Ok($"Se cambió el estado del color con ID {id}");
        }

        [HttpPut("sizes/{id}")]
        public IActionResult ChangeSizeStatus(int id)
        {
            _productService.ChangeEntityStatus<Size>(id);
            return Ok($"Se cambió el estado del tamaño con ID {id}");
        }

        [HttpPut("categories/{id}")]
        public IActionResult ChangeCategoryStatus(int id)
        {
            _productService.ChangeEntityStatus<Category>(id);
            return Ok($"Se cambió el estado de la categoría con ID {id}");
        }
        #endregion

    }
}
