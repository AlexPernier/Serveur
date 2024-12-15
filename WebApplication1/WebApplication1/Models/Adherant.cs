using System.ComponentModel.DataAnnotations;

namespace SiteWebMultiSport.Models
{
    public class Adherant
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom est obligatoire")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "La date de naissance est obligatoire")]
        [DataType(DataType.Date)]
        public required string DateNaissance { get; set; }

        [Required(ErrorMessage = "L'email est obligatoire")]
        [EmailAddress(ErrorMessage = "Le format de l'email est invalide")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Le numéro de téléphone est obligatoire")]
        [Phone(ErrorMessage = "Le format du numéro de téléphone est invalide")]
        public required string Phone { get; set; }

        public bool IsAdmin { get; set; } 

        public bool IsEncadrant { get; set; }

   

    }
}
