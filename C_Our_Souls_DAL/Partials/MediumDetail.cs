using C_Our_Souls_DAL.BasisModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_DAL.Models
{
    public partial class MediumDetail : Basisklasse
    {
        public string Genres
        {
            get
            {
                string returnvalue = "";
                int aantal = 0;
                foreach (var mediumcategorie in MediumCategorieen)
                {
                    if (aantal == 0)
                    {
                        returnvalue += mediumcategorie.Categorie.Omschrijving;
                        aantal++;
                    }
                    else
                    {
                        returnvalue += ", " + mediumcategorie.Categorie.Omschrijving;
                    }
                }
                return returnvalue;
            }
        }

        public string Auteurs
        {
            get
            {
                string returnvalue = "";
                int aantal = 0;
                foreach (var auteur in MediumDetailMedewerker)
                {
                    if (aantal == 0)
                    {
                        returnvalue += auteur.Medewerker.Naam;
                        aantal++;
                    }
                    else
                    {
                        returnvalue += ", " + auteur.Medewerker.Naam;
                    }
                }
                return returnvalue;
            }
        }

        public int Aantal
        {
            get
            {
                int aantalExemplaren = 0;
                foreach (var item in Medium)
                {
                    aantalExemplaren++;
                }
                return aantalExemplaren;
            }
        }

        public override string this[string columnName]
        {
            get
            {
                if (columnName == nameof(Title) && string.IsNullOrEmpty(Title))
                {
                    return "Titel is verplicht.";
                }
                if (columnName == nameof(Ean) && string.IsNullOrEmpty(Ean))
                {
                    return "Ean is verplicht.";
                }
                if (columnName == nameof(LeeftijdsKlasse) && LeeftijdsKlasse == null && LeeftijdsKlasseId == 0)
                {
                    return "Leeftijdsklasse is verplicht.";
                }
                if (columnName == nameof(Soort) && Soort == null && SoortId == 0)
                {
                    return "Medium type is verplicht.";
                }
                return "";
            }
        }
    }
}