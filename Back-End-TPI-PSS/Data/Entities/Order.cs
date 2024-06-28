using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Back_End_TPI_PSS.Data.Entities
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string PreferenceId { get; set; }

        public int ProductQuantity { get; set; }

        public string Status { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int UserId { get; set; }  // Clave foránea

        [ForeignKey("UserId")]
        public User User { get; set; }  // Relación con User

        public ICollection<OrderLine> OrderLines { get; set; }  // Relación con OrderLines
    }
}
