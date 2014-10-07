namespace MMO.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUploadSizeAndName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Uploads", "FileSizeBytes", c => c.Long(nullable: false));
            AddColumn("dbo.Uploads", "OriginalFileName", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Uploads", "OriginalFileName");
            DropColumn("dbo.Uploads", "FileSizeBytes");
        }
    }
}
