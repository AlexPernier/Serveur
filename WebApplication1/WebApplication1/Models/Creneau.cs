using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteWebMultiSport.Models
{
    public class Creneau
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le jour est obligatoire.")]
        public string Jour { get; set; } // Par exemple, Lundi, Mardi...

        [Required(ErrorMessage = "L'heure de début est obligatoire.")]
        [DataType(DataType.Time)]
        public TimeSpan HeureDebut { get; set; }

        [Required(ErrorMessage = "L'heure de fin est obligatoire.")]
        [DataType(DataType.Time)]
        public TimeSpan HeureFin { get; set; }

        [Required(ErrorMessage = "Le lieu est obligatoire.")]
        public string Lieu { get; set; }

        [Required(ErrorMessage = "La capacité est obligatoire.")]
        [Range(1, int.MaxValue, ErrorMessage = "La capacité doit être au moins 1.")]
        public int Capacite { get; set; }

        // Relation avec la section
        public int SectionId { get; set; } // Clé étrangère
        [ForeignKey("SectionId")]
        public Section Section { get; set; } // Navigation property
                                             
        // Propriété de navigation pour Adherants
        public List<Adherant> Adherants { get; set; } = new();

    }
}
