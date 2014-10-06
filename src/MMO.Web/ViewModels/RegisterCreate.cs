using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MMO.Web.ViewModels
{
    public class RegisterCreate
    {
        [Required, MaxLength(128)]
        public string Username { get; set; }

        [Required, MaxLength(128), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, DataType(DataType.Password), Compare("Password"), DisplayName("Confirm Password")]
        public string PasswordConfirm { get; set; }
    }
}