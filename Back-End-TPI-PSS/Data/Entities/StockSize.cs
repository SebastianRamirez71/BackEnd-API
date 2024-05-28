using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Back_End_TPI_PSS.Data.Entities
{
    public class StockSize
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int StockId { get; set; }
        [ForeignKey("StockId")]
        public int SizeId { get; set; }
        [ForeignKey("SizeId")]
        public int Quantity { get; set; }
        [JsonIgnore]
        public Size Size { get; set; }
    }
}
