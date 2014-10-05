using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMO.Data.Entities;

namespace MMO.Data
{
    public class MMODatabseContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
 
        public MMODatabseContext() : base("MMOContext")
        { }
    }
}
