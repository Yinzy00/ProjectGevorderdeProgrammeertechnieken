using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_DAL.Models
{
    public partial class MediumDetailExtra
    {
        public string DisplayMember
        {
            get
            {
                return $"{Extra.Type} : {Beschrijving}";
            }
        }
    }
}
