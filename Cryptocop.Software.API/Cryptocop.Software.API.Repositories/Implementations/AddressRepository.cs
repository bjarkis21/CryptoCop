using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Context;
using Cryptocop.Software.API.Repositories.Entities;
using Cryptocop.Software.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class AddressRepository : IAddressRepository
    {
        private readonly CryptoCopDbContext _dbContext;
        private readonly IMapper _mapper;

        public AddressRepository(CryptoCopDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public void AddAddress(string email, AddressInputModel address)
        {
            var userId = _dbContext.Users.AsNoTracking().Where(u => u.Email == email).Select(u => u.Id).FirstOrDefault();
            Address newAddress = _mapper.Map<Address>(address);
            newAddress.UserId = userId;
            _dbContext.Addresses.Add(newAddress);
            _dbContext.SaveChanges();
        }

        public IEnumerable<AddressDto> GetAllAddresses(string email)
        {
            return _dbContext.Addresses.AsNoTracking()
                    .Where(a => a.User.Email == email)
                    .Select(a => _mapper.Map<AddressDto>(a))
                    .ToList();
        }

        public void DeleteAddress(string email, int addressId)
        {
            var address = _dbContext.Addresses.Where(a => a.User.Email == email && a.Id == addressId)
                                .FirstOrDefault();

            if (address == null) return;

            _dbContext.Addresses.Remove(address);
            _dbContext.SaveChanges();
        }
    }
}