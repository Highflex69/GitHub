namespace Labb2_Dis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingLastLoginResetAttribut : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.User", "LastLoginReset", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.User", "LastLoginReset");
        }
    }
}
