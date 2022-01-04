using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_DAL.Models
{
    public partial class Uitlening
    {
        public int Id { get; set; }
        public int MediumId { get; set; }
        public int? GebruikerId { get; set; }
        public DateTime OntleenIntresse { get; set; }
        public DateTime? UitgeleendOp { get; set; }
        public DateTime? Binnengebracht { get; set; }
        public DateTime? BoeteBetaald { get; set; }
        public DateTime? LaatsteEmail { get; set; }

        //Nav props

        public Medium Medium { get; set; }
        public Gebruiker Gebruiker { get; set; }
    }
}