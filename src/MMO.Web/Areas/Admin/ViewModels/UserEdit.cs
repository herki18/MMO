using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MMO.Web.Areas.Admin.ViewModels
{
    public class UserEdit
    {
        [Required, MaxLength(128)]
        public string UserName { get; set; }

        [Required, MaxLength(128), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public IEnumerable<UserRole> Roles { get; set; }
    }
}