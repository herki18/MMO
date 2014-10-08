using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;

namespace MMO.Data.Entities
{
    public class DeployToken
    {
        public int Id { get; set; }
        [Required, MaxLength(64)]
        public string IpAddress { get; set; }
        [Required, MaxLength(128), Index(IsUnique = true)]
        public string Token { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        public DeployToken() {}

        public DeployToken(string ipAddress) {
            
            IpAddress = ipAddress;
            CreatedAt = DateTime.UtcNow;

            using (var random = new RNGCryptoServiceProvider())
            {
                var buffer = new byte[32];
                random.GetNonZeroBytes(buffer);
                Token = Convert.ToBase64String(buffer);
            }
        }
    }
}
