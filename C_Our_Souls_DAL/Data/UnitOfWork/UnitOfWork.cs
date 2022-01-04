using C_Our_Souls_DAL.Data.Repository;
using C_Our_Souls_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_DAL.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public DatabaseContext _dbContext;

        public UnitOfWork(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        private IRepository<Boekenbeurs> boekenbeursRepository;
        private IRepository<Categorie> categorieRepository;
        private IRepository<Extra> extraRepository;
        private IRepository<Functie> functieRepository;
        private IRepository<Gebruiker> gebruikerRepository;
        private IRepository<GebruikerBoekenbeurs> gebruikerBoekenbeursRepository;
        private IRepository<LeeftijdsKlasse> leeftijdsKlasseRepository;
        private IRepository<Lidgeld> lidgeldRepository;
        private IRepository<Medewerker> medewerkerRepository;
        private IRepository<Medium> mediumRepository;
        private IRepository<MediumCategorie> mediumCategorieRepository;
        private IRepository<MediumDetail> mediumDetailRepository;
        private IRepository<MediumDetailMedewerker> mediumDetailMedewerkerRepository;
        private IRepository<MediumVerkoop> mediumVerkoopRepository;
        private IRepository<Soort> soortRepository;
        private IRepository<Uitlening> uitleningRepository;

        public IRepository<Boekenbeurs> BoekenbeursRepository
        {
            get
            {
                if (this.boekenbeursRepository == null)
                {
                    this.boekenbeursRepository = new Repository<Boekenbeurs>(_dbContext);
                }
                return this.boekenbeursRepository;
            }
        }

        public IRepository<Categorie> CategorieRepository
        {
            get
            {
                if (this.categorieRepository == null)
                {
                    this.categorieRepository = new Repository<Categorie>(_dbContext);
                }
                return this.categorieRepository;
            }
        }

        public IRepository<Extra> ExtraRepository
        {
            get
            {
                if (this.extraRepository == null)
                {
                    this.extraRepository = new Repository<Extra>(_dbContext);
                }
                return this.extraRepository;
            }
        }

        public IRepository<Functie> FunctieRepository
        {
            get
            {
                if (this.functieRepository == null)
                {
                    this.functieRepository = new Repository<Functie>(_dbContext);
                }
                return this.functieRepository;
            }
        }

        public IRepository<Gebruiker> GebruikerRepository
        {
            get
            {
                if (this.gebruikerRepository == null)
                {
                    this.gebruikerRepository = new Repository<Gebruiker>(_dbContext);
                }
                return this.gebruikerRepository;
            }
        }

        public IRepository<GebruikerBoekenbeurs> GebruikerBoekenbeursRepository
        {
            get
            {
                if (this.gebruikerBoekenbeursRepository == null)
                {
                    this.gebruikerBoekenbeursRepository = new Repository<GebruikerBoekenbeurs>(_dbContext);
                }
                return this.gebruikerBoekenbeursRepository;
            }
        }

        public IRepository<LeeftijdsKlasse> LeeftijdsKlasseRepository
        {
            get
            {
                if (this.leeftijdsKlasseRepository == null)
                {
                    this.leeftijdsKlasseRepository = new Repository<LeeftijdsKlasse>(_dbContext);
                }
                return this.leeftijdsKlasseRepository;
            }
        }

        public IRepository<Lidgeld> LidgeldRepository
        {
            get
            {
                if (this.lidgeldRepository == null)
                {
                    this.lidgeldRepository = new Repository<Lidgeld>(_dbContext);
                }
                return this.lidgeldRepository;
            }
        }

        public IRepository<Medewerker> MedewerkerRepository
        {
            get
            {
                if (this.medewerkerRepository == null)
                {
                    this.medewerkerRepository = new Repository<Medewerker>(_dbContext);
                }
                return this.medewerkerRepository;
            }
        }

        public IRepository<Medium> MediumRepository
        {
            get
            {
                if (this.mediumRepository == null)
                {
                    this.mediumRepository = new Repository<Medium>(_dbContext);
                }
                return this.mediumRepository;
            }
        }

        public IRepository<MediumCategorie> MediumCategorieRepository
        {
            get
            {
                if (this.mediumCategorieRepository == null)
                {
                    this.mediumCategorieRepository = new Repository<MediumCategorie>(_dbContext);
                }
                return this.mediumCategorieRepository;
            }
        }

        public IRepository<MediumDetail> MediumDetailRepository
        {
            get
            {
                if (this.mediumDetailRepository == null)
                {
                    this.mediumDetailRepository = new Repository<MediumDetail>(_dbContext);
                }
                return this.mediumDetailRepository;
            }
        }

        public IRepository<MediumDetailMedewerker> MediumDetailMedewerkerRepository
        {
            get
            {
                if (this.mediumDetailMedewerkerRepository == null)
                {
                    this.mediumDetailMedewerkerRepository = new Repository<MediumDetailMedewerker>(_dbContext);
                }
                return this.mediumDetailMedewerkerRepository;
            }
        }

        public IRepository<MediumVerkoop> MediumVerkoopRepository
        {
            get
            {
                if (this.mediumVerkoopRepository == null)
                {
                    this.mediumVerkoopRepository = new Repository<MediumVerkoop>(_dbContext);
                }
                return this.mediumVerkoopRepository;
            }
        }

        public IRepository<Soort> SoortRepository
        {
            get
            {
                if (this.soortRepository == null)
                {
                    this.soortRepository = new Repository<Soort>(_dbContext);
                }
                return this.soortRepository;
            }
        }

        public IRepository<Uitlening> UitleningRepository
        {
            get
            {
                if (this.uitleningRepository == null)
                {
                    this.uitleningRepository = new Repository<Uitlening>(_dbContext);
                }
                return this.uitleningRepository;
            }
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public int Save()
        {
            return _dbContext.SaveChanges();
        }
    }
}