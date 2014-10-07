using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace MMO.Data.Entities
{
    public class Role
    {
        public int Id { get; set; }

        [Required, MaxLength(128)]
        public string Name { get; set; }

        public bool IsUserDefined { get; set; }

        [NotMapped]
        public bool CanEditAndDelete { get { return IsUserDefined; } }

        public virtual ICollection<User> Users { get; set; } 
    }
}
