using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EFCProject.Models;
using Microsoft.AspNetCore.Identity;

namespace EFCProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<EFCProject.Models.Game>? Game { get; set; }
        public DbSet<EFCProject.Models.Favorit>? Favorit { get; set; }
        public DbSet<EFCProject.Models.ApplicationUser>? User { get; set; }

        public DbSet<EFCProject.Models.ModificationLog>? ModificationLog { get; set; }
        
    }
}


