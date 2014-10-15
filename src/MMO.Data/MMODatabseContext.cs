using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMO.Base;
using MMO.Base.Infrastructure;
using MMO.Data.Entities;
using MMO.Data.Migrations;

namespace MMO.Data
{
    public class MMODatabseContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<DeployToken> DeployTokens { get; set; }
        public DbSet<Upload> Uploads { get; set; }
        public DbSet<Launcher> Launchers { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<MMOSetting> MmoSettings { get; set; }
        public DbSet<ClientAuthenticationToken> ClientAuthenticationTokens { get; set; }

        public MMODatabseContext() : base("MMOContext") {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MMODatabseContext, Configuration>("MMOContext"));
        }

        protected override void OnModelCreating(DbModelBuilder mapping) {
             var buildNumberMap = mapping.ComplexType<BuildNumber>();
            buildNumberMap.Property(t => t.Timestamp);
            buildNumberMap.Property(t => t.Version);



            mapping.Entity<Upload>()
                .Map<Client>(t => t.Requires("Type").HasValue((int)UploadType.Client))
                .Map<Launcher>(t => t.Requires("Type").HasValue((int)UploadType.Launcher));

            mapping.Entity<User>()
                .HasMany(t => t.Roles)
                .WithMany(t => t.Users);

        }
    }
}
