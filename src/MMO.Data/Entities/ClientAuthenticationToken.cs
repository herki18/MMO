using System;
using System.ComponentModel.DataAnnotations;

namespace MMO.Data.Entities
{
    public class ClientAuthenticationToken
    {
        public int Id { get; set; }

        [Required, MaxLength(64)]
        public string Token { get; set; }

        [Required, MaxLength(64)]
        public string RequestIp { get; set; }

        [Required]
        public User User { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
