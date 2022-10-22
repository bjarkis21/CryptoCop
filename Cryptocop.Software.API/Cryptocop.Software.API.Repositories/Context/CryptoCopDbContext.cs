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
    }
}