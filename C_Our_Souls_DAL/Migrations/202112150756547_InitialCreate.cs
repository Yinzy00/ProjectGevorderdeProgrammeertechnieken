namespace C_Our_Souls_DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "bib.Boekenbeurs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Naam = c.String(nullable: false),
                        DatumVan = c.DateTime(nullable: false),
                        DatumTot = c.DateTime(),
                        Locatie = c.String(),
                        InkomPrijs = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "bib.GebruikerBoekenbeurs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GebruikerId = c.Int(nullable: false),
                        BoekenbeursId = c.Int(nullable: false),
                        IngeschrevenOp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("bib.Boekenbeurs", t => t.BoekenbeursId, cascadeDelete: true)
                .ForeignKey("bib.Gebruiker", t => t.GebruikerId, cascadeDelete: true)
                .Index(t => t.GebruikerId)
                .Index(t => t.BoekenbeursId);
            
            CreateTable(
                "bib.Gebruiker",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Naam = c.String(nullable: false),
                        Voornaam = c.String(nullable: false),
                        Adres = c.String(nullable: false),
                        Postcode = c.String(nullable: false),
                        Woonplaats = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Wachtwoord = c.String(nullable: false),
                        Key = c.String(nullable: false),
                        Admin = c.Boolean(nullable: false),
                        LidmaatschapAanvraag = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "bib.MediumVerkoop",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IntresseDatum = c.DateTime(nullable: false),
                        MediumId = c.Int(nullable: false),
                        GebruikerId = c.Int(nullable: false),
                        Medium_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("bib.Gebruiker", t => t.GebruikerId, cascadeDelete: true)
                .ForeignKey("bib.Medium", t => t.Medium_Id)
                .Index(t => t.GebruikerId)
                .Index(t => t.Medium_Id);
            
            CreateTable(
                "bib.Medium",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MediumDetailId = c.Int(nullable: false),
                        Registratie = c.DateTime(nullable: false),
                        EindeLevensduur = c.DateTime(nullable: false),
                        Verkoopprijs = c.Double(nullable: false),
                        Verkocht = c.Boolean(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("bib.MediumDetail", t => t.MediumDetailId, cascadeDelete: true)
                .Index(t => t.MediumDetailId);
            
            CreateTable(
                "bib.MediumDetail",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        LeeftijdsKlasseId = c.Int(nullable: false),
                        Ean = c.String(nullable: false),
                        SoortId = c.Int(nullable: false),
                        KorteInhoud = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("bib.LeeftijdsKlasse", t => t.LeeftijdsKlasseId, cascadeDelete: true)
                .ForeignKey("bib.Soort", t => t.SoortId, cascadeDelete: true)
                .Index(t => t.LeeftijdsKlasseId)
                .Index(t => t.SoortId);
            
            CreateTable(
                "bib.LeeftijdsKlasse",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Omschrijving = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "bib.MediumCategorie",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MediumDetailId = c.Int(nullable: false),
                        CategorieId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("bib.Categorie", t => t.CategorieId, cascadeDelete: true)
                .ForeignKey("bib.MediumDetail", t => t.MediumDetailId, cascadeDelete: true)
                .Index(t => t.MediumDetailId)
                .Index(t => t.CategorieId);
            
            CreateTable(
                "bib.Categorie",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Omschrijving = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "bib.MediumDetailExtra",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MediumDetailId = c.Int(nullable: false),
                        ExtraId = c.Int(nullable: false),
                        Beschrijving = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("bib.Extra", t => t.ExtraId)
                .ForeignKey("bib.MediumDetail", t => t.MediumDetailId)
                .Index(t => t.MediumDetailId)
                .Index(t => t.ExtraId);
            
            CreateTable(
                "bib.Extra",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SoortId = c.Int(nullable: false),
                        Type = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("bib.Soort", t => t.SoortId, cascadeDelete: true)
                .Index(t => t.SoortId);
            
            CreateTable(
                "bib.Soort",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Naam = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "bib.MediumDetailMedewerker",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MedewerkerId = c.Int(nullable: false),
                        MediumDetailId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("bib.Medewerker", t => t.MedewerkerId, cascadeDelete: true)
                .ForeignKey("bib.MediumDetail", t => t.MediumDetailId, cascadeDelete: true)
                .Index(t => t.MedewerkerId)
                .Index(t => t.MediumDetailId);
            
            CreateTable(
                "bib.Medewerker",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Naam = c.String(nullable: false),
                        FunctieId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("bib.Functie", t => t.FunctieId, cascadeDelete: true)
                .Index(t => t.FunctieId);
            
            CreateTable(
                "bib.Functie",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Naam = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "bib.Uitlening",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MediumId = c.Int(nullable: false),
                        GebruikerId = c.Int(),
                        OntleenIntresse = c.DateTime(nullable: false),
                        UitgeleendOp = c.DateTime(),
                        Binnengebracht = c.DateTime(),
                        BoeteBetaald = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("bib.Gebruiker", t => t.GebruikerId)
                .ForeignKey("bib.Medium", t => t.MediumId, cascadeDelete: true)
                .Index(t => t.MediumId)
                .Index(t => t.GebruikerId);
            
            CreateTable(
                "bib.Lidgeld",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LidgeldBetaaldOp = c.DateTime(nullable: false),
                        DuurLidmaatschap = c.DateTime(nullable: false),
                        GebruikerId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("bib.Gebruiker", t => t.GebruikerId, cascadeDelete: true)
                .Index(t => t.GebruikerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("bib.Lidgeld", "GebruikerId", "bib.Gebruiker");
            DropForeignKey("bib.GebruikerBoekenbeurs", "GebruikerId", "bib.Gebruiker");
            DropForeignKey("bib.Uitlening", "MediumId", "bib.Medium");
            DropForeignKey("bib.Uitlening", "GebruikerId", "bib.Gebruiker");
            DropForeignKey("bib.MediumVerkoop", "Medium_Id", "bib.Medium");
            DropForeignKey("bib.MediumDetail", "SoortId", "bib.Soort");
            DropForeignKey("bib.MediumDetailMedewerker", "MediumDetailId", "bib.MediumDetail");
            DropForeignKey("bib.MediumDetailMedewerker", "MedewerkerId", "bib.Medewerker");
            DropForeignKey("bib.Medewerker", "FunctieId", "bib.Functie");
            DropForeignKey("bib.MediumDetailExtra", "MediumDetailId", "bib.MediumDetail");
            DropForeignKey("bib.MediumDetailExtra", "ExtraId", "bib.Extra");
            DropForeignKey("bib.Extra", "SoortId", "bib.Soort");
            DropForeignKey("bib.MediumCategorie", "MediumDetailId", "bib.MediumDetail");
            DropForeignKey("bib.MediumCategorie", "CategorieId", "bib.Categorie");
            DropForeignKey("bib.Medium", "MediumDetailId", "bib.MediumDetail");
            DropForeignKey("bib.MediumDetail", "LeeftijdsKlasseId", "bib.LeeftijdsKlasse");
            DropForeignKey("bib.MediumVerkoop", "GebruikerId", "bib.Gebruiker");
            DropForeignKey("bib.GebruikerBoekenbeurs", "BoekenbeursId", "bib.Boekenbeurs");
            DropIndex("bib.Lidgeld", new[] { "GebruikerId" });
            DropIndex("bib.Uitlening", new[] { "GebruikerId" });
            DropIndex("bib.Uitlening", new[] { "MediumId" });
            DropIndex("bib.Medewerker", new[] { "FunctieId" });
            DropIndex("bib.MediumDetailMedewerker", new[] { "MediumDetailId" });
            DropIndex("bib.MediumDetailMedewerker", new[] { "MedewerkerId" });
            DropIndex("bib.Extra", new[] { "SoortId" });
            DropIndex("bib.MediumDetailExtra", new[] { "ExtraId" });
            DropIndex("bib.MediumDetailExtra", new[] { "MediumDetailId" });
            DropIndex("bib.MediumCategorie", new[] { "CategorieId" });
            DropIndex("bib.MediumCategorie", new[] { "MediumDetailId" });
            DropIndex("bib.MediumDetail", new[] { "SoortId" });
            DropIndex("bib.MediumDetail", new[] { "LeeftijdsKlasseId" });
            DropIndex("bib.Medium", new[] { "MediumDetailId" });
            DropIndex("bib.MediumVerkoop", new[] { "Medium_Id" });
            DropIndex("bib.MediumVerkoop", new[] { "GebruikerId" });
            DropIndex("bib.GebruikerBoekenbeurs", new[] { "BoekenbeursId" });
            DropIndex("bib.GebruikerBoekenbeurs", new[] { "GebruikerId" });
            DropTable("bib.Lidgeld");
            DropTable("bib.Uitlening");
            DropTable("bib.Functie");
            DropTable("bib.Medewerker");
            DropTable("bib.MediumDetailMedewerker");
            DropTable("bib.Soort");
            DropTable("bib.Extra");
            DropTable("bib.MediumDetailExtra");
            DropTable("bib.Categorie");
            DropTable("bib.MediumCategorie");
            DropTable("bib.LeeftijdsKlasse");
            DropTable("bib.MediumDetail");
            DropTable("bib.Medium");
            DropTable("bib.MediumVerkoop");
            DropTable("bib.Gebruiker");
            DropTable("bib.GebruikerBoekenbeurs");
            DropTable("bib.Boekenbeurs");
        }
    }
}
