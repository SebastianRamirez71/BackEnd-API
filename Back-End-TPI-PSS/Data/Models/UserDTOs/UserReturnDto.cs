using System.Text.Json.Serialization;

namespace Back_End_TPI_PSS.Data.Models.UserDTOs
{
    public class UserReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
    }
}
