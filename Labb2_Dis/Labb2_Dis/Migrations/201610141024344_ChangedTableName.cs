namespace Labb2_Dis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedTableName : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AspNetUsers", newName: "User");
            RenameColumn(table: "dbo.User", name: "Id", newName: "UserId");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.User", name: "UserId", newName: "Id");
            RenameTable(name: "dbo.User", newName: "AspNetUsers");
        }
    }
}
