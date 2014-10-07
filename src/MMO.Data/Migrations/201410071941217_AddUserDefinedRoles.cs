namespace MMO.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserDefinedRoles : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Roles", "IsUserDefined", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Roles", "IsUserDefined");
        }
    }
}
