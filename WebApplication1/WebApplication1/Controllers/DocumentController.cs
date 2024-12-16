using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SiteWebMultiSport.Models;

namespace WebApplication1.Controllers
{
    public class DocumentController : Controller
    {

        private readonly ApplicationDbContext _context;

        public DocumentController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var adherantId = HttpContext.Session.GetString("AdherantId");
            if (adherantId == null)
            {
                return Unauthorized();
            }

            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Le fichier est vide.");
                return View();
            }

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var document = new SiteWebMultiSport.Models.Document
                {
                    FileName = file.FileName,
                    Content = memoryStream.ToArray(),
                    ContentType = file.ContentType,
                    AdherantId = int.Parse(adherantId)
                };

                _context.Documents.Add(document);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("MyDocuments");
        }

        public IActionResult MyDocuments()
        {
            var adherantId = HttpContext.Session.GetString("AdherantId");
            if (adherantId == null)
            {
                return Unauthorized();
            }

            var documents = _context.Documents
                .Where(d => d.AdherantId == int.Parse(adherantId))
                .ToList();

            return View(documents);
        }

        public IActionResult Download(int id)
        {
            var document = _context.Documents.Find(id);
            if (document == null)
            {
                return NotFound();
            }

            return File(document.Content, document.ContentType, document.FileName);
        }

        [HttpGet]
        public IActionResult PendingDocuments()
        {
            var documents = _context.Documents
           .Where(d => !d.IsValidated) // Exemple de filtre pour les documents non validés
           .ToList();

            foreach (var item in documents)
            {
               var adherant = _context.Adherants.FirstOrDefault(a => a.Id == item.AdherantId);
               item.Adherant = adherant;
            }
            
            return View(documents ?? new List<SiteWebMultiSport.Models.Document>());

        }


        [HttpPost]
        public IActionResult Validate(int id)
        {
            var document = _context.Documents.Find(id);
            if (document == null)
            {
                return NotFound();
            }

            document.IsValidated = true;
            _context.SaveChanges();

            return RedirectToAction("PendingDocuments"); // Liste des documents à valider
        }




    }
}
