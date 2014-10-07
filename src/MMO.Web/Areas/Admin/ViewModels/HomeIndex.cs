using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MMO.Web.Areas.Admin.ViewModels
{
    public class HomeIndex
    {
        [DisplayName("Game Enabled for Roles")]
        public IEnumerable<UserRole> EnabledGameRoles { get; set; } 
    }
}