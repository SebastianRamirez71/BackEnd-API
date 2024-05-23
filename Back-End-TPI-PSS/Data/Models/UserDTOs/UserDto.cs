using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Back_End_TPI_PSS.Data.Models.UserDTOs
{
    public class UserDto
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

}
