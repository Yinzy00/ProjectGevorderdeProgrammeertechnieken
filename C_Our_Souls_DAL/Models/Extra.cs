using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_DAL.Models
{
    public class Extra
    {
        public int Id { get; set; }
        public int SoortId { get; set; }
        [Required]
        public string Type { get; set; }
        //Nav props
        public Soort Soort{ get; set; }
        public ICollection<MediumDetailExtra> MediumDetailExtras { get; set; } = new List<MediumDetailExtra>();
    }
}
