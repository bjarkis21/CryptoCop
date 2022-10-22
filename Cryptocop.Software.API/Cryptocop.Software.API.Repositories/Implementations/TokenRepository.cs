using System.Linq;
using Cryptocop.Software.API.Models.Dtos;
using Cryptocop.Software.API.Repositories.Context;
using Cryptocop.Software.API.Repositories.Interfaces;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class TokenRepository : ITokenRepository
    {
        private readonly CryptoCopDbContext _dbContext;

        public TokenRepository(CryptoCopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public JwtTokenDto CreateNewToken()
        {
            throw new System.NotImplementedException();
        }

        public bool IsTokenBlacklisted(int tokenId)
        {
            var token = _dbContext.JwtTokens.Where(t => t.Id == tokenId).FirstOrDefault();
            if (token == null) return true;
            return token.Blacklisted;
        }

        public void VoidToken(int tokenId)
        {
            var token = _dbContext.JwtTokens.Where(t => t.Id == tokenId).FirstOrDefault();
            if (token == null) return;
            token.Blacklisted = true;
            _dbContext.SaveChanges();
        }
    }
}