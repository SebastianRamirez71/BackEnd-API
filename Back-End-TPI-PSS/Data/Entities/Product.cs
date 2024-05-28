using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using Back_End_TPI_PSS.Services.Interfaces;

namespace Back_End_TPI_PSS.Data.Entities
{
    public class Product : IStatusEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public string Genre { get; set; }
        public decimal Price { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<Stock> Stocks { get; set; } = new List<Stock>();
        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
