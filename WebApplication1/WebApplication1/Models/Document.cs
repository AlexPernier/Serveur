using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SiteWebMultiSport.Models
{
    public class Document
    {
        public int Id { get; set; }

        [Required]
        public string FileName { get; set; } // Nom du fichier

        [Required]
        public byte[] Content { get; set; } // Contenu du fichier (stocké en base)

        public string ContentType { get; set; } // Type MIME, ex: "application/pdf"

        public DateTime UploadDate { get; set; } = DateTime.Now;

        public bool IsValidated { get; set; } = false; // Statut de validation

        public int AdherantId { get; set; }
        [ForeignKey("AdherantId")]
        public Adherant Adherant { get; set; } // Relation avec l'adhérent
    }
}
