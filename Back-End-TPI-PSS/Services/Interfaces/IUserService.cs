using Back_End_TPI_PSS.Data.Models;
using Back_End_TPI_PSS.Data.Models.UserDTOs;

namespace Back_End_TPI_PSS.Services.Interfaces
{
    public interface IUserService
    {
        public bool CreateUser(UserDto userDto);
        public bool ValidateUser(string Name);
        public bool UserLogin(UserLoginDto userLoginDto);
        public List<UserReturnDto> GetUsers();
        public bool DeleteUser(int id);
    }
}
