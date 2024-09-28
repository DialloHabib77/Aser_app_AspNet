using aser_electrification.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace aser_electrification.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Region> Regions { get; set; }
        public DbSet<Departement> Departements { get; set; }

        public DbSet<Commune> Communes { get; set; }

        public DbSet<Village> Villages { get; set; }

        public DbSet<Recensement> Recensements { get; set;}
    }
}
