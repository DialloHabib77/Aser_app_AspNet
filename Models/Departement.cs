using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace aser_electrification.Models
{
    public class Departement
    {
        [Key]
        public int ID_Departement { get; set; }

        [Required]
        [StringLength(100)]
        public string Nom_Departement { get; set; }

        [ForeignKey("Region")]
        public int ID_Region { get; set; }

        public virtual Region Region { get; set; }

        public virtual ICollection<Commune> Communes { get; set; }




    }
}
