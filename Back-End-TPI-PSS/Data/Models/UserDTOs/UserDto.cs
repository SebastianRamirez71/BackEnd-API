using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Back_End_TPI_PSS.Data.Models.UserDTOs
{
    public class UserDto
    {
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        [JsonIgnore]
        public string UserType { get; set; }
    }
}
