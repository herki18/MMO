namespace MMO.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedToUserNewPasswordTokenFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "ResetPasswordToken", c => c.String(maxLength: 64, storeType: "nvarchar"));
            AddColumn("dbo.Users", "ResetPasswordTokenExpiresAt", c => c.DateTime(precision: 0));
            CreateIndex("Users", "ResetPasswordToken", unique: true);
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "ResetPasswordTokenExpiresAt");
            DropColumn("dbo.Users", "ResetPasswordToken");
        }
    }
}
