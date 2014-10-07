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
            
            var adminRole = new Role {Name = "admin", IsUserDefined = false};
            context.Roles.AddOrUpdate(t => t.Name, adminRole);

            var registeredRole = new Role {Name = "registered", IsUserDefined = false};
            context.Roles.AddOrUpdate(t => t.Name, registeredRole);

            

            if (!context.Users.Any(t => t.UserName == "admin")) {
                var admin = new User()
                {
                    UserName = "admin",
                    Roles = new[] { adminRole, registeredRole },
                    Email = "admin@admin.ee"
                };

                admin.SetPassword("admin");

                context.Users.Add(admin);
            }
                

        }
    }
}
