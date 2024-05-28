using Back_End_TPI_PSS.Data.Entities;

namespace Back_End_TPI_PSS.Data.Models.ProductDTOs
{
    public class ProductToEditDto
    {
        public string Description { get; set; }
        public bool Status { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string Genre { get; set; }
        public List<StockDto> Stocks { get; set; }
        public List<int> Category { get; set; }
    }
    
}
