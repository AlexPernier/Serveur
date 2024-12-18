using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiteWebMultiSport.Models;

namespace SiteWebMultiSport.Controllers
{
    public class CreneauController : Controller
    {

        private readonly ApplicationDbContext _context;

      

        public CreneauController(ApplicationDbContext context)
        {
            _context = context;
        }

        private bool IsAdmin()
        {

  
            var adherantId = HttpContext.Session.GetString("AdherantId");
            if (adherantId == null) return false;

            var adherant = _context.Adherants.FirstOrDefault(a => a.Id.ToString() == adherantId);
            return adherant?.IsAdmin ?? false;
        }

        private bool IsEncadrant()
        {


            var adherantId = HttpContext.Session.GetString("AdherantId");
            if (adherantId == null) return false;

            var adherant = _context.Adherants.FirstOrDefault(a => a.Id.ToString() == adherantId);
            return adherant?.IsEncadrant ?? false;
        }
        public IActionResult Index()
        {
            if (!IsAdmin() && !IsEncadrant()) return Unauthorized();
            return View();
        }

        public IActionResult Create(int sectionId)
        {
            if (!IsAdmin() && !IsEncadrant()) return Unauthorized();
            ViewBag.SectionId = sectionId; // Envoyer l'ID de la section à la vue
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Creneau creneau)
        {

            if (!IsAdmin() && !IsEncadrant()) return Unauthorized();
            // Charger manuellement la section correspondante
            creneau.Section = _context.Sections.FirstOrDefault(s => s.Id == creneau.SectionId);

                // Vérifier si la section existe
                if (creneau.Section == null)
                {
                    ModelState.AddModelError("SectionId", "La section spécifiée est invalide.");
                }
   
                _context.Creneaux.Add(creneau);
                _context.SaveChanges();
                return RedirectToAction("Creneaux", "Section", new { id = creneau.SectionId });
               
        }

        public IActionResult Liste()
        {
            var adherantId = HttpContext.Session.GetString("AdherantId");
            if (adherantId == null)
            {
                return Unauthorized();
            }

            var creneaux = _context.Creneaux
                .Include(c => c.Section)
                .ThenInclude(s => s.Discipline)
                .Include(c => c.Adherants) // Charger les inscriptions
                .Where(c => !c.Adherants.Any(a => a.Id == int.Parse(adherantId))) // Exclure les créneaux déjà inscrits
                .ToList();

            return View(creneaux);
        }

        public IActionResult Details(int id)
        {
            var creneau = _context.Creneaux
                .Include(c => c.Adherants) // Inclure les adhérents associés
                .FirstOrDefault(c => c.Id == id);

            if (creneau == null)
            {
                return NotFound();
            }

            return View(creneau); // Retourner la vue avec le créneau et ses adhérents
        }


        [HttpPost]
        public IActionResult Inscription(int creneauId)
        {
            var adherantId = HttpContext.Session.GetString("AdherantId");
            if (adherantId == null)
            {
                return Unauthorized();
            }

            var adherant = _context.Adherants.FirstOrDefault(a => a.Id == int.Parse(adherantId));
            if (adherant == null)
            {
                return Unauthorized();
            }

            // Vérifier si l'adhérent est abonné
            if (!adherant.IsSubscribed)
            {
                TempData["ErrorMessage"] = "Vous devez payer un abonnement pour vous inscrire.";
                return RedirectToAction("PaySubscription", "Payment");
            }

            var creneau = _context.Creneaux
                .Include(c => c.Adherants)
                .FirstOrDefault(c => c.Id == creneauId);

            if (creneau == null)
            {
                return NotFound();
            }

            if (creneau.Adherants.Count >= creneau.Capacite)
            {
                ModelState.AddModelError("", "Ce créneau est complet.");
                return RedirectToAction("Liste");
            }

          

            if (!creneau.Adherants.Contains(adherant))
            {
                creneau.Adherants.Add(adherant);
                _context.SaveChanges();
            }

            return RedirectToAction("Liste");
        }

        //Méthode pour un adhérent qui souhaite se déinscrire
        [HttpPost]
        public IActionResult Desinscription(int creneauId)
        {
            var adherantId = HttpContext.Session.GetString("AdherantId");
            if (adherantId == null)
            {
                return Unauthorized();
            }

            // Récupérer l'adhérent
            var adherant = _context.Adherants
                .Include(a => a.Creneaux)
                .FirstOrDefault(a => a.Id == int.Parse(adherantId));

            if (adherant == null)
            {
                return NotFound();
            }

            // Retirer le créneau de la liste des créneaux de l'adhérent
            var creneau = adherant.Creneaux.FirstOrDefault(c => c.Id == creneauId);
            if (creneau != null)
            {
                adherant.Creneaux.Remove(creneau);
                _context.SaveChanges();
            }

            return RedirectToAction("MesCreneaux"); // Rediriger vers la vue des créneaux inscrits
        }

        //Méthode pour un admin ou un encadrant qui souhaite désinscrire un adhérant x d'un créneau y
        [HttpPost]
        public IActionResult Desinscrire(int creneauId, int adherantId)
        {
            var creneau = _context.Creneaux
                .Include(c => c.Adherants)
                .FirstOrDefault(c => c.Id == creneauId);

            if (creneau == null)
            {
                return NotFound();
            }

            var adherant = creneau.Adherants.FirstOrDefault(a => a.Id == adherantId);
            if (adherant != null)
            {
                creneau.Adherants.Remove(adherant);
                _context.SaveChanges();
            }

            return RedirectToAction("Details", new { id = creneauId });
        }


        public IActionResult MesCreneaux()
        {
            var adherantId = HttpContext.Session.GetString("AdherantId");
            if (adherantId == null)
            {
                return Unauthorized();
            }

            var creneaux = _context.Creneaux
                .Include(c => c.Section)
                .ThenInclude(s => s.Discipline)
                .Include(c => c.Adherants)
                .Where(c => c.Adherants.Any(a => a.Id == int.Parse(adherantId)))
                .ToList();

            return View(creneaux);
        }


        // Supprimer un créneau
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var creneau = _context.Creneaux
                .Include(c => c.Adherants) // Charger les relations avec les adhérents
                .FirstOrDefault(c => c.Id == id);

            if (creneau == null)
            {
                return NotFound();
            }

            // Supprimer manuellement les relations dans la table de jointure
            _context.Entry(creneau).Collection(c => c.Adherants).Load();
            creneau.Adherants.Clear(); // Supprime toutes les relations
            _context.SaveChanges();

            // Ensuite, supprimer le créneau
            _context.Creneaux.Remove(creneau);
            _context.SaveChanges();

            if (IsAdmin())
            {
                return RedirectToAction("Index", "Section");
            }
            else
            {
                return RedirectToAction("MesSections", "Section");
            }
           

        }

    }
}
