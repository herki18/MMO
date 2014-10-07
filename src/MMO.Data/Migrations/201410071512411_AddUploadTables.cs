namespace MMO.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUploadTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Uploads",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Version_Version = c.Short(nullable: false),
                        Version_Timestamp = c.Int(nullable: false),
                        UploadedAt = c.DateTime(nullable: false, precision: 0),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Uploads");
        }
    }
}
