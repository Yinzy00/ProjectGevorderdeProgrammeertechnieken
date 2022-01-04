using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_DAL.Models
{
    public partial class Medewerker
    {
        public override bool Equals(object obj)
        {
            return obj is Medewerker medewerker &&
                   Naam == medewerker.Naam &&
                   FunctieId == medewerker.FunctieId;
        }

        public override int GetHashCode()
        {
            int hashCode = -1128741780;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Naam);
            hashCode = hashCode * -1521134295 + FunctieId.GetHashCode();
            return hashCode;
        }
    }
}