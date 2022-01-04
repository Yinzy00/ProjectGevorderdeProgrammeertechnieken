using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_DAL.Models
{
    public partial class Gebruiker
    {
        public int Id { get; set; }

        //[Required]
        public string Naam { get; set; }

        //[Required]
        public string Voornaam { get; set; }

        //[Required]
        public string Adres { get; set; }

        //[Required]
        public string Postcode { get; set; }

        //[Required]
        public string Woonplaats { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Wachtwoord { get; set; }

        [Required]
        public string Key { get; set; }
        [Required]
        public GebruikerType Admin { get; set; } = GebruikerType.Gebruiker;
        //Met de propery LidgeldId is het maar mogelijk 1x een lidgeld op te slaan per gebruiker (Heb GebruikerId toegevoegd aan Lidgeld klasse)
        //public int? LidgeldId { get; set; }

        public DateTime? LidmaatschapAanvraag { get; set; }

        //Nav props
        public ICollection<Uitlening> Uitleningen { get; set; }

        public ICollection<Lidgeld> Lidegelden { get; set; }
        public ICollection<GebruikerBoekenbeurs> GebruikerBoekenbeurzen { get; set; }
        public ICollection<MediumVerkoop> BoekVerkopen { get; set; }
    }

    public enum GebruikerType
    {
        Gebruiker,
        Beheerder,
        Super
    }
}