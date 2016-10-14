namespace Labb2_Dis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingMessageTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        isRead = c.Boolean(nullable: false),
                        To = c.String(),
                        From = c.String(),
                        Date = c.DateTime(nullable: false),
                        IsRemoved = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.MessageId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Messages");
        }
    }
}
