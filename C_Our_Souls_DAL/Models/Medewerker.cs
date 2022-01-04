using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_DAL.Models
{
    public partial class Medewerker
    {
        public int Id { get; set; }

        [Required]
        public string Naam { get; set; }

        [Required]
        public int FunctieId { get; set; }

        //Navigation properties
        public Functie Functie { get; set; }

        public ICollection<MediumDetailMedewerker> MediumDetailMedewerkers { get; set; }
    }
}