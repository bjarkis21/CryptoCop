using System;
using System.Linq;
using System.Text;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.Exceptions;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Context;
using Cryptocop.Software.API.Repositories.Entities;
using Cryptocop.Software.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly CryptoCopDbContext _dbContext;
        private string _salt = "00209b47-08d7-475d-a0fb-20abf0872ba0";

        public UserRepository(CryptoCopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public UserDto CreateUser(RegisterInputModel inputModel)
        {
            if (_dbContext.Users.Any(u => u.Email == inputModel.Email))
            {
                throw new ModelFormatException("This email address is already in use");
            }

            var user = new User {
                FullName = inputModel.FullName,
                Email = inputModel.Email,
                HashedPassword = HashPassword(inputModel.Password)
            };

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            return new UserDto();

            
        }

        public UserDto AuthenticateUser(LoginInputModel loginInputModel)
        {
            var user = _dbContext.Users.FirstOrDefault(u =>
                u.Email == loginInputModel.Email &&
                u.HashedPassword == HashPassword(loginInputModel.Password));
            if (user == null) { return null; }

            var token = new JwtToken();
            _dbContext.JwtTokens.Add(token);
            _dbContext.SaveChanges();

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                TokenId = token.Id
            };
        }

        private string HashPassword(string password)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: CreateSalt(),
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
        }

        private byte[] CreateSalt() =>
            Convert.FromBase64String(Convert.ToBase64String(Encoding.UTF8.GetBytes(_salt)));
    }
}