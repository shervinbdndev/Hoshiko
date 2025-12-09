using Hoshiko.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Hoshiko.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Hoshiko.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

        public DbSet<User> UsersDomain {get;set;} = null!;
        public DbSet<Stage> Stages => Set<Stage>();
        public DbSet<Quiz> Quizzes => Set<Quiz>();
        public DbSet<Learn> Learns => Set<Learn>();
    }
}