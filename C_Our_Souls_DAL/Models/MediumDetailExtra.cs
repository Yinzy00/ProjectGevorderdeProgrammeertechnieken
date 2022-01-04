using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_DAL.Models
{
    public partial class MediumDetailExtra
    {
        public int Id { get; set; }
        public int MediumDetailId { get; set; }
        public int ExtraId { get; set; }
        public string Beschrijving { get; set; }

        //Nav props
        public MediumDetail MediumDetail { get; set; }
        public Extra Extra { get; set; }
    }
}
