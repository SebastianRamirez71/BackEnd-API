using System.Text.Json.Serialization;

namespace Back_End_TPI_PSS.Data.Models.OrderDTOs
{
    public class OrderDto
    {
        [JsonIgnore]
        public int Id { get; set; }
        public bool Status { get; set; } = true;
        [JsonIgnore]
        public int UserId { get; set; }
    }
}
