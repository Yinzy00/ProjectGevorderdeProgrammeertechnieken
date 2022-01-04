using C_Our_Souls_DAL.BasisModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace C_Our_Souls_DAL.Models
{
    public partial class Gebruiker : Basisklasse
    {
        public string VolledigeNaam
        {
            get
            {
                return $"{Voornaam} {Naam}";
            }
        }

        public override string this[string columnName]
        {
            get
            {
                //Checks for all types of accounts
                if (columnName == "Email" && string.IsNullOrEmpty(Email))
                {
                    return "Email adres is een verplicht veld.";
                }
                //^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$ => OLD REGEX (Updated)
                else if (columnName == "Email" && !new Regex(@"^([\w\.\-]+)@([\w\-]+)([\.\w]+)((\.(\w){2,3})+)$").Match(Email).Success)
                {
                    return "Het ingevulde email adres is niet geldig.";
                }
                if (columnName == "Wachtwoord" && string.IsNullOrEmpty(Wachtwoord))
                {
                    return "Wachtwoord is een verplicht veld.";
                }
                //Checks for "Gebruiker"
                if (Admin == GebruikerType.Gebruiker)
                {
                    if (columnName == "Naam" && string.IsNullOrEmpty(Naam))
                    {
                        return "Naam is een verplicht veld.";
                    }
                    if (columnName == "Voornaam" && string.IsNullOrEmpty(Voornaam))
                    {
                        return "Voornaam is een verplicht veld.";
                    }
                    if (columnName == "Adres" && string.IsNullOrEmpty(Adres))
                    {
                        return "Adres is een verplicht veld.";
                    }
                    if (columnName == "Postcode" && string.IsNullOrEmpty(Postcode))
                    {
                        return "Postcode is een verplicht veld.";
                    }
                    if (columnName == "Woonplaats" && string.IsNullOrEmpty(Woonplaats))
                    {
                        return "Woonplaats is een verplicht veld.";
                    }
                }
                return "";
            }
        }

        public bool IsLid
        {
            get
            {
                if (Lidegelden != null)
                {
                    if (Lidegelden.Count > 0)
                    {//Er zijn ooit lidgelden betaald
                        if (Lidegelden.Last().DuurLidmaatschap > DateTime.Now)
                        {//De duurlidmaatschap van het laatste lidgeld item kleiner dan huidige datum, dus klant is nu nog lid
                            return true;
                        }
                        else
                        {//Klant is nu geen lid meer. Duurlidmaatschap is verstreken.
                            return false;
                        }
                    }
                    else
                    {//Er zijn nooit lidgelden betaald, dus klant is geen lid
                        return false;
                    }
                }
                return false;
            }
        }
        public double getTotaalBoeteBedrag
        {
            get
            {
                double returnValue = 0;
                Uitleningen.Where(u => u.BoeteBedrag() > 0).ToList().ForEach(u => returnValue += u.BoeteBedrag());
                return returnValue;
            }
        }
    }
}