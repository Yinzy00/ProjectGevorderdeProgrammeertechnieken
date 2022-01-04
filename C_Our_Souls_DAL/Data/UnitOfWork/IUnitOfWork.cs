using C_Our_Souls_DAL.Data.Repository;
using C_Our_Souls_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_DAL.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Boekenbeurs> BoekenbeursRepository { get; }
        IRepository<Categorie> CategorieRepository { get; }
        IRepository<Extra> ExtraRepository { get; }
        IRepository<Functie> FunctieRepository { get; }
        IRepository<Gebruiker> GebruikerRepository { get; }
        IRepository<GebruikerBoekenbeurs> GebruikerBoekenbeursRepository { get; }
        IRepository<LeeftijdsKlasse> LeeftijdsKlasseRepository { get; }
        IRepository<Lidgeld> LidgeldRepository { get; }
        IRepository<Medewerker> MedewerkerRepository { get; }
        IRepository<Medium> MediumRepository { get; }
        IRepository<MediumCategorie> MediumCategorieRepository { get; }
        IRepository<MediumDetail> MediumDetailRepository { get; }
        IRepository<MediumDetailMedewerker> MediumDetailMedewerkerRepository { get; }
        IRepository<MediumVerkoop> MediumVerkoopRepository { get; }
        IRepository<Soort> SoortRepository { get; }
        IRepository<Uitlening> UitleningRepository { get; }

        int Save();
    }
}