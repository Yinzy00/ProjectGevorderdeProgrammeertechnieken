using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Our_Souls_DAL.Models
{
    public partial class Categorie
    {
        public int Id { get; set; }

        [Required]
        public string Omschrijving { get; set; }

        //Navigation props
        public ICollection<MediumCategorie> MediumCategorieen { get; set; }
    }
}