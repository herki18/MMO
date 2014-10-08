namespace MMO.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddInitialBuild : DbMigration
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
                .Index(t => t.Token, unique: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Password = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Email = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        VerifyEmailToken = c.String(maxLength: 64, storeType: "nvarchar"),
                        ResetPasswordToken = c.String(maxLength: 64, storeType: "nvarchar"),
                        ResetPasswordTokenExpiresAt = c.DateTime(precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Email, unique: true)
                .Index(t => t.VerifyEmailToken, unique: true)
                .Index(t => t.ResetPasswordToken, unique: true);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        IsUserDefined = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Uploads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Version_Version = c.Short(nullable: false),
                        Version_Timestamp = c.Int(nullable: false),
                        UploadedAt = c.DateTime(nullable: false, precision: 0),
                        FileSizeBytes = c.Long(nullable: false),
                        OriginalFileName = c.String(unicode: false),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DeployTokens",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IpAddress = c.String(nullable: false, maxLength: 64, storeType: "nvarchar"),
                        Token = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        CreatedAt = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Token, unique: true);
            
            CreateTable(
                "dbo.MMOSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Value = c.String(nullable: false, unicode: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Key, unique: true);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        Role_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Role_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.Role_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Role_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ClientAuthenticationTokens", "User_Id", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "Role_Id", "dbo.Roles");
            DropForeignKey("dbo.UserRoles", "User_Id", "dbo.Users");
            DropIndex("dbo.UserRoles", new[] { "Role_Id" });
            DropIndex("dbo.UserRoles", new[] { "User_Id" });
            DropIndex("dbo.MMOSettings", new[] { "Key" });
            DropIndex("dbo.DeployTokens", new[] { "Token" });
            DropIndex("dbo.Users", new[] { "ResetPasswordToken" });
            DropIndex("dbo.Users", new[] { "VerifyEmailToken" });
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.ClientAuthenticationTokens", new[] { "User_Id" });
            DropIndex("dbo.ClientAuthenticationTokens", new[] { "Token" });
            DropTable("dbo.UserRoles");
            DropTable("dbo.MMOSettings");
            DropTable("dbo.DeployTokens");
            DropTable("dbo.Uploads");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.ClientAuthenticationTokens");
        }
    }
}
