using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Back_End_TPI_PSS.Data.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Description { get; set; } 
        public string Image { get; set; }
        public string Category { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
        public bool Status { get; set; }
        public ICollection<Colour> Colours { get; set; } = new List<Colour>();
        public ICollection<Size> Sizes { get; set; } = new List<Size>();
    }
}
