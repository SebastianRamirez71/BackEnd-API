using System.ComponentModel.DataAnnotations;

namespace Back_End_TPI_PSS.Data.Models.ProductDTOs
{
    public class ProductDto
    {
        public string Description { get; set; }
        public bool Status { get; set; } = true;
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string Genre { get; set; }
        public string Category { get; set; }
        public DateTime CreatedDate { get; set;} = DateTime.Now;
        public List<int> ColourId { get; set; }
        public List<int> SizeId { get; set; }
    }
}
