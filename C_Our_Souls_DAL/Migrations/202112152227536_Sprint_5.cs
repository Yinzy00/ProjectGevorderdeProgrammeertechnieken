namespace C_Our_Souls_DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sprint_5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("bib.Uitlening", "LaatsteEmail", c => c.DateTime());
            AddColumn("bib.Lidgeld", "LastEmail", c => c.DateTime());
            AlterColumn("bib.Gebruiker", "Naam", c => c.String());
            AlterColumn("bib.Gebruiker", "Voornaam", c => c.String());
            AlterColumn("bib.Gebruiker", "Adres", c => c.String());
            AlterColumn("bib.Gebruiker", "Postcode", c => c.String());
            AlterColumn("bib.Gebruiker", "Woonplaats", c => c.String());
            AlterColumn("bib.Gebruiker", "Admin", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("bib.Gebruiker", "Admin", c => c.Boolean(nullable: false));
            AlterColumn("bib.Gebruiker", "Woonplaats", c => c.String(nullable: false));
            AlterColumn("bib.Gebruiker", "Postcode", c => c.String(nullable: false));
            AlterColumn("bib.Gebruiker", "Adres", c => c.String(nullable: false));
            AlterColumn("bib.Gebruiker", "Voornaam", c => c.String(nullable: false));
            AlterColumn("bib.Gebruiker", "Naam", c => c.String(nullable: false));
            DropColumn("bib.Lidgeld", "LastEmail");
            DropColumn("bib.Uitlening", "LaatsteEmail");
        }
    }
}
