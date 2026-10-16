namespace LGYtechStockApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SyncCurrentModel : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Products", new[] { "Sku" });
            AlterColumn("dbo.Products", "Sku", c => c.String(nullable: false, maxLength: 50));
            CreateIndex("dbo.Products", "Sku", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Products", new[] { "Sku" });
            AlterColumn("dbo.Products", "Sku", c => c.String());
            CreateIndex("dbo.Products", "Sku", unique: true);
        }
    }
}
