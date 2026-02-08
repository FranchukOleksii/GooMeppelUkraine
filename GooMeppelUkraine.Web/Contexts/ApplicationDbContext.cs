using GooMeppelUkraine.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GooMeppelUkraine.Web.Contexts
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Article> Articles => Set<Article>();
        public DbSet<Stat> Stats => Set<Stat>();
        public DbSet<Partner> Partners => Set<Partner>();
        public DbSet<TeamMember> TeamMembers => Set<TeamMember>();

    }
}
