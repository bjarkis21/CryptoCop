using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cryptocop.Software.API.Repositories.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cryptocop.Software.API.Repositories.Context
{
    public class CryptoCopDbContext : DbContext
    {
        public CryptoCopDbContext(DbContextOptions<CryptoCopDbContext> options) : base(options) {}

        public DbSet<User> Users {get; set;}
        public DbSet<JwtToken> JwtTokens {get; set;}
        public DbSet<Address> Addresses {get; set;}
        public DbSet<PaymentCard> PaymentCards {get; set;}
        public DbSet<ShoppingCart> ShoppingCarts {get; set;}
        public DbSet<ShoppingCartItem> ShoppingCartItems {get; set;}
    }
}