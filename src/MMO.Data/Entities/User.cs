using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Security.Cryptography;

namespace MMO.Data.Entities
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(128)]
        public string UserName { get; set; }

        [Required, MaxLength(128)]
        public string Password { get; set; }

        [Required, MaxLength(128), Index(IsUnique = true)]
        public string Email { get; set; }

        [MaxLength(64), Index(IsUnique = true)]
        public string VerifyEmailToken { get; set; }

        [MaxLength(64), Index(IsUnique = true)]
        public string ResetPasswordToken { get; set; }

        public DateTime? ResetPasswordTokenExpiresAt { get; set; }

        public ICollection<Role> Roles { get; set; }

        public void SetPassword(string password) {
            Password = BCrypt.Net.BCrypt.HashPassword(password, 13);
        }

        public bool CheckPassword(string password) {
            return BCrypt.Net.BCrypt.Verify(password, Password);
        }

        public void GenerateEmailVerificationToken() {
            VerifyEmailToken = GenerateRandomToken();
        }

        public void GenerateResetPasswordToken()
        {
            ResetPasswordToken = GenerateRandomToken();
            ResetPasswordTokenExpiresAt = DateTime.UtcNow.AddMinutes(30);
        }

        public void ClearResetPasswordToken() {
            ResetPasswordToken = null;
            ResetPasswordTokenExpiresAt = null;
        }

        private string GenerateRandomToken() {
            using (var random = new RNGCryptoServiceProvider())
            {
                var buffer = new byte[32];
                random.GetNonZeroBytes(buffer);
                return Convert.ToBase64String(buffer);
            }
        }

        
    }
}
