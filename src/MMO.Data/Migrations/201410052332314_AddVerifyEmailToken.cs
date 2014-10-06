namespace MMO.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVerifyEmailToken : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "VerifyEmailToken", c => c.String(maxLength: 64, storeType: "nvarchar"));
            CreateIndex("dbo.Users", "VerifyEmailToken", true);
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "VerifyEmailToken");
        }
    }
}
