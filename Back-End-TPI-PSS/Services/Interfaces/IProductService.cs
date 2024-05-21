using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Data.Models.ProductDTOs;

namespace Back_End_TPI_PSS.Services.Interfaces
{
    public interface IProductService
    {
        public bool AddProduct(ProductDto product);
        public Task<IEnumerable<Product>> GetProducts(string? priceOrder, string? size, string? genre, string? category, string? colour);
        public Task<IEnumerable<Product>> GetAllProducts();
        public bool AddSize(SizeDto sizeDto);
        public bool AddColour(ColourDto colourDto);
        public bool AddCategory(CategoryDto categoryDto);
        public List<Colour> GetColours();
        public List<Size> GetSizes();
        public List<Category> GetCategories();
        public bool EditProductById(int id, ProductToEditDto productToEditDto);
        public void ChangeProductStatus(int id);
        public void ChangeColourStatus(int id);
        public void ChangeSizeStatus(int id);
        public void ChangeCategoryStatus(int id);
    }
}
