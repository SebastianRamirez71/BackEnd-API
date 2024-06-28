using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Back_End_TPI_PSS.Data.Entities
{
    public class OrderLine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int OrderId { get; set; }  // Clave foránea

        [ForeignKey("OrderId")]
        public Order Order { get; set; }  // Relación con Order

        public string PreferenceId { get; set; }

        public string Description { get; set; }

        public int ProductId { get; set; }  // Clave foránea

        [ForeignKey("ProductId")]
        public Product Product { get; set; }  // Relación con Product

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public int ColorId { get; set; }  // Clave foránea

        [ForeignKey("ColorId")]
        public Colour Color { get; set; }  // Relación con Color

        public int SizeId { get; set; }  // Clave foránea

        [ForeignKey("SizeId")]
        public Size Size { get; set; }  // Relación con Size
    }
}
