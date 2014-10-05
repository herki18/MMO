using System.Data;
using System.Data.Entity.Core.Common.CommandTrees;

namespace MMO.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmailToUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Email", c => c.String(nullable: false, maxLength: 128, storeType: "nvarchar"));
            CreateIndex("dbo.Users", "Email", unique:true);
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Email");
        }
    }
}
