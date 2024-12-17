using SiteWebMultiSport.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int AdherantId { get; set; }
        public Adherant Adherant { get; set; }
        public DateTime PaymentDate { get; set; }
       
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } // Montant payé
        public string PaymentMethod { get; set; } // Exemple : "Carte bancaire", "PayPal"
    }

}
