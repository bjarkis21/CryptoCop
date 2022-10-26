using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Context;
using Cryptocop.Software.API.Repositories.Entities;
using Cryptocop.Software.API.Repositories.Helpers;
using Cryptocop.Software.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly CryptoCopDbContext _dbContext;
        private readonly IMapper _mapper;

        public PaymentRepository(CryptoCopDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public void AddPaymentCard(string email, PaymentCardInputModel paymentCard)
        {
            int? userId = _dbContext.Users.Where(u => u.Email == email).Select(u => u.Id).FirstOrDefault();

            if (userId == null) return;

            PaymentCard newPC = _mapper.Map<PaymentCard>(paymentCard);
            newPC.UserId =  (int) userId;

            newPC.CardNumber = newPC.CardNumber.Replace("-", "").Replace(" ","");

            _dbContext.PaymentCards.Add(newPC);
            _dbContext.SaveChanges();
        }

        public IEnumerable<PaymentCardDto> GetStoredPaymentCards(string email)
        {
            return _dbContext.PaymentCards.AsNoTracking()
                    .Where(p => p.User.Email == email)
                    .Select(p => _mapper.Map<PaymentCardDto>(p))
                    .ToList();
        }
    }
}