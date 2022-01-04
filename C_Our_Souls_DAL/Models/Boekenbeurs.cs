using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_DAL.Models
{
    public class Boekenbeurs
    {
        public int Id { get; set; }

        [Required]
        public string Naam { get; set; }

        public DateTime DatumVan { get; set; }
        public DateTime? DatumTot { get; set; }
        public string Locatie { get; set; }
        public double InkomPrijs { get; set; }

        //Nav props
        public ICollection<GebruikerBoekenbeurs> GebruikerBoekenbeurzen { get; set; }
    }
}