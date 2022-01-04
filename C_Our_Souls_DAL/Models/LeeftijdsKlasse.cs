using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_DAL.Models
{
    public class LeeftijdsKlasse
    {
        public int Id { get; set; }
        [Required]
        public string Omschrijving { get; set; }
    }
}
