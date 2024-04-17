using AutoMapper;
using Back_End_TPI_PSS.Context;
using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Data.Models;
using Back_End_TPI_PSS.Data.Models.UserDTOs;
using Back_End_TPI_PSS.Mappings;
using Back_End_TPI_PSS.Services.Interfaces;

namespace Back_End_TPI_PSS.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly PPSContext _context;
        private readonly IMapper _mapper;
        public UserService(PPSContext context)
        {
            _context = context;
            _mapper = AutoMapperConfig.Configure();
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

        public bool UserLogin(UserLoginDto userLoginDto)
        {
            User? userForLogin = _context.Users.SingleOrDefault(u => u.Email == userLoginDto.Email);

            if (userForLogin != null)
            {
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(userLoginDto.Password, userForLogin.Password);

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

        public List<UserReturnDto> GetUsers()
        {
            return _mapper.Map<List<UserReturnDto>>(_context.Users.ToList());
        }

        public bool DeleteUser(int id)
        {
            var existingUser = _context.Users.FirstOrDefault(x => x.Id == id && x.Status == true); 
            if (existingUser != null) 
            {
                existingUser.Status = false;
                _context.Users.Update(existingUser);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
