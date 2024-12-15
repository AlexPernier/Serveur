using Microsoft.AspNetCore.Mvc;
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
            var discipline = _context.Disciplines.Find(id);
            if (discipline != null)
            {
                _context.Disciplines.Remove(discipline);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}

