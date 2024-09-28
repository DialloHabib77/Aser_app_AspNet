using System.ComponentModel.DataAnnotations;

namespace aser_electrification.Models
{
    public class Region
    {
        [Key]
        public int ID_Region { get; set; }

        [Required]
        [StringLength(100)]
        public string Nom_Region { get; set; }

       public virtual ICollection<Departement> Departements { get; set; }

      



    }
}
