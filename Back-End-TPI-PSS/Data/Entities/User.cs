using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Back_End_TPI_PSS.Data.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SurName { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; }
        public string Password { get; set; } = string.Empty;
        public string UserType { get; set; } = "Cliente";
        public bool Status { get; set; } = true;
        public bool Notification { get; set; } = false;
        public string VerificationCode { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime Expiry { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();

    }
}