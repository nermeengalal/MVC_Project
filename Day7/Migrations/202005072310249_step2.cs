namespace Day7.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class step2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "Card_ID", "dbo.Cards");
            DropIndex("dbo.Products", new[] { "Card_ID" });
            CreateTable(
                "dbo.ProductCards",
                c => new
                    {
                        Product_ID = c.Int(nullable: false),
                        Card_ID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Product_ID, t.Card_ID })
                .ForeignKey("dbo.Products", t => t.Product_ID, cascadeDelete: true)
                .ForeignKey("dbo.Cards", t => t.Card_ID, cascadeDelete: true)
                .Index(t => t.Product_ID)
                .Index(t => t.Card_ID);
            
            DropColumn("dbo.Products", "Card_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Card_ID", c => c.Int());
            DropForeignKey("dbo.ProductCards", "Card_ID", "dbo.Cards");
            DropForeignKey("dbo.ProductCards", "Product_ID", "dbo.Products");
            DropIndex("dbo.ProductCards", new[] { "Card_ID" });
            DropIndex("dbo.ProductCards", new[] { "Product_ID" });
            DropTable("dbo.ProductCards");
            CreateIndex("dbo.Products", "Card_ID");
            AddForeignKey("dbo.Products", "Card_ID", "dbo.Cards", "ID");
        }
    }
}
