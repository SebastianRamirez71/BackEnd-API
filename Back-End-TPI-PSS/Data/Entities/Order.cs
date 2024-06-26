using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Back_End_TPI_PSS.Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string PreferenceId { get; set; }
        public OrderStatus Status { get; set; } // Enumeración para los estados de la orden
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<OrderLine> OrderLines { get; set; }
    }

    public enum OrderStatus
    {
        Pending,
        Approved,
        Rejected
        // Agrega más estados según tus necesidades
    }
}
