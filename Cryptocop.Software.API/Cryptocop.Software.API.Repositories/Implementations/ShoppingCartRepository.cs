using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Models.Exceptions;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Context;
using Cryptocop.Software.API.Repositories.Entities;
using Cryptocop.Software.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly CryptoCopDbContext _dbContext;
        private readonly IMapper _mapper;

        public ShoppingCartRepository(CryptoCopDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<ShoppingCartItemDto> GetCartItems(string email)
        {
            return _dbContext.ShoppingCartItems.AsNoTracking()
                        .Where(s => s.ShoppingCart.User.Email == email)
                        .Select(s => _mapper.Map<ShoppingCartItemDto>(s))
                        .ToList();
        }

        public void AddCartItem(string email, ShoppingCartItemInputModel shoppingCartItemItem, float priceInUsd)
        {
            int? userId = _dbContext.Users.Where(u => u.Email == email).Select(u => u.Id).FirstOrDefault();

            if (userId == null) return;

            ShoppingCart shoppingCart = _dbContext.ShoppingCarts.Where(sc => sc.User.Id == userId).FirstOrDefault();

            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart
                {
                    UserId = (int) userId
                };

                _dbContext.ShoppingCarts.Add(shoppingCart);
                _dbContext.SaveChanges();
            }

            ShoppingCartItem newItem = new ShoppingCartItem 
            {
                ShoppingCartId = shoppingCart.Id,
                ProductIdentifier = shoppingCartItemItem.ProductIdentifier,
                Quantity = (double) shoppingCartItemItem.Quantity,
                UnitPrice = priceInUsd
            };

            _dbContext.ShoppingCartItems.Add(newItem);
            _dbContext.SaveChanges();
        }

        public void RemoveCartItem(string email, int id)
        {
            ShoppingCartItem item = _dbContext.ShoppingCartItems
                                    .Where(i => i.Id == id && i.ShoppingCart.User.Email == email)
                                    .FirstOrDefault();

            if (item == null) throw new CartItemException($"Cart item with id {id} was not found within your cart items");

            _dbContext.ShoppingCartItems.Remove(item);
            _dbContext.SaveChanges();
        }

        public void UpdateCartItemQuantity(string email, int id, double quantity)
        {
            ShoppingCartItem item = _dbContext.ShoppingCartItems
                                    .Where(i => i.Id == id && i.ShoppingCart.User.Email == email)
                                    .FirstOrDefault();
            if (item == null) throw new CartItemException($"Cart item with id {id} was not found within your cart items");

            item.Quantity = quantity;
            _dbContext.SaveChanges();
        }

        public void ClearCart(string email)
        {
            var items = _dbContext.ShoppingCartItems
                        .Where(i => i.ShoppingCart.User.Email == email)
                        .ToList();

            foreach (var item in items)
            {
                _dbContext.ShoppingCartItems.Remove(item);
            }

            _dbContext.SaveChanges();
        }

        public void DeleteCart(string email)
        {
            throw new System.NotImplementedException();
        }
    }
}