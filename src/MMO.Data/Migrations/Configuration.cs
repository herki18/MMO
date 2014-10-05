using MMO.Data.Entities;

namespace MMO.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MMO.Data.MMODatabseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MMO.Data.MMODatabseContext context) {
            
            var adminRole = new Role {Name = "admin"};
            context.Roles.AddOrUpdate(t => t.Name, adminRole);

            var registeredRole = new Role {Name = "registered"};
            context.Roles.AddOrUpdate(t => t.Name, registeredRole);

            var admin = new User() {
                UserName = "admin",
                Roles = new[] {adminRole, registeredRole},
                Email = "admin@admin.ee"
            };

            admin.SetPassword("admin");

            if(!context.Users.Any(t=>t.UserName == "admin")) 
                context.Users.Add(admin);

        }
    }
}
