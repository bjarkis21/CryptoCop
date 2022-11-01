using System;
using System.Collections.Generic;
using System.Linq;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.Exceptions;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Context;
using Cryptocop.Software.API.Repositories.Entities;
using Cryptocop.Software.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly CryptoCopDbContext _dbContext;

        public OrderRepository(CryptoCopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<OrderDto> GetOrders(string email)
        {
            throw new NotImplementedException();
        }

        public OrderDto CreateNewOrder(string email, OrderInputModel order)
        {
            ShoppingCart shoppingCart = _dbContext.ShoppingCarts.AsNoTracking()
                                        .Include(sc => sc.ShoppingCartItems)
                                        .Where(sc => sc.User.Email == email)
                                        .FirstOrDefault();

            if (shoppingCart == null || shoppingCart.ShoppingCartItems.Count() == 0)
            {
                throw new OrderException("Shopping cart is currently empty");
            }

            PaymentCard paymentCard = _dbContext.PaymentCards.AsNoTracking()
                                        .Where(pc => pc.User.Email == email && pc.Id == order.PaymentCardId)
                                        .FirstOrDefault();

            if (paymentCard == null)
            {
                throw new ResourceNotFoundException($"Card with id {order.PaymentCardId} was not found");
            }

            Address address = _dbContext.Addresses.AsNoTracking()
                                .Where(a => a.User.Email == email && a.Id == order.AddressId)
                                .FirstOrDefault();

            if (address == null)
            {
                throw new ResourceNotFoundException($"Address with id {order.AddressId} was not found");
            }

            User user = _dbContext.Users
                            .Where(u => u.Email == email)
                            .FirstOrDefault();

            if (user == null)
            {
                throw new ResourceNotFoundException($"User with email {email} was not found");
            }
            

            Order newOrder = new Order
            {
                Email = email,
                FullName = user.FullName,
                StreetName = address.StreetName,
                HouseNumber = address.HouseNumber,
                ZipCode = address.ZipCode,
                Country = address.Country,
                City = address.City,
                CardHolderName = paymentCard.CardholderName,
                MaskedCreditCard = "************" + paymentCard.CardNumber.Substring(12),
                OrderDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                OrderItems = shoppingCart.ShoppingCartItems.Select(i => new OrderItem {
                    ProductIdentifier = i.ProductIdentifier,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.Quantity*i.UnitPrice
                }).ToList(),
                TotalPrice = shoppingCart.ShoppingCartItems.Select(i => i.Quantity*i.UnitPrice).Sum(),
                User = user
            };

            _dbContext.Orders.Add(newOrder);
            _dbContext.SaveChanges();

            return new OrderDto {
                Id = newOrder.Id,
                Email = newOrder.Email,
                FullName = newOrder.FullName,
                HouseNumber = newOrder.HouseNumber,
                ZipCode = newOrder.ZipCode,
                Country = newOrder.Country,
                City = newOrder.City,
                StreetName = newOrder.StreetName,
                CardHolderName = newOrder.CardHolderName,
                CreditCard = paymentCard.CardNumber,
                OrderDate = newOrder.OrderDate.ToString("MM.dd.yyyy"),
                TotalPrice = newOrder.TotalPrice,
                OrderItems = newOrder.OrderItems.Select(i => new OrderItemDto {
                    Id = i.Id,
                    ProductIdentifier = i.ProductIdentifier,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.TotalPrice
                }).ToList()
            };
        }
    }
}