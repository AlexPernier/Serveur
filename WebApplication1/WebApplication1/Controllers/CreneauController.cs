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
            var creneaux = _context.Creneaux
                .Include(c => c.Section)
                .ThenInclude(s => s.Discipline)
                .Include(c => c.Adherants) // Charger les inscriptions
                .ToList();

            return View(creneaux);
        }

        [HttpPost]
        public IActionResult Inscription(int creneauId)
        {
            var adherantId = HttpContext.Session.GetString("AdherantId");
            if (adherantId == null)
            {
                return Unauthorized();
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

            var adherant = _context.Adherants.FirstOrDefault(a => a.Id == int.Parse(adherantId));
            if (adherant == null)
            {
                return Unauthorized();
            }

            if (!creneau.Adherants.Contains(adherant))
            {
                creneau.Adherants.Add(adherant);
                _context.SaveChanges();
            }

            return RedirectToAction("Liste");
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

            return RedirectToAction("Index", "Section");

        }

    }
}
