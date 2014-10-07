namespace MMO.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMMOSetting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MMOSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Key = c.String(nullable: false, maxLength: 128, storeType: "nvarchar"),
                        Value = c.String(nullable: false, unicode: false, storeType: "ntext"),
                    })
                .PrimaryKey(t => t.Id)
                .Index(f => f.Key, unique: true);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.MMOSettings");
        }
    }
}
