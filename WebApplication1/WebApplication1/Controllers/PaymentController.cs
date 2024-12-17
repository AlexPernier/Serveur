using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PaymentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult PaySubscription()
        {
            return View(); // Vue avec le formulaire de paiement
        }

        [HttpPost]
        public IActionResult PaySubscription(int adherantId)
        {
            var adherant = _context.Adherants.Find(adherantId);
            if (adherant == null)
            {
                return NotFound();
            }

            // Simulation du paiement
            adherant.IsSubscribed = true;
            HttpContext.Session.SetString("IsSubscribed", adherant.IsSubscribed ? "true" : "false"); // Détermine si l'adhérent est un abonné
            // Ajouter une trace dans la table Payment
            var payment = new Payment
            {
                AdherantId = adherantId,
                PaymentDate = DateTime.Now,
                Amount = 50, // Exemple de montant fixe
                PaymentMethod = "Simulation"
            };
            _context.Payments.Add(payment);
            _context.SaveChanges();

            return RedirectToAction("Liste", "Creneau"); // Redirige vers la liste des créneaux
        }
    }

}
