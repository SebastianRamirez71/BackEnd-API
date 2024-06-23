using Back_End_TPI_PSS.Data.Models.ProductDTOs;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Back_End_TPI_PSS.Data.Entities
{
    public class Stock
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public int ColourId { get; set; }
        [ForeignKey("ColourId")]

        [JsonIgnore]
        public Colour Colour { get; set; }   
        public List<StockSize> StockSizes { get; set; } = new List<StockSize>();
        public List<Image> Images { get; set; } = new List<Image>();
        public bool Status { get; set; }
    }
}
