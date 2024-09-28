using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace aser_electrification.Models
{
    public class Village
    {
        [Key]
        public int ID_Village { get; set; }

        [Required]
        [StringLength(100)]
        public string Nom_Village { get; set; }

        [Required]
        [StringLength(20)]
        public string Identifiant_Village { get; set; }

        [Required]
        [Range(-180, 180)]
        public float Longitude { get; set; }

        [Required]
        [Range(-90, 90)]
        public float Latitude { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Nombre_Menages { get; set; }

        [Required]
        public bool Statut_Electrification { get; set; }

        [ForeignKey("Commune")]
        public int ID_Commune { get; set; }

        public virtual Commune Commune { get; set; }

        public virtual ICollection<Recensement> Recensements { get; set; }



    }
}
