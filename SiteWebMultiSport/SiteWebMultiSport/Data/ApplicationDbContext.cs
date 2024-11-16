using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SiteWebMultiSport.Models;

namespace SiteWebMultiSport.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Adherant> Adherants { get; set; }
        public DbSet<Discipline> Disciplines { get; set; }
    }

    public class Discipline
    {
        public int Id { get; set; }
        public string Name { get; set; }
     
    }
}
