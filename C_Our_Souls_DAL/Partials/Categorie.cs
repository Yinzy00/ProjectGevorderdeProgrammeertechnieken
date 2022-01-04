using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_DAL.Models
{
    public partial class Categorie
    {
        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(Categorie))
            {
                if ((obj as Categorie).Id == Id)
                {
                    return true;
                }
            }
            return false;
        }
        //public override bool Equals(object obj)
        //{
        //    return obj is Categorie categorie &&
        //           Id == categorie.Id;
        //}

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }
    }
}