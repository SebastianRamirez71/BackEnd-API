using System.ComponentModel.DataAnnotations;

namespace Back_End_TPI_PSS.Data.Models.ProductDTOs
{
    public class ProductDto
    {
        [Required(ErrorMessage ="La descripcion es obligatoria")]
        public string Description { get; set; }
        public bool Status { get; set; } = true;
        [Range(0, double.MaxValue, ErrorMessage ="El precio debe ser mayor a 0")]
        public decimal Price { get; set; }
        public string Image { get; set; }
        [Required(ErrorMessage = "La categoria es obligatoria")]
        public string Genre { get; set; }
        public string Category { get; set; }
        public List<int> ColourId { get; set; }
        public List<int> SizeId { get; set; }
    }
}
