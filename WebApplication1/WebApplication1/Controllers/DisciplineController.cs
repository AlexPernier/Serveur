using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiteWebMultiSport.Models;

namespace SiteWebMultiSport.Controllers
{
    public class DisciplineController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DisciplineController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Afficher toutes les disciplines
        public IActionResult Index()
        {
            var disciplines = _context.Disciplines.ToList();
            return View(disciplines);
        }

        // Afficher le formulaire pour ajouter une discipline
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // Ajouter une discipline
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Discipline discipline)
        {
            if (ModelState.IsValid)
            {
                _context.Disciplines.Add(discipline);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(discipline);
        }

        // Supprimer une discipline
        public IActionResult Delete(int id)
        {
            var discipline = _context.Disciplines
       .Include(d => d.Sections)
       .ThenInclude(s => s.Creneaux)
       .ThenInclude(c => c.Adherants) // Inclut les adhérents des créneaux
       .FirstOrDefault(d => d.Id == id);

            if (discipline == null)
            {
                return NotFound();
            }

            // Supprimer les adhérents des créneaux avant de supprimer la discipline
            foreach (var section in discipline.Sections)
            {
                foreach (var creneau in section.Creneaux)
                {
                    foreach (var adherant in creneau.Adherants.ToList())
                    {
                        creneau.Adherants.Remove(adherant); // Retirer l'adhérent du créneau
                    }
                }
            }

            // Maintenant, vous pouvez supprimer la discipline sans violer les contraintes
            _context.Disciplines.Remove(discipline);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

