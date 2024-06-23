using Back_End_TPI_PSS.Data.Entities;

namespace Back_End_TPI_PSS.Data.Models.ProductDTOs
{
    public class StockDto
    {
        public int ColourId { get; set; }
        public List<StockSizeDto> StockSizes { get; set; }
        public List<ImageDto> Images { get; set; }
        public bool Status { get; set; }
    }
}
