using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_DAL.Models
{
    public class MediumDetailMedewerker
    {
        public MediumDetailMedewerker()
        {
        }

        public int Id { get; set; }
        public int MedewerkerId { get; set; }
        public int MediumDetailId { get; set; }

        //Navigation props
        public Medewerker Medewerker { get; set; }

        public MediumDetail MediumDetail { get; set; }
    }
}