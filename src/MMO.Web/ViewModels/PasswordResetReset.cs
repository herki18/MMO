using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MMO.Web.ViewModels
{
    public class PasswordResetReset
    {
        
        public string Username { get; set; }

        [Required, DataType(DataType.Password), DisplayName("New Password")]
        public string NewPassword { get; set; }

        [Required, DataType(DataType.Password), DisplayName("Confirm New Password"), Compare("NewPassword")]
        public string NewPasswordConfirm { get; set; }
    }
}