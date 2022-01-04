namespace C_Our_Souls_DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixedMedium : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("bib.MediumVerkoop", "Medium_Id", "bib.Medium");
            DropIndex("bib.MediumVerkoop", new[] { "Medium_Id" });
            DropColumn("bib.MediumVerkoop", "MediumId");
            RenameColumn(table: "bib.MediumVerkoop", name: "Medium_Id", newName: "MediumId");
            AlterColumn("bib.MediumVerkoop", "MediumId", c => c.Int(nullable: false));
            CreateIndex("bib.MediumVerkoop", "MediumId");
            AddForeignKey("bib.MediumVerkoop", "MediumId", "bib.Medium", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("bib.MediumVerkoop", "MediumId", "bib.Medium");
            DropIndex("bib.MediumVerkoop", new[] { "MediumId" });
            AlterColumn("bib.MediumVerkoop", "MediumId", c => c.Int());
            RenameColumn(table: "bib.MediumVerkoop", name: "MediumId", newName: "Medium_Id");
            AddColumn("bib.MediumVerkoop", "MediumId", c => c.Int(nullable: false));
            CreateIndex("bib.MediumVerkoop", "Medium_Id");
            AddForeignKey("bib.MediumVerkoop", "Medium_Id", "bib.Medium", "Id");
        }
    }
}
