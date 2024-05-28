using Back_End_TPI_PSS.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Back_End_TPI_PSS.Data.Models.ProductDTOs
{
    public class ProductDto
    {
        public string Description { get; set; }
        public bool Status { get; set; } = true;
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string Genre { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public List<int> Category { get; set; }
        public List<StockDto> Stocks { get; set; }
    }
}
