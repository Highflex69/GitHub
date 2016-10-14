namespace Labb2_Dis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFieldInTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "LastLogin", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "NoOfLoginThisMonth", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "NoOfUnreadMessages", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "NoOfUnreadMessages");
            DropColumn("dbo.AspNetUsers", "NoOfLoginThisMonth");
            DropColumn("dbo.AspNetUsers", "LastLogin");
        }
    }
}
