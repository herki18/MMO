namespace MMO.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddClientAuthenticationToken : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ClientAuthenticationTokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Token = c.String(nullable: false, maxLength: 64, storeType: "nvarchar"),
                        RequestIp = c.String(nullable: false, maxLength: 64, storeType: "nvarchar"),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                        User_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Token, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClientAuthenticationTokens", "User_Id", "dbo.Users");
            DropIndex("dbo.ClientAuthenticationTokens", new[] { "User_Id" });
            DropTable("dbo.ClientAuthenticationTokens");
        }
    }
}
