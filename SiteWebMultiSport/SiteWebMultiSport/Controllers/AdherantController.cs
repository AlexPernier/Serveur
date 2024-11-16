using Microsoft.AspNetCore.Mvc;
using SiteWebMultiSport.Data;
using SiteWebMultiSport.Models;
using System.Linq;

namespace SiteWebMultiSport.Controllers
{
    public class AdherantController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdherantController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Action pour afficher le formulaire d'inscription
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Action pour traiter l'inscription
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Adherant adherant)
        {
            if (ModelState.IsValid)
            {
                _context.Adherants.Add(adherant);
                _context.SaveChanges();
                return RedirectToAction("Success");
            }
            return View(adherant);
        }

        // Page de confirmation avec la liste des adhérents
        public IActionResult Success()
        {
            var adherants = _context.Adherants.ToList(); // Récupère tous les adhérents
            return View(adherants); // Passe la liste à la vue
        }

        // Action pour afficher la page de connexion
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Action pour traiter le formulaire de connexion
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                ModelState.AddModelError(string.Empty, "Le nom est obligatoire.");
                return View();
            }

            var adherant = _context.Adherants.FirstOrDefault(a => a.Name == name);
            if (adherant == null)
            {
                ModelState.AddModelError(string.Empty, "Aucun adhérant trouvé avec ce nom.");
                return View();
            }

            // Connexion réussie
            return RedirectToAction("Profile", new { id = adherant.Id });
        }

        // Action pour afficher le profil de l'adhérant
        public IActionResult Profile(int id)
        {
            var adherant = _context.Adherants.Find(id);
            if (adherant == null)
            {
                return NotFound();
            }
            return View(adherant);
        }
    }
}
