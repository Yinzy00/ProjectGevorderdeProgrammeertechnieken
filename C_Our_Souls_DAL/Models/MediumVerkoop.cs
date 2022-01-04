using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_DAL.Models
{
    public class MediumVerkoop
    {
        public int Id { get; set; }
        public DateTime IntresseDatum { get; set; }
        public int MediumId { get; set; }
        public int GebruikerId { get; set; }

        //Nav props
        public Gebruiker Gebruiker { get; set; }
        public Medium Medium { get; set; }
    }
}
