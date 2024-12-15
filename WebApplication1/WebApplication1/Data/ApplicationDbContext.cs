using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SiteWebMultiSport.Models;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }

    public DbSet<Adherant> Adherants { get; set; }
    public DbSet<Discipline> Disciplines { get; set; }
    public DbSet<Section> Sections { get; set; }
    public DbSet<Creneau> Creneaux { get; set; }



}
