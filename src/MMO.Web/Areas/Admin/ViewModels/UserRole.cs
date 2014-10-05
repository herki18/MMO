using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MMO.Web.Areas.Admin.ViewModels
{
    public class UserRole
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsSeleceted { get; set; }

        public UserRole() { }

        public UserRole(int id, string name, bool isSeleceted)
        {
            Id = id;
            Name = name;
            IsSeleceted = isSeleceted;
        }
    }
}