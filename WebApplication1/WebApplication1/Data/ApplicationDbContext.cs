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
    public DbSet<Document> Documents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuration Many-to-Many entre Adherant et Creneau
        modelBuilder.Entity<Adherant>()
            .HasMany(a => a.Creneaux)
            .WithMany(c => c.Adherants)
            .UsingEntity<Dictionary<string, object>>(
                "AdherantCreneau",
                j => j
                    .HasOne<Creneau>()
                    .WithMany()
                    .HasForeignKey("CreneauxId")
                    .OnDelete(DeleteBehavior.Restrict), // Évite les cascade paths
                j => j
                    .HasOne<Adherant>()
                    .WithMany()
                    .HasForeignKey("AdherantsId")
                    .OnDelete(DeleteBehavior.Restrict) // Évite les cascade paths
            );
    }




}
