using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Back_End_TPI_PSS.Data.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; } = "Cliente";
        public bool Status { get; set; } = false;
        public ICollection<Order> Orders { get; set; } = new List<Order>();

    }
}
