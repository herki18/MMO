using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MMO.Data.Entities;

namespace MMO.Web.Areas.Admin.ViewModels
{
    public class RolesIndex
    {
        public IEnumerable<Role> Roles { get; set; } 
    }
}