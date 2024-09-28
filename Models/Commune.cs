using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace aser_electrification.Models
{
    public class Commune
    {
        [Key]
        public int ID_Commune { get; set; }

        [Required]
        [StringLength(100)]
        public string Nom_Commune { get; set; }

        [ForeignKey("Departement")]
        public int ID_Departement { get; set; }

        public virtual Departement Departement { get; set; }
        public virtual ICollection<Village> Villages { get; set; }

    }
}
