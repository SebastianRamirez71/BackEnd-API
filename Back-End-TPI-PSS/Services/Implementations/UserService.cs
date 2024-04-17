using Back_End_TPI_PSS.Context;
using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Data.Models;
using Back_End_TPI_PSS.Services.Interfaces;


namespace Back_End_TPI_PSS.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly PPSContext _context;
        public UserService(PPSContext context)
        {
            _context = context;
        }

        public bool CreateUser(UserDto userDto) 
        {
            if (ValidateUser(userDto.Name))
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

                var newUser = new User()
                {
                    Name = userDto.Name,
                    SurName = userDto.SurName,
                    Email = userDto.Email,
                    Password = hashedPassword,
                };
                _context.Users.Add(newUser);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool ValidateUser(string Name) 
        {
            bool existingName = _context.Users.Any(u => u.Name == Name);

            if (existingName == false)
            {
                return true;
            }
            return false;
        }

        public bool UserLogin(string Email, string Password)
        {
            User? userForLogin = _context.Users.SingleOrDefault(u => u.Email == Email);

            if (userForLogin != null)
            {
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(Password, userForLogin.Password);

                if (isPasswordValid)
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
            return false;
        }
    }
}
