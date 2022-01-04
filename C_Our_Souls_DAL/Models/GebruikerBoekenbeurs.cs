using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_DAL.Models
{
    public class GebruikerBoekenbeurs
    {
        public int Id { get; set; }
        public int GebruikerId { get; set; }
        public int BoekenbeursId { get; set; }
        public DateTime IngeschrevenOp { get; set; }

        //Nav props
        public Gebruiker Gebruiker { get; set; }
        public Boekenbeurs Boekenbeurs { get; set; }
    }
}
