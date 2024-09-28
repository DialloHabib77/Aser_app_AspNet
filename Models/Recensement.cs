using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace aser_electrification.Models
{
    public class Recensement
    {
        [Key]
        public int ID_Recensement { get; set; }

        [Required]
        public DateTime Date_Recensement { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Nombre_Menages_Electrifies { get; set; }

        [ForeignKey("Village")]
        public int ID_Village { get; set; }

        public virtual Village Village { get; set; }

        [NotMapped]
        public float TauxAcces => Village != null && Village.Nombre_Menages > 0
            ? (float)Nombre_Menages_Electrifies / Village.Nombre_Menages
            : 0;

    }
}
