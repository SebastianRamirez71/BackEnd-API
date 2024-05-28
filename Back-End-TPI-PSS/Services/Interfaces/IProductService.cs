using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Data.Models.ProductDTOs;

namespace Back_End_TPI_PSS.Services.Interfaces
{
    public interface IProductService
    {
        public bool AddProduct(ProductDto product);
        public Task<IEnumerable<Product>> GetProducts(string? priceOrder, string? size, string? genre, string? category, string? colour, string? dateOrder);
        public Task<IEnumerable<Product>> GetAllProducts();
        public bool AddSize(SizeDto sizeDto);
        public bool AddColour(ColourDto colourDto);
        public bool AddCategory(CategoryDto categoryDto);
        public bool EditProductById(int id, ProductToEditDto productToEditDto);
        public void ChangeEntityStatus<T>(int entityId) where T : class, IStatusEntity;
        List<T> GetActiveEntities<T>() where T : class, IStatusEntity;
        List<TDto> GetMappedVariants<TEntity, TDto>()
        where TEntity : class, IStatusEntity
        where TDto : class;
    }
}
