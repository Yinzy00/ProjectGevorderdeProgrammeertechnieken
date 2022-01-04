using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_DAL.Models
{
    public partial class MediumDetail
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public int LeeftijdsKlasseId { get; set; }

        [Required]
        public string Ean { get; set; }

        public int SoortId { get; set; }
        public string KorteInhoud { get; set; }

        //Navigation props
        public ICollection<MediumDetailMedewerker> MediumDetailMedewerker { get; set; } = new List<MediumDetailMedewerker>();
        public ICollection<MediumCategorie> MediumCategorieen { get; set; } = new List<MediumCategorie>();
        public LeeftijdsKlasse LeeftijdsKlasse { get; set; }
        public Soort Soort { get; set; }
        public ICollection<Medium> Medium { get; set; } = new List<Medium>();
        public ICollection<MediumDetailExtra> MediumDetailExtras { get; set; } = new List<MediumDetailExtra>();
    }
}