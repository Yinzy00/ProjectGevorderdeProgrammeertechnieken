using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_DAL.Models
{
    public class MediumCategorie
    {
        public int Id { get; set; }
        public int MediumDetailId { get; set; }
        public int CategorieId { get; set; }

        //Navigation props
        public MediumDetail MediumDetail { get; set; }
        public Categorie Categorie { get; set; }
    }
}
