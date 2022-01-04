using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_DAL.Models
{
    public class Lidgeld
    {
        public int Id { get; set; }
        public DateTime LidgeldBetaaldOp { get; set; }
        public DateTime DuurLidmaatschap { get; set; }
        public int GebruikerId { get; set; }
        public DateTime? LastEmail { get; set; }

        //Nav props
        public Gebruiker Gebruiker { get; set; }
    }
}