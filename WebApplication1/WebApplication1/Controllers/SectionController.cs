using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiteWebMultiSport.Models;
using static System.Collections.Specialized.BitVector32;
using Section = SiteWebMultiSport.Models.Section;

namespace SiteWebMultiSport.Controllers
{
    public class SectionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SectionController(ApplicationDbContext context)
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

        [HttpGet]
        public IActionResult Index()
        {
            if (!IsAdmin() && !IsEncadrant())
            {
                return Unauthorized(); // Renvoie une erreur si l'utilisateur n'est pas admin
            }

            // Récupération des sections avec les données nécessaires
            var sections = _context.Sections
                .Include(s => s.Discipline)  // Inclure la discipline associée
                .Include(s => s.Encadrant)   // Inclure les données d'Adherant pour l'encadrant
                .Select(s => new
                {
                    s.Id,
                    s.Nom,
                    Discipline = s.Discipline.Nom, // Nom de la discipline
                    Encadrant = s.Encadrant.IsEncadrant ? s.Encadrant.Name : "Aucun encadrant" // Vérifie si c'est un encadrant
                })
                .ToList();

            return View(sections);
        }


        [HttpGet]
        public IActionResult Create()
        {
            if (!IsAdmin()) return Unauthorized();

            ViewBag.Disciplines = _context.Disciplines.ToList();
            ViewBag.Encadrants = _context.Adherants.Where(a => a.IsEncadrant).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Section section)
        {
            if (!IsAdmin()) return Unauthorized();

            // Vérifiez les valeurs envoyées par le formulaire
            if (section.DisciplineId == 0)
            {
                ModelState.AddModelError("DisciplineId", "Veuillez sélectionner une discipline valide.");
            }
            if (section.EncadrantId == 0)
            {
                ModelState.AddModelError("EncadrantId", "Veuillez sélectionner un encadrant valide.");
            }

            // Charger les objets Discipline et Encadrant à partir des IDs
            var discipline = _context.Disciplines.FirstOrDefault(d => d.Id == section.DisciplineId);
            var encadrant = _context.Adherants.FirstOrDefault(a => a.Id == section.EncadrantId);

            // Vérifier si les objets sont trouvés et ajouter des erreurs pour chaque champ
            if (discipline == null)
            {
                ModelState.AddModelError("DisciplineId", "La discipline sélectionnée est invalide.");
            }
            if (encadrant == null)
            {
                ModelState.AddModelError("EncadrantId", "L'encadrant sélectionné est invalide.");
            }

           
            
                section.Discipline = discipline;
                section.Encadrant = encadrant;

            _context.Sections.Add(section);
           _context.SaveChanges();
           return RedirectToAction("Index");
        }




        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!IsAdmin() && !IsEncadrant()) {
                return Unauthorized();
            }
            var section = _context.Sections.Find(id);
            if (section == null) return NotFound();

            ViewBag.Disciplines = _context.Disciplines.ToList();
            ViewBag.Encadrants = _context.Adherants.Where(a => a.IsEncadrant).ToList();
            return View(section);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Section section)
        {
            if (!IsAdmin() && !IsEncadrant()) {
                return Unauthorized();
            }

            // Vérifiez les valeurs envoyées par le formulaire
            if (section.DisciplineId == 0)
            {
                ModelState.AddModelError("DisciplineId", "Veuillez sélectionner une discipline valide.");
            }
            if (section.EncadrantId == 0)
            {
                ModelState.AddModelError("EncadrantId", "Veuillez sélectionner un encadrant valide.");
            }

            // Charger les objets Discipline et Encadrant à partir des IDs
            var discipline = _context.Disciplines.FirstOrDefault(d => d.Id == section.DisciplineId);
            var encadrant = _context.Adherants.FirstOrDefault(a => a.Id == section.EncadrantId);

            // Vérifier si les objets sont trouvés et ajouter des erreurs pour chaque champ
            if (discipline == null)
            {
                ModelState.AddModelError("DisciplineId", "La discipline sélectionnée est invalide.");
            }
            if (encadrant == null)
            {
                ModelState.AddModelError("EncadrantId", "L'encadrant sélectionné est invalide.");
            }

            section.Discipline = discipline;
            section.Encadrant = encadrant;

            _context.Sections.Update(section);
            _context.SaveChanges();
            if (IsAdmin())
            {
                return RedirectToAction("Index");
            } else
            {
                return RedirectToAction("MesSections");
            }
            
        }

        [HttpPost]
        public IActionResult Delete(int sectionId)
        {
            if (!IsAdmin() && !IsEncadrant()) return Unauthorized();

            // Récupère la section à supprimer avec ses créneaux associés
            var section = _context.Sections
                .Include(s => s.Creneaux)
                .ThenInclude(c => c.Adherants)  // Inclure les adhérents inscrits aux créneaux
                .FirstOrDefault(s => s.Id == sectionId);

            if (section == null)
            {
                return NotFound(); // Si la section n'existe pas, retourner une erreur 404
            }

            // Dissocier les adhérents de tous les créneaux associés à cette section
            foreach (var creneau in section.Creneaux)
            {
                foreach (var adherant in creneau.Adherants.ToList()) // On utilise ToList() pour itérer sur la collection tout en la modifiant
                {
                    creneau.Adherants.Remove(adherant); // Retirer l'adhérent du créneau
                }
            }

            // Supprimer les créneaux associés à cette section
            _context.Creneaux.RemoveRange(section.Creneaux);

            // Supprimer la section
            _context.Sections.Remove(section);

            // Sauvegarder les changements dans la base de données
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Creneaux(int id)
        {
            var section = _context.Sections
                .Include(s => s.Creneaux)                // Inclut les créneaux
                    .ThenInclude(c => c.Adherants)       // Inclut les adhérents pour chaque créneau
                .FirstOrDefault(s => s.Id == id);

            if (section == null)
            {
                return NotFound();
            }

            return View(section);
        }


        public IActionResult MesSections()
        {
            var adherantId = HttpContext.Session.GetString("AdherantId");
            if (adherantId == null)
            {
                return Unauthorized();
            }

            var sections = _context.Sections
                .Include(s => s.Discipline) // Charger la relation Discipline
                .Include(s => s.Encadrant)  // Charger la relation Encadrant si nécessaire
                .Where(s => s.EncadrantId == int.Parse(adherantId)) // Sections gérées par l'encadrant
                .ToList();

            return View(sections);
        }



    }


}
