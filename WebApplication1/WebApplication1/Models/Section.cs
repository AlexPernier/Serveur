using System.ComponentModel.DataAnnotations;

namespace SiteWebMultiSport.Models
{
    public class Section
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom de la section est obligatoire.")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "La discipline est obligatoire.")]
        public int DisciplineId { get; set; }
        public Discipline Discipline { get; set; }

        [Required(ErrorMessage = "L'encadrant est obligatoire.")]
        public int EncadrantId { get; set; }
        public Adherant Encadrant { get; set; }

        // Liste des créneaux pour cette section
        public ICollection<Creneau> Creneaux { get; set; } = new List<Creneau>();
    }

}

