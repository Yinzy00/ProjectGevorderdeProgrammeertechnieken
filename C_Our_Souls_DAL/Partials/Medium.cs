using C_Our_Souls_DAL.BasisModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_DAL.Models
{
    public partial class Medium : Basisklasse
    {
        public override string this[string columnName]
        {
            get
            {
                if (columnName == nameof(EindeLevensduur) && EindeLevensduur < DateTime.Now)
                {
                    return "Vervaldatum kan niet in het verleden liggen!";
                }
                if (columnName == nameof(Verkoopprijs) && Verkoopprijs < 0)
                {
                    return "Prijs is een verplicht veld.";
                }
                return "";
            }
        }

        public int AantalUitleningen
        {
            get
            {
                int aantalUitleningen = 0;
                foreach (var item in Uitleningen.Where(x => x.UitgeleendOp != null))
                {
                    aantalUitleningen++;
                }
                return aantalUitleningen;
            }
        }

        public DateTime LaatsteUitlening
        {
            get
            {
                return (DateTime)Uitleningen.Where(x => x.UitgeleendOp != null).LastOrDefault().UitgeleendOp;
            }
        }
    }
}