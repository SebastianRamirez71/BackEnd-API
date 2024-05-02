using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Data.Models.ColoursAndSizesDTOs;
using Back_End_TPI_PSS.Data.Models.ProductDTOs;

namespace Back_End_TPI_PSS.Services.Interfaces
{
    public interface IProductService
    {
        public bool AddProduct(ProductDto product);
        public List<Product> GetProducts();
        public bool AddSize(SizeDto sizeDto);
        public bool AddColour(ColourDto colourDto);
        public List<Colour> GetColours();
        public List<Size> GetSizes();
    }
}
