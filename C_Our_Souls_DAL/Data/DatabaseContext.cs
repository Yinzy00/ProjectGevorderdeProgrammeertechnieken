using C_Our_Souls_DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_DAL.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("name=bibConnection")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("bib");
            //Boekenbeurs
            modelBuilder.Entity<Boekenbeurs>().ToTable("Boekenbeurs");
            //Categorie
            modelBuilder.Entity<Categorie>().ToTable("Categorie");
            //Extra
            modelBuilder.Entity<Extra>().ToTable("Extra");
            //Functie
            modelBuilder.Entity<Functie>().ToTable("Functie");
            //Gebruiker
            modelBuilder.Entity<Gebruiker>().ToTable("Gebruiker");
            //GebruikerBoekenbeurs
            modelBuilder.Entity<GebruikerBoekenbeurs>().ToTable("GebruikerBoekenbeurs");
            //LeeftijdsKlasse
            modelBuilder.Entity<LeeftijdsKlasse>().ToTable("LeeftijdsKlasse");
            //Lidgeld
            modelBuilder.Entity<Lidgeld>().ToTable("Lidgeld");
            //Medewerker
            modelBuilder.Entity<Medewerker>().ToTable("Medewerker");
            modelBuilder.Entity<Medewerker>().HasMany(m => m.MediumDetailMedewerkers).WithRequired(mdm => mdm.Medewerker).WillCascadeOnDelete();
            //MediumCategorie
            modelBuilder.Entity<MediumCategorie>().ToTable("MediumCategorie");
            //MediumDetail
            modelBuilder.Entity<MediumDetail>().ToTable("MediumDetail");
            modelBuilder.Entity<MediumDetail>().HasMany(md => md.MediumDetailMedewerker).WithRequired(mdm => mdm.MediumDetail).WillCascadeOnDelete();
            //MediumDetailMedewerker
            modelBuilder.Entity<MediumDetailMedewerker>().ToTable("MediumDetailMedewerker");
            //MediumVerkoop
            modelBuilder.Entity<MediumVerkoop>().ToTable("MediumVerkoop");
            modelBuilder.Entity<MediumVerkoop>().HasRequired(mv => mv.Medium).WithMany(m=>m.MediumVerkoopen);
            //Soort
            modelBuilder.Entity<Soort>().ToTable("Soort");
            modelBuilder.Entity<Soort>().HasMany(s => s.Extras).WithRequired(e => e.Soort).WillCascadeOnDelete();
            //Uitlening
            modelBuilder.Entity<Uitlening>().ToTable("Uitlening");
            //Medium
            modelBuilder.Entity<Medium>().ToTable("Medium");
            modelBuilder.Entity<Medium>().HasMany(m => m.MediumVerkoopen).WithRequired(mv => mv.Medium);
            //MediumDetailExtra
            modelBuilder.Entity<MediumDetailExtra>().ToTable("MediumDetailExtra");
            modelBuilder.Entity<MediumDetailExtra>().HasRequired(mde => mde.MediumDetail).WithMany(md => md.MediumDetailExtras).WillCascadeOnDelete(false);
            modelBuilder.Entity<MediumDetailExtra>().HasRequired(mde => mde.Extra).WithMany(e => e.MediumDetailExtras).WillCascadeOnDelete(false);
            //modelBuilder.Entity<Medium>().HasMany(m => m.MediumVerkoopen).WithOptionalPrincipal(mv => mv.Medium);
        }

        public DbSet<Boekenbeurs> Boekenbeurzen { get; set; }
        public DbSet<Categorie> Categorieen { get; set; }
        public DbSet<Extra> Extras { get; set; }
        public DbSet<Functie> Functies { get; set; }
        public DbSet<Gebruiker> Gebruikers { get; set; }
        public DbSet<GebruikerBoekenbeurs> GebruikerBoekenbeurzen { get; set; }
        public DbSet<LeeftijdsKlasse> LeeftijdsKlasses { get; set; }
        public DbSet<Lidgeld> Lidgelden { get; set; }
        public DbSet<Medewerker> Medewerkers { get; set; }
        public DbSet<Medium> Medium { get; set; }
        public DbSet<MediumCategorie> MediumCategorieen { get; set; }
        public DbSet<MediumDetail> MediumDetails { get; set; }
        public DbSet<MediumDetailMedewerker> MediumDetailMedewerkers { get; set; }
        public DbSet<MediumVerkoop> MediumVerkoopen { get; set; }
        public DbSet<Soort> Soorten { get; set; }
        public DbSet<Uitlening> Uitleningen { get; set; }
    }
}