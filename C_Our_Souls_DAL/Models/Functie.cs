using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_DAL.Models
{
    public class Functie
    {
        public int Id { get; set; }

        [Required]
        public string Naam { get; set; }

        //Navigation props
        public ICollection<Medewerker> Medewerkers { get; set; }
    }
}