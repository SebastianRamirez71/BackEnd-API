using Back_End_TPI_PSS.Data.Entities;
using System.Text.Json.Serialization;

namespace Back_End_TPI_PSS.Data.Models.OrderDTOs
{
    public class OrderDto
    {
        public string PreferenceId { get; set; }
        public string OrderStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<OrderLine> OrderLines { get; set; }
    }

}
