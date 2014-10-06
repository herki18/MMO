namespace MMO.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createdeploytoken : DbMigration
    {
        public override void Up()
        {
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
                .Index(t => t.Token, unique:true);
        }
        
        public override void Down()
        {
            DropTable("dbo.DeployTokens");
        }
    }
}
