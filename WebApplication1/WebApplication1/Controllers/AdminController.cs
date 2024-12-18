using Microsoft.AspNetCore.Mvc;
using SiteWebMultiSport.Helpers;
using SiteWebMultiSport.Models;
using SiteWebMultiSport.Helpers;
using Microsoft.EntityFrameworkCore;
using static System.Collections.Specialized.BitVector32;

namespace SiteWebMultiSport.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
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

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!IsUserAdmin())
            {
                return Unauthorized();
            }

            var adherant = _context.Adherants.FirstOrDefault(a => a.Id == id);
            if (adherant == null)
            {
                return NotFound();
            }

            return View(adherant);
        }

        [HttpPost]
        public IActionResult Edit(Adherant adherant, string Password)
        {
            if (!IsUserAdmin())
            {
                return Unauthorized();
            }

            var existingAdherant = _context.Adherants.FirstOrDefault(a => a.Id == adherant.Id);
            if (existingAdherant == null)
            {
                return NotFound();
            }

            // Met à jour les champs existants
            existingAdherant.Name = adherant.Name;
            existingAdherant.Email = adherant.Email;
            existingAdherant.DateNaissance = adherant.DateNaissance;
            existingAdherant.Phone = adherant.Phone;
            existingAdherant.IsAdmin = adherant.IsAdmin;
            existingAdherant.IsEncadrant = adherant.IsEncadrant;
            existingAdherant.IsSubscribed = adherant.IsSubscribed;

            // Si un nouveau mot de passe est fourni, le hacher
            if (!string.IsNullOrEmpty(Password))
            {
                existingAdherant.PasswordHash = PasswordHelper.HashPassword(Password);
            }

            _context.Adherants.Update(existingAdherant);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            if (!IsUserAdmin())
            {
                return Unauthorized();
            }

            var adherant = _context.Adherants
                .Include(c => c.Creneaux) // Charger les relations avec les adhérents
                .FirstOrDefault(c => c.Id == id);

            if (adherant == null)
            {
                return NotFound();
            }

            // Supprimer manuellement les relations dans la table de jointure
            _context.Entry(adherant).Collection(c => c.Creneaux).Load();
            adherant.Creneaux.Clear(); // Supprime toutes les relations
            _context.SaveChanges();

            // Ensuite, supprimer l'adherant
            _context.Adherants.Remove(adherant);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        private bool IsUserAdmin()
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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Adherant adherant, string Password)
        {
           
            
                Console.WriteLine(adherant);
                // Hache le mot de passe avant de l'enregistrer
                adherant.PasswordHash = PasswordHelper.HashPassword(Password);
                _context.Adherants.Add(adherant);
                _context.SaveChanges();
                return RedirectToAction("Index");
            

            return View(adherant);  // Renvoyer la vue avec les erreurs de validation si le modèle n'est pas valide
        }




    }

}


