using Microsoft.AspNetCore.Mvc;
using SiteWebMultiSport.Models;

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
    public IActionResult Edit(Adherant adherant)
    {
        if (!IsUserAdmin())
        {
            return Unauthorized();
        }

        if (ModelState.IsValid)
        {
            _context.Adherants.Update(adherant);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        return View(adherant);
    }

    public IActionResult Delete(int id)
    {
        if (!IsUserAdmin())
        {
            return Unauthorized();
        }

        var adherant = _context.Adherants.FirstOrDefault(a => a.Id == id);
        if (adherant != null)
        {
            _context.Adherants.Remove(adherant);
            _context.SaveChanges();
        }

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
    public IActionResult Create(Adherant adherant)
    {
        if (ModelState.IsValid)
        {
            Console.WriteLine(adherant);
            _context.Adherants.Add(adherant);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        return View(adherant);  // Renvoyer la vue avec les erreurs de validation si le modèle n'est pas valide
    }




}

