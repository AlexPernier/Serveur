using Microsoft.AspNetCore.Mvc;
using SiteWebMultiSport.Models;

namespace SiteWebMultiSport.Controllers
{
    public class AdherantController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdherantController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (!IsUserAdmin())
            {
                return Unauthorized(); // Redirige si l'utilisateur n'est pas admin
            }

            var adherants = _context.Adherants.ToList();
            return View(adherants);
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

            // Recherche de l'adhérent
            var adherant = _context.Adherants.FirstOrDefault(a => a.Name == name);
            if (adherant == null)
            {
                ModelState.AddModelError(string.Empty, "Aucun adhérent trouvé avec ce nom.");
                return View();
            }

            // Enregistre les informations importantes dans la session
            HttpContext.Session.SetString("AdherantId", adherant.Id.ToString());
            HttpContext.Session.SetString("AdherantName", adherant.Name); // Nom de l'adhérent
            HttpContext.Session.SetString("IsAdmin", adherant.IsAdmin ? "true" : "false"); // Détermine si l'adhérent est un admin
            HttpContext.Session.SetString("IsEncadrant", adherant.IsEncadrant ? "true" : "false"); // Détermine si l'adhérent est un encadrant

            // Redirection conditionnelle en fonction du rôle
            if (adherant.IsAdmin)
            {
                // Redirige vers une page d'administration
                return RedirectToAction("Index", "Admin");
            }

            // Redirige vers la page de profil si ce n'est pas un admin
            return RedirectToAction("Profile", new { id = adherant.Id });
        }


        // Action pour afficher le profil de l'adhérant
        public IActionResult Profile(int id)
        {
            // Récupère l'ID de l'adhérent connecté depuis la session
            var sessionAdherantId = HttpContext.Session.GetString("AdherantId");

            if (sessionAdherantId == null || id.ToString() != sessionAdherantId)
            {
                // Si aucun adhérent connecté ou tentative d'accès à un autre profil, renvoie une erreur
                return Unauthorized();
            }

            // Récupère les informations de l'adhérent depuis la base de données
            var adherant = _context.Adherants.Find(id);
            if (adherant == null)
            {
                return NotFound();
            }

            return View(adherant);
        }


        //Action pour verifier si l'utilisateur est admin
        public bool IsUserAdmin()
        {
            if (HttpContext.Session.GetString("AdherantId") != null)
            {
                var adherantId = int.Parse(HttpContext.Session.GetString("AdherantId"));
                var adherant = _context.Adherants.FirstOrDefault(a => a.Id == adherantId);
                return adherant?.IsAdmin ?? false;
            }
            return false;
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Supprime toutes les données de session
            return RedirectToAction("Login");
        }
    }
}
