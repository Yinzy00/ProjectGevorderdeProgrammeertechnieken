using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_DAL.Models
{
    public partial class Medium
    {
        public int Id { get; set; }
        public int MediumDetailId { get; set; }
        public DateTime Registratie { get; set; }
        public DateTime EindeLevensduur { get; set; }
        public double Verkoopprijs { get; set; }
        public bool? Verkocht { get; set; }

        //Navigation props
        public MediumDetail MediumDetail { get; set; }

        public ICollection<Uitlening> Uitleningen { get; set; } = new List<Uitlening>();
        public ICollection<MediumVerkoop> MediumVerkoopen { get; set; }
    }
}