using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MMO.Web.Areas.Admin.ViewModels
{
    public class RolesEdit
    {
        [Required, MaxLength(128)]
        public string Name { get; set; }
    }
}