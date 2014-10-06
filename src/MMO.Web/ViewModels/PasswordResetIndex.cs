using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MMO.Web.ViewModels
{
    public class PasswordResetIndex
    {
        [Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public PasswordResetError? Error { get; set; }
    }
}