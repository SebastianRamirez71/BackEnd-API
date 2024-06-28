using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Back_End_TPI_PSS.Data.Entities
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string PreferenceId { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Relación con User
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public User User { get; set; }

        // Relación con Product
        //[ForeignKey("ProductId")]
        //public int ProductId { get; set; }
        //public Product Product { get; set; }

        public ICollection<OrderLine> OrderLines { get; set; }
    }

    public enum OrderStatus
    {
        Pending,
        Approved,
        Rejected
    }
}
