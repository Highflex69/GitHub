namespace Labb2_Dis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedForenKeyFromMessageToUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "Content", c => c.String());
            AddColumn("dbo.Messages", "Title", c => c.String());
            AddColumn("dbo.Messages", "To_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Messages", "To_Id");
            AddForeignKey("dbo.Messages", "To_Id", "dbo.User", "UserId");
            DropColumn("dbo.Messages", "To");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Messages", "To", c => c.String());
            DropForeignKey("dbo.Messages", "To_Id", "dbo.User");
            DropIndex("dbo.Messages", new[] { "To_Id" });
            DropColumn("dbo.Messages", "To_Id");
            DropColumn("dbo.Messages", "Title");
            DropColumn("dbo.Messages", "Content");
        }
    }
}
