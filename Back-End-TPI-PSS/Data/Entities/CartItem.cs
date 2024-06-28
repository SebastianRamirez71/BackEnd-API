using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Back_End_TPI_PSS.Models
{
    public class CartItem
    {
        [Required]
        public int id { get; set; }
        [Required]
        public int ProductId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]

        public string Color { get; set; }

        [Required]

        public string SizeName { get; set; }

        [Required]
        public int ColorId { get; set; }

        [Required]

        public string Image { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int SizeId { get; set; }
    }
}

