using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MMO.Data.Entities;

namespace MMO.Data.Services
{
    public class ClientAuthenticationTokenService 
    {

        private readonly MMODatabseContext _database;

        public ClientAuthenticationTokenService(MMODatabseContext database) {
            _database = database;
        }

        public string GenerateTokenFor(string requestIp, User user) {
            _database.ClientAuthenticationTokens.RemoveRange(_database.ClientAuthenticationTokens.Where(t => t.User.Id == user.Id));

            var token = new ClientAuthenticationToken() {
                CreatedAt = DateTime.UtcNow,
                RequestIp = requestIp,
                User = user,
                Token = GenerateRandomToken()
            };

            _database.ClientAuthenticationTokens.Add(token);
            _database.SaveChanges();

            return token.Token;
        }

        private string GenerateRandomToken()
        {
            using (var random = new RNGCryptoServiceProvider())
            {
                var buffer = new byte[32];
                random.GetNonZeroBytes(buffer);
                return Convert.ToBase64String(buffer);
            }
        }
    }
}
