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
        public string Category { get; set; }
        public List<Colour> ColourId { get; set; }
        public List<Size> SizeId { get; set; }
        public List<Category> CategoryId { get; set; }

    }
}
