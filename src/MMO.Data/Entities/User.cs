using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMO.Data.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(128)]
        public string UserName { get; set; }

        [Required, MaxLength(128)]
        public string Password { get; set; }

        [Required, MaxLength(128)]
        public string Email { get; set; }

        public ICollection<Role> Roles { get; set; }

        public void SetPassword(string password) {
            Password = BCrypt.Net.BCrypt.HashPassword(password, 13);
        }

        public bool CheckPassword(string password) {
            return BCrypt.Net.BCrypt.Verify(password, Password);
        }
    }
}
