using System.Text.Json.Serialization;

namespace Back_End_TPI_PSS.Data.Models.OrderDTOs
{
    public class OrderLineDto
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int ColourId { get; set; }
        public int SizeId { get; set; }
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
