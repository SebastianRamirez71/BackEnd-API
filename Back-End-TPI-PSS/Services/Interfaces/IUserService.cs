using Back_End_TPI_PSS.Data.Models;

namespace Back_End_TPI_PSS.Services.Interfaces
{
    public interface IUserService
    {
        public bool CreateUser(UserDto userDto);
        public bool ValidateUser(string Name);
        public bool UserLogin(string Email, string Password);
    }
}
