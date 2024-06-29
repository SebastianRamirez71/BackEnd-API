using Xunit;
using Back_End_TPI_PSS.Context;
using Back_End_TPI_PSS.Data.Entities;
using Back_End_TPI_PSS.Data.Models.UserDTOs;
using Back_End_TPI_PSS.Services.Implementations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Back_End_TPI_PSS.Test
{
    public class UnitTest1
    {
        public UnitTest1()
        {
        }
        [Fact]
        public void CreateUser_WhenEmailIsValidAndNotExists()
        {
            // Arrange
            var userDto = new UserDto
            {
                Name = "Jose",
                SurName = "Jose",
                Email = "jose.alvarez@gmail.com",
                Password = "J0S3333L"
            };

            // Act
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            // Assert
            Assert.Equal("Jose", userDto.Name);
            Assert.Equal("jose.alvarez@gmail.com", userDto.Email);
            Assert.NotEqual(userDto.Password, hashedPassword);


            bool isPasswordValid = BCrypt.Net.BCrypt.Verify(userDto.Password, hashedPassword);
            Assert.True(isPasswordValid);
        }

        [Fact]
        public void CreateUser_WhenEmailIsNotValid()
        {
            // Arrange
            var userDto = new UserDto
            {
                Name = "Juan",
                SurName = "Perez",
                Email = "correo-invalido//",
                Password = "Password123"
            };

            // Act
            bool isEmailValid = IsValidEmail(userDto.Email);

            // Assert
            Assert.False(isEmailValid);
        }

        [Fact]
        public void CreateUser_WhenEmailIsValid()
        {
            // Arrange
            var userDto = new UserDto
            {
                Name = "Juan",
                SurName = "Perez",
                Email = "jperez@gmail.com",
                Password = "Jperez09"
            };

            // Act
            bool isEmailValid = IsValidEmail(userDto.Email);

            // Assert
            Assert.True(isEmailValid);
        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }


    }
}
