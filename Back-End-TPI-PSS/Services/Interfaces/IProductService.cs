using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Data.Models.ProductDTOs;

namespace Back_End_TPI_PSS.Services.Interfaces
{
    public interface IProductService
    {
        public bool AddProduct(ProductDto product);
        public Task<IEnumerable<Product>> GetProducts(string? order, string? genre);
        public bool AddSize(SizeDto sizeDto);
        public bool AddColour(ColourDto colourDto);
        public bool AddCategory(CategoryDto categoryDto);
        public List<Colour> GetColours();
        public List<Size> GetSizes();
        public List<Category> GetCategories();
    }
}
